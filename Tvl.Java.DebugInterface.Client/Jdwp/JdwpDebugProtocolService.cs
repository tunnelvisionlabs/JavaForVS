namespace Tvl.Java.DebugInterface.Client.Jdwp
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Tvl.Java.DebugInterface.Client.DebugProtocol;
    using Tvl.Java.DebugInterface.Types;
    using Tvl.Java.DebugInterface.Types.Loader;

    public class JdwpDebugProtocolService : IDebugProtocolService
    {
        private const int HeaderSize = 11;

        private readonly IDebugProtocolServiceCallback _callback;

        private readonly CancellationTokenSource _cancellationTokenSource =
            new CancellationTokenSource();
        private readonly ConcurrentDictionary<int, TaskCompletionSource<byte[]>> _tasks =
            new ConcurrentDictionary<int, TaskCompletionSource<byte[]>>();

        private int _nextMessageId = 1;

        private bool _handshakeComplete;
        private int? _fieldIdSize;
        private int? _methodIdSize;
        private int? _objectIdSize;
        private int? _referenceTypeIdSize;
        private int? _frameIdSize;

        private Socket _socket;
        private readonly LinkedList<byte[]> _buffers = new LinkedList<byte[]>();

        public JdwpDebugProtocolService(IDebugProtocolServiceCallback callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            _callback = callback;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(IPAddress.Loopback, 6777);
        }

        private int ObjectIdSize
        {
            get
            {
                return _objectIdSize.Value;
            }
        }

        private int ThreadIdSize
        {
            get
            {
                return ObjectIdSize;
            }
        }

        private int ReferenceTypeIdSize
        {
            get
            {
                return _referenceTypeIdSize.Value;
            }
        }

        private int ClassIdSize
        {
            get
            {
                return ReferenceTypeIdSize;
            }
        }

        private int FieldIdSize
        {
            get
            {
                return _fieldIdSize.Value;
            }
        }

        private int MethodIdSize
        {
            get
            {
                return _methodIdSize.Value;
            }
        }

        private int FrameIdSize
        {
            get
            {
                return _frameIdSize.Value;
            }
        }

        private int LocationSize
        {
            get
            {
                return sizeof(byte) + ReferenceTypeIdSize + MethodIdSize + sizeof(long);
            }
        }

        private void ReceiveAsync()
        {
            if (!_handshakeComplete)
                throw new InvalidOperationException();

            try
            {
                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                e.SetBuffer(new byte[1024], 0, 1024);
                e.RemoteEndPoint = _socket.RemoteEndPoint;
                e.Completed += HandleSocketReceive;
                if (!_socket.ReceiveAsync(e))
                    HandleSocketReceive(this, e);
            }
            catch
            {
                _cancellationTokenSource.Cancel();
                throw;
            }
        }

        private void HandleSocketReceive(object sender, SocketAsyncEventArgs e)
        {
            byte[] data = new byte[e.BytesTransferred];
            Buffer.BlockCopy(e.Buffer, e.Offset, data, 0, e.BytesTransferred);
            _buffers.AddLast(data);

            // process messages in _buffers
            while (TryProcessNextMessage())
            {
                // intentionally empty
            }

            if (!_socket.Connected)
            {
                _cancellationTokenSource.Cancel();
                return;
            }

            // start the next receive operation
            Task.Factory.StartNew(ReceiveAsync).HandleNonCriticalExceptions();
        }

        private bool TryProcessNextMessage()
        {
            if (_buffers.Count == 0)
                return false;

            byte[] segment = _buffers.First.Value;
            _buffers.RemoveFirst();
            while (segment.Length < HeaderSize && _buffers.Count > 0)
            {
                // combine the segments
                byte[] nextSegment = _buffers.First.Value;
                _buffers.RemoveFirst();
                byte[] combined = new byte[segment.Length + nextSegment.Length];
                Buffer.BlockCopy(segment, 0, combined, 0, segment.Length);
                Buffer.BlockCopy(nextSegment, 0, combined, segment.Length, nextSegment.Length);
                segment = combined;
            }

            if (segment.Length < HeaderSize)
            {
                _buffers.AddFirst(segment);
                return false;
            }

            // read the length of the packet to make sure we actually have the full packet
            int offset = 0;
            int packetSize = ReadInt32(segment, ref offset);
            while (segment.Length < packetSize && _buffers.Count > 0)
            {
                // combine the segments
                byte[] nextSegment = _buffers.First.Value;
                _buffers.RemoveFirst();
                byte[] combined = new byte[segment.Length + nextSegment.Length];
                Buffer.BlockCopy(segment, 0, combined, 0, segment.Length);
                Buffer.BlockCopy(nextSegment, 0, combined, segment.Length, nextSegment.Length);
                segment = combined;
            }

            if (segment.Length < packetSize)
            {
                _buffers.AddFirst(segment);
                return false;
            }

            byte[] packet = new byte[packetSize];
            Buffer.BlockCopy(segment, 0, packet, 0, packetSize);
            if (segment.Length > packetSize)
            {
                byte[] remaining = new byte[segment.Length - packetSize];
                Buffer.BlockCopy(segment, packetSize, remaining, 0, segment.Length - packetSize);
                _buffers.AddFirst(remaining);
            }

            Task.Factory.StartNew(() => ProcessPacket(packet)).HandleNonCriticalExceptions();
            return true;
        }

        private void ProcessPacket(byte[] packet)
        {
            int offset = sizeof(int);
            int id = ReadInt32(packet, ref offset);
            TaskCompletionSource<byte[]> completionSource;
            if (_tasks.TryRemove(id, out completionSource))
            {
                completionSource.SetResult(packet);
                return;
            }

            // this is probably an event if we get here
            CommandSet commandSet = (CommandSet)packet[sizeof(int) + sizeof(int) + sizeof(byte)];
            if (commandSet != CommandSet.Event)
                throw new NotImplementedException();

            ProcessEventPacket(packet);
        }

        private void ProcessEventPacket(byte[] packet)
        {
            if (!_objectIdSize.HasValue)
            {
                RequestIdSizes();
            }

            int offset = HeaderSize;
            SuspendPolicy suspendPolicy = (SuspendPolicy)ReadByte(packet, ref offset);
            int eventCount = ReadInt32(packet, ref offset);
            for (int i = 0; i < eventCount; i++)
            {
                EventKind eventKind = (EventKind)ReadByte(packet, ref offset);
                switch (eventKind)
                {
                case EventKind.SingleStep:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        RequestId mappedRequestId;
                        if (_requestRemap.TryGetValue(requestId, out mappedRequestId))
                            requestId = mappedRequestId;

                        ThreadId threadId = (ThreadId)ReadObjectId(packet, ref offset);
                        Location location = ReadLocation(packet, ref offset);
                        _callback.SingleStep(suspendPolicy, requestId, threadId, location);
                    }
                    continue;

                case EventKind.Breakpoint:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        ThreadId threadId = (ThreadId)ReadObjectId(packet, ref offset);
                        Location location = ReadLocation(packet, ref offset);
                        _callback.Breakpoint(suspendPolicy, requestId, threadId, location);
                    }
                    continue;

                case EventKind.FramePop:
                    throw new NotImplementedException();

                case EventKind.Exception:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        ThreadId threadId = (ThreadId)ReadObjectId(packet, ref offset);
                        Location location = ReadLocation(packet, ref offset);
                        TaggedObjectId exception = ReadTaggedObjectId(packet, ref offset);
                        Location catchLocation = ReadLocation(packet, ref offset);
                        _callback.Exception(suspendPolicy, requestId, threadId, location, exception, catchLocation);
                    }
                    continue;

                case EventKind.UserDefined:
                    throw new NotImplementedException();

                case EventKind.ThreadStart:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        ThreadId threadId = (ThreadId)ReadObjectId(packet, ref offset);
                        _callback.ThreadStart(suspendPolicy, requestId, threadId);
                    }
                    continue;

                case EventKind.ThreadDeath:
                //case EventKind.ThreadEnd:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        ThreadId threadId = (ThreadId)ReadObjectId(packet, ref offset);
                        _callback.ThreadDeath(suspendPolicy, requestId, threadId);
                    }
                    continue;

                case EventKind.ClassPrepare:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        ThreadId threadId = (ThreadId)ReadObjectId(packet, ref offset);
                        TypeTag typeTag = (TypeTag)ReadByte(packet, ref offset);
                        ReferenceTypeId typeId = ReadReferenceTypeId(packet, ref offset);
                        string signature = ReadString(packet, ref offset);
                        ClassStatus status = (ClassStatus)ReadInt32(packet, ref offset);
                        _callback.ClassPrepare(suspendPolicy, requestId, threadId, typeTag, typeId, signature, status);
                    }
                    continue;

                case EventKind.ClassUnload:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        string signature = ReadString(packet, ref offset);
                        _callback.ClassUnload(suspendPolicy, requestId, signature);
                    }
                    continue;

                case EventKind.ClassLoad:
                    throw new NotImplementedException();

                case EventKind.FieldAccess:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        ThreadId threadId = (ThreadId)ReadObjectId(packet, ref offset);
                        Location location = ReadLocation(packet, ref offset);
                        TypeTag typeTag = (TypeTag)ReadByte(packet, ref offset);
                        ReferenceTypeId typeId = ReadReferenceTypeId(packet, ref offset);
                        FieldId fieldId = ReadFieldId(packet, ref offset);
                        TaggedObjectId objectId = ReadTaggedObjectId(packet, ref offset);
                        _callback.FieldAccess(suspendPolicy, requestId, threadId, location, typeTag, typeId, fieldId, objectId);
                    }
                    continue;

                case EventKind.FieldModification:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        ThreadId threadId = (ThreadId)ReadObjectId(packet, ref offset);
                        Location location = ReadLocation(packet, ref offset);
                        TypeTag typeTag = (TypeTag)ReadByte(packet, ref offset);
                        ReferenceTypeId typeId = ReadReferenceTypeId(packet, ref offset);
                        FieldId fieldId = ReadFieldId(packet, ref offset);
                        TaggedObjectId objectId = ReadTaggedObjectId(packet, ref offset);
                        Value newValue = ReadValue(packet, ref offset);
                        _callback.FieldModification(suspendPolicy, requestId, threadId, location, typeTag, typeId, fieldId, objectId, newValue);
                    }
                    continue;

                case EventKind.ExceptionCatch:
                    throw new NotImplementedException();

                case EventKind.MethodEntry:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        ThreadId threadId = (ThreadId)ReadObjectId(packet, ref offset);
                        Location location = ReadLocation(packet, ref offset);
                        _callback.MethodEntry(suspendPolicy, requestId, threadId, location);
                    }
                    continue;

                case EventKind.MethodExit:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        ThreadId threadId = (ThreadId)ReadObjectId(packet, ref offset);
                        Location location = ReadLocation(packet, ref offset);
                        _callback.MethodExit(suspendPolicy, requestId, threadId, location, default(Value));
                    }
                    continue;

                case EventKind.MethodExitWithReturnValue:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        ThreadId threadId = (ThreadId)ReadObjectId(packet, ref offset);
                        Location location = ReadLocation(packet, ref offset);
                        Value returnValue = ReadValue(packet, ref offset);
                        _callback.MethodExit(suspendPolicy, requestId, threadId, location, returnValue);
                    }
                    continue;

                case EventKind.VirtualMachineStart:
                //case EventKind.VirtualMachineInit:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        ThreadId threadId = (ThreadId)ReadObjectId(packet, ref offset);
                        _callback.VirtualMachineStart(suspendPolicy, requestId, threadId);
                    }
                    continue;

                case EventKind.VirtualMachineDeath:
                    {
                        RequestId requestId = new RequestId(ReadInt32(packet, ref offset));
                        _callback.VirtualMachineDeath(suspendPolicy, requestId);
                    }
                    continue;

                case EventKind.VirtualMachineDisconnected:
                    throw new NotImplementedException();

                case EventKind.Invalid:
                default:
                    throw new NotImplementedException();
                }
            }
        }

        private void RequestIdSizes()
        {
            // the ID sizes will be needed by many calls
            int fieldIdSize;
            int methodIdSize;
            int objectIdSize;
            int referenceTypeIdSize;
            int frameIdSize;
            Error result = GetIdSizes(out fieldIdSize, out methodIdSize, out objectIdSize, out referenceTypeIdSize, out frameIdSize);
            if (result != Error.None)
                return;

            _fieldIdSize = fieldIdSize;
            _methodIdSize = methodIdSize;
            _objectIdSize = objectIdSize;
            _referenceTypeIdSize = referenceTypeIdSize;
            _frameIdSize = frameIdSize;
        }

        public Error Attach()
        {
            byte[] packet = Encoding.ASCII.GetBytes("JDWP-Handshake");
            if (_socket.Send(packet) != packet.Length)
                return Error.TransportInit;

            byte[] response = new byte[14];
            if (_socket.Receive(response) != response.Length)
                return Error.TransportInit;

            // start processing messages
            _handshakeComplete = true;
            ReceiveAsync();
            return Error.None;
        }

        public Error GetVersion(out string description, out int majorVersion, out int minorVersion, out string vmVersion, out string vmName)
        {
            byte[] packet = new byte[HeaderSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, VirtualMachineCommand.Version);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                description = null;
                majorVersion = 0;
                minorVersion = 0;
                vmVersion = null;
                vmName = null;
                return errorCode;
            }

            int offset = HeaderSize;
            description = ReadString(response, ref offset);
            majorVersion = ReadInt32(response, ref offset);
            minorVersion = ReadInt32(response, ref offset);
            vmVersion = ReadString(response, ref offset);
            vmName = ReadString(response, ref offset);
            return Error.None;
        }

        public Error GetClassesBySignature(out ReferenceTypeData[] classes, string signature)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(signature);
            byte[] packet = new byte[HeaderSize + sizeof(int) + encoded.Length];

            int id = GetMessageId();
            SerializeHeader(packet, id, VirtualMachineCommand.ClassesBySignature);
            WriteInt32(packet, HeaderSize, encoded.Length);
            Buffer.BlockCopy(encoded, 0, packet, HeaderSize + sizeof(int), encoded.Length);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                classes = null;
                return errorCode;
            }

            int offset = HeaderSize;
            int count = ReadInt32(response, ref offset);
            classes = new ReferenceTypeData[count];
            for (int i = 0; i < count; i++)
            {
                TypeTag typeTag = (TypeTag)ReadByte(response, ref offset);
                ReferenceTypeId typeId = ReadReferenceTypeId(response, ref offset);
                ClassStatus status = (ClassStatus)ReadInt32(response, ref offset);

                string actualSignature;
                string genericSignature;
                errorCode = GetSignature(out actualSignature, out genericSignature, typeId);
                if (errorCode != Error.None)
                {
                    classes = null;
                    return errorCode;
                }

                classes[i] = new ReferenceTypeData(new TaggedReferenceTypeId(typeTag, typeId), actualSignature, genericSignature, status);
            }

            return errorCode;
        }

        public Error GetAllClasses(out ReferenceTypeData[] classes)
        {
            byte[] packet = new byte[HeaderSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, VirtualMachineCommand.AllClassesWithGeneric);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                classes = null;
                return errorCode;
            }

            int offset = HeaderSize;
            int classCount = ReadInt32(response, ref offset);
            classes = new ReferenceTypeData[classCount];
            for (int i = 0; i < classCount; i++)
            {
                TypeTag typeTag = (TypeTag)ReadByte(response, ref offset);
                ReferenceTypeId typeId = ReadReferenceTypeId(response, ref offset);
                string signature = ReadString(response, ref offset);
                string genericSignature = ReadString(response, ref offset);
                ClassStatus status = (ClassStatus)ReadInt32(response, ref offset);
                classes[i] = new ReferenceTypeData(new TaggedReferenceTypeId(typeTag, typeId), signature, genericSignature, status);
            }

            return Error.None;
        }

        public Error GetAllThreads(out ThreadId[] threads)
        {
            byte[] packet = new byte[HeaderSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, VirtualMachineCommand.AllThreads);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                threads = null;
                return errorCode;
            }

            int offset = HeaderSize;
            int threadCount = ReadInt32(response, ref offset);
            threads = new ThreadId[threadCount];
            for (int i = 0; i < threadCount; i++)
                threads[i] = (ThreadId)ReadObjectId(response, ref offset);

            return Error.None;
        }

        public Error GetTopLevelThreadGroups(out ThreadGroupId[] groups)
        {
            throw new NotImplementedException();
        }

        public Error Dispose()
        {
            byte[] packet = new byte[HeaderSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, VirtualMachineCommand.Dispose);

            byte[] response = SendPacket(id, packet);
            return ReadErrorCode(response);
        }

        public Error GetIdSizes(out int fieldIdSize, out int methodIdSize, out int objectIdSize, out int referenceTypeIdSize, out int frameIdSize)
        {
            byte[] packet = new byte[HeaderSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, VirtualMachineCommand.IDSizes);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                fieldIdSize = 0;
                methodIdSize = 0;
                objectIdSize = 0;
                referenceTypeIdSize = 0;
                frameIdSize = 0;
                return errorCode;
            }

            int offset = HeaderSize;
            fieldIdSize = ReadInt32(response, ref offset);
            methodIdSize = ReadInt32(response, ref offset);
            objectIdSize = ReadInt32(response, ref offset);
            referenceTypeIdSize = ReadInt32(response, ref offset);
            frameIdSize = ReadInt32(response, ref offset);
            return Error.None;
        }

        public Error Suspend()
        {
            byte[] packet = new byte[HeaderSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, VirtualMachineCommand.Suspend);

            byte[] response = SendPacket(id, packet);
            return ReadErrorCode(response);
        }

        public Error Resume()
        {
            byte[] packet = new byte[HeaderSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, VirtualMachineCommand.Resume);

            byte[] response = SendPacket(id, packet);
            return ReadErrorCode(response);
        }

        public Error Exit(int exitCode)
        {
            byte[] packet = new byte[HeaderSize + sizeof(int)];
            int id = GetMessageId();
            SerializeHeader(packet, id, VirtualMachineCommand.Exit);
            WriteInt32(packet, HeaderSize, exitCode);

            byte[] response = SendPacket(id, packet);
            return ReadErrorCode(response);
        }

        public Error CreateString(out StringId stringObject, string value)
        {
            throw new NotImplementedException();
        }

        public Error GetCapabilities(out Capabilities capabilities)
        {
            byte[] packet = new byte[HeaderSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, VirtualMachineCommand.CapabilitiesNew);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                capabilities = default(Capabilities);
                return errorCode;
            }

            Capabilities[] returnedFields =
            {
                Capabilities.CanGenerateFieldModificationEvents,
                Capabilities.CanGenerateFieldAccessEvents,
                Capabilities.CanGetBytecodes,
                Capabilities.CanGetSyntheticAttribute,
                Capabilities.CanGetOwnedMonitorInfo,
                Capabilities.CanGetCurrentContendedMonitor,
                Capabilities.CanGetMonitorInfo,
                Capabilities.CanRedefineClasses,
                Capabilities.None, // add method?
                Capabilities.CanRedefineAnyClass,
                Capabilities.CanPopFrame,
                Capabilities.None, // use instance filters?
                Capabilities.CanGetSourceDebugExtension,
                Capabilities.None, // request VM death event?
                Capabilities.None, // set default stratum?
                Capabilities.None, // get instance info?
                Capabilities.CanGenerateMonitorEvents,
                Capabilities.CanGetOwnedMonitorStackDepthInfo,
                Capabilities.None, // use source name filters?
                Capabilities.CanGetConstantPool,
                Capabilities.CanForceEarlyReturn,
                Capabilities.None, // reserved22
                Capabilities.None, // reserved23
                Capabilities.None, // reserved24
                Capabilities.None, // reserved25
                Capabilities.None, // reserved26
                Capabilities.None, // reserved27
                Capabilities.None, // reserved28
                Capabilities.None, // reserved29
                Capabilities.None, // reserved30
                Capabilities.None, // reserved31
                Capabilities.None, // reserved32
            };

            capabilities = Capabilities.None;
            int offset = HeaderSize;
            foreach (Capabilities capability in returnedFields)
            {
                if (ReadBoolean(response, ref offset))
                    capabilities |= capability;
            }

            return Error.None;
        }

        public Error GetClassPaths(out string baseDirectory, out string[] classPaths, out string[] bootClassPaths)
        {
            throw new NotImplementedException();
        }

        public Error DisposeObjects(ObjectReferenceCountData[] requests)
        {
            throw new NotImplementedException();
        }

        public Error HoldEvents()
        {
            byte[] packet = new byte[HeaderSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, VirtualMachineCommand.HoldEvents);

            byte[] response = SendPacket(id, packet);
            return ReadErrorCode(response);
        }

        public Error ReleaseEvents()
        {
            byte[] packet = new byte[HeaderSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, VirtualMachineCommand.ReleaseEvents);

            byte[] response = SendPacket(id, packet);
            return ReadErrorCode(response);
        }

        public Error RedefineClasses(ClassDefinitionData[] definitions)
        {
            throw new NotImplementedException();
        }

        public Error SetDefaultStratum(string stratumId)
        {
            throw new NotImplementedException();
        }

        public Error GetSignature(out string signature, out string genericSignature, ReferenceTypeId referenceType)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ReferenceTypeCommand.SignatureWithGeneric);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                signature = null;
                genericSignature = null;
                return errorCode;
            }

            int offset = HeaderSize;
            signature = ReadString(response, ref offset);
            genericSignature = ReadString(response, ref offset);
            return Error.None;
        }

        public Error GetClassLoader(out ClassLoaderId classLoader, ReferenceTypeId referenceType)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ReferenceTypeCommand.ClassLoader);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                classLoader = default(ClassLoaderId);
                return errorCode;
            }

            int offset = HeaderSize;
            classLoader = (ClassLoaderId)ReadObjectId(response, ref offset);
            return Error.None;
        }

        public Error GetModifiers(out AccessModifiers modifiers, ReferenceTypeId referenceType)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ReferenceTypeCommand.Modifiers);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                modifiers = default(AccessModifiers);
                return errorCode;
            }

            int offset = HeaderSize;
            modifiers = (AccessModifiers)ReadInt32(response, ref offset);
            return Error.None;
        }

        public Error GetFields(out DeclaredFieldData[] fields, ReferenceTypeId referenceType)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ReferenceTypeCommand.FieldsWithGeneric);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                fields = null;
                return errorCode;
            }

            int offset = HeaderSize;
            int fieldCount = ReadInt32(response, ref offset);
            fields = new DeclaredFieldData[fieldCount];
            for (int i = 0; i < fieldCount; i++)
            {
                FieldId fieldId = ReadFieldId(response, ref offset);
                string name = ReadString(response, ref offset);
                string signature = ReadString(response, ref offset);
                string genericSignature = ReadString(response, ref offset);
                AccessModifiers modifiers = (AccessModifiers)ReadInt32(response, ref offset);
                fields[i] = new DeclaredFieldData(fieldId, name, signature, genericSignature, modifiers);
            }

            return Error.None;
        }

        public Error GetMethods(out DeclaredMethodData[] methods, ReferenceTypeId referenceType)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ReferenceTypeCommand.MethodsWithGeneric);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                methods = null;
                return errorCode;
            }

            int offset = HeaderSize;
            int methodCount = ReadInt32(response, ref offset);
            methods = new DeclaredMethodData[methodCount];
            for (int i = 0; i < methodCount; i++)
            {
                MethodId methodId = ReadMethodId(response, ref offset);
                string name = ReadString(response, ref offset);
                string signature = ReadString(response, ref offset);
                string genericSignature = ReadString(response, ref offset);
                AccessModifiers modifiers = (AccessModifiers)ReadInt32(response, ref offset);
                methods[i] = new DeclaredMethodData(methodId, name, signature, genericSignature, modifiers);
            }

            return Error.None;
        }

        public Error GetReferenceTypeValues(out Value[] values, ReferenceTypeId referenceType, FieldId[] fields)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize + sizeof(int) + (fields.Length * FieldIdSize)];
            int id = GetMessageId();
            SerializeHeader(packet, id, ReferenceTypeCommand.GetValues);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);
            WriteInt32(packet, HeaderSize + ReferenceTypeIdSize, fields.Length);
            for (int i = 0; i < fields.Length; i++)
            {
                WriteFieldId(packet, HeaderSize + ReferenceTypeIdSize + sizeof(int) + (i * FieldIdSize), fields[i]);
            }

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                values = null;
                return errorCode;
            }

            int offset = HeaderSize;
            int valueCount = ReadInt32(response, ref offset);
            values = new Value[valueCount];
            for (int i = 0; i < valueCount; i++)
            {
                values[i] = ReadValue(response, ref offset);
            }

            return Error.None;
        }

        public Error GetSourceFile(out string sourceFile, ReferenceTypeId referenceType)
        {
            byte[] packet = new byte[HeaderSize + _referenceTypeIdSize.Value];
            int id = GetMessageId();
            SerializeHeader(packet, id, ReferenceTypeCommand.SourceFile);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                sourceFile = null;
                return errorCode;
            }

            int offset = HeaderSize;
            sourceFile = ReadString(response, ref offset);
            return Error.None;
        }

        public Error GetNestedTypes(out TaggedReferenceTypeId[] classes, ReferenceTypeId referenceType)
        {
            throw new NotImplementedException();
        }

        public Error GetReferenceTypeStatus(out ClassStatus status, ReferenceTypeId referenceType)
        {
            byte[] packet = new byte[HeaderSize + _referenceTypeIdSize.Value];
            int id = GetMessageId();
            SerializeHeader(packet, id, ReferenceTypeCommand.Status);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                status = default(ClassStatus);
                return errorCode;
            }

            int offset = HeaderSize;
            status = (ClassStatus)ReadInt32(response, ref offset);
            return Error.None;
        }

        public Error GetInterfaces(out InterfaceId[] interfaces, ReferenceTypeId referenceType)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ReferenceTypeCommand.Interfaces);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                interfaces = null;
                return errorCode;
            }

            int offset = HeaderSize;
            int interfaceCount = ReadInt32(response, ref offset);
            interfaces = new InterfaceId[interfaceCount];
            for (int i = 0; i < interfaceCount; i++)
            {
                interfaces[i] = (InterfaceId)ReadReferenceTypeId(response, ref offset);
            }

            return Error.None;
        }

        public Error GetClassObject(out ClassObjectId classObject, ReferenceTypeId referenceType)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ReferenceTypeCommand.ClassObject);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                classObject = default(ClassObjectId);
                return errorCode;
            }

            int offset = HeaderSize;
            classObject = (ClassObjectId)ReadObjectId(response, ref offset);
            return Error.None;
        }

        public Error GetSourceDebugExtension(out string extension, ReferenceTypeId referenceType)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ReferenceTypeCommand.SourceDebugExtension);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                extension = null;
                return errorCode;
            }

            int offset = HeaderSize;
            extension = ReadString(response, ref offset);
            return Error.None;
        }

        public Error GetInstances(out TaggedObjectId[] instances, ReferenceTypeId referenceType, int maxInstances)
        {
            throw new NotImplementedException();
        }

        public Error GetClassFileVersion(out int majorVersion, out int minorVersion, ReferenceTypeId referenceType)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ReferenceTypeCommand.ClassFileVersion);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                majorVersion = 0;
                minorVersion = 0;
                return errorCode;
            }

            int offset = HeaderSize;
            majorVersion = ReadInt32(response, ref offset);
            minorVersion = ReadInt32(response, ref offset);
            return Error.None;
        }

        public Error GetConstantPool(out int constantPoolCount, out byte[] data, ReferenceTypeId referenceType)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ReferenceTypeCommand.ConstantPool);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                constantPoolCount = 0;
                data = null;
                return errorCode;
            }

            int offset = HeaderSize;
            constantPoolCount = ReadInt32(response, ref offset);
            data = new byte[ReadInt32(response, ref offset)];
            Buffer.BlockCopy(response, offset, data, 0, data.Length);
            return Error.None;
        }

        public Error GetSuperclass(out ClassId superclass, ClassId @class)
        {
            byte[] packet = new byte[HeaderSize + ObjectIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ClassTypeCommand.Superclass);
            WriteReferenceTypeId(packet, HeaderSize, @class);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                superclass = default(ClassId);
                return errorCode;
            }

            int offset = HeaderSize;
            superclass = (ClassId)ReadReferenceTypeId(response, ref offset);
            return Error.None;
        }

        public Error SetClassValues(ClassId @class, FieldId[] fields, Value[] values)
        {
            throw new NotImplementedException();
        }

        public Error InvokeClassMethod(out Value returnValue, out TaggedObjectId thrownException, ClassId @class, ThreadId thread, MethodId method, InvokeOptions options, Value[] arguments)
        {
            throw new NotImplementedException();
        }

        public Error CreateClassInstance(out TaggedObjectId newObject, out TaggedObjectId thrownException, ClassId @class, ThreadId thread, MethodId method, InvokeOptions options, Value[] arguments)
        {
            throw new NotImplementedException();
        }

        public Error CreateArrayInstance(out TaggedObjectId newArray, ArrayTypeId arrayType, int length)
        {
            throw new NotImplementedException();
        }

        public Error GetMethodExceptionTable(out ExceptionTableEntry[] entries, ReferenceTypeId referenceType, MethodId method)
        {
            entries = null;
            return Error.NotImplemented;
        }

        public Error GetMethodLineTable(out long start, out long end, out LineNumberData[] lines, ReferenceTypeId referenceType, MethodId method)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize + MethodIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, MethodCommand.LineTable);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);
            WriteMethodId(packet, HeaderSize + ReferenceTypeIdSize, method);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                start = 0;
                end = 0;
                lines = null;
                return errorCode;
            }

            int offset = HeaderSize;
            start = ReadInt64(response, ref offset);
            end = ReadInt64(response, ref offset);
            int lineCount = ReadInt32(response, ref offset);
            lines = new LineNumberData[lineCount];
            for (int i = 0; i < lineCount; i++)
            {
                long lineCodeIndex = ReadInt64(response, ref offset);
                int lineNumber = ReadInt32(response, ref offset);
                lines[i] = new LineNumberData(lineCodeIndex, lineNumber);
            }

            return Error.None;
        }

        public Error GetMethodVariableTable(out VariableData[] slots, ReferenceTypeId referenceType, MethodId method)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize + MethodIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, MethodCommand.VariableTableWithGeneric);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);
            WriteMethodId(packet, HeaderSize + ReferenceTypeIdSize, method);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                slots = null;
                return errorCode;
            }

            int offset = HeaderSize;
            int argumentCount = ReadInt32(response, ref offset);
            int slotCount = ReadInt32(response, ref offset);
            slots = new VariableData[slotCount];
            for (int i = 0; i < slotCount; i++)
            {
                ulong codeIndex = ReadUInt64(response, ref offset);
                string name = ReadString(response, ref offset);
                string signature = ReadString(response, ref offset);
                string genericSignature = ReadString(response, ref offset);
                uint length = ReadUInt32(response, ref offset);
                int slot = ReadInt32(response, ref offset);
                slots[i] = new VariableData(slot, codeIndex, length, name, signature, genericSignature);
            }

            return Error.None;
        }

        public Error GetMethodBytecodes(out byte[] bytecode, ReferenceTypeId referenceType, MethodId method)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize + MethodIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, MethodCommand.Bytecodes);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);
            WriteMethodId(packet, HeaderSize + ReferenceTypeIdSize, method);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                bytecode = null;
                return errorCode;
            }

            int offset = HeaderSize;
            int bytes = ReadInt32(response, ref offset);
            bytecode = new byte[bytes];
            Buffer.BlockCopy(response, offset, bytecode, 0, bytes);
            return Error.None;
        }

        public Error GetMethodIsObsolete(out bool result, ReferenceTypeId referenceType, MethodId method)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize + MethodIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, MethodCommand.Bytecodes);
            WriteReferenceTypeId(packet, HeaderSize, referenceType);
            WriteMethodId(packet, HeaderSize + ReferenceTypeIdSize, method);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                result = false;
                return errorCode;
            }

            int offset = HeaderSize;
            result = ReadByte(response, ref offset) != 0;
            return Error.None;
        }

        public Error GetObjectReferenceType(out TypeTag typeTag, out ReferenceTypeId typeId, ObjectId @object)
        {
            byte[] packet = new byte[HeaderSize + ObjectIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ObjectReferenceCommand.ReferenceType);
            WriteObjectId(packet, HeaderSize, @object);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                typeTag = default(TypeTag);
                typeId = default(ReferenceTypeId);
                return errorCode;
            }

            int offset = HeaderSize;
            typeTag = (TypeTag)ReadByte(response, ref offset);
            typeId = ReadReferenceTypeId(response, ref offset);
            return Error.None;
        }

        public Error GetObjectValues(out Value[] values, ObjectId @object, FieldId[] fields)
        {
            byte[] packet = new byte[HeaderSize + ObjectIdSize + sizeof(int) + (fields.Length * FieldIdSize)];
            int id = GetMessageId();
            SerializeHeader(packet, id, ObjectReferenceCommand.GetValues);
            WriteObjectId(packet, HeaderSize, @object);
            WriteInt32(packet, HeaderSize + ObjectIdSize, fields.Length);
            for (int i = 0; i < fields.Length; i++)
            {
                WriteFieldId(packet, HeaderSize + ObjectIdSize + sizeof(int) + (i * FieldIdSize), fields[i]);
            }

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                values = null;
                return errorCode;
            }

            int offset = HeaderSize;
            int valueCount = ReadInt32(response, ref offset);
            values = new Value[valueCount];
            for (int i = 0; i < valueCount; i++)
            {
                values[i] = ReadValue(response, ref offset);
            }

            return Error.None;
        }

        public Error SetObjectValues(ObjectId @object, FieldId[] fields, Value[] values)
        {
            throw new NotImplementedException();
        }

        public Error GetObjectMonitorInfo(out ThreadId owner, out int entryCount, out ThreadId[] waiters, ObjectId @object)
        {
            throw new NotImplementedException();
        }

        public Error InvokeObjectMethod(out Value returnValue, out TaggedObjectId thrownException, ObjectId @object, ThreadId thread, ClassId @class, MethodId method, InvokeOptions options, Value[] arguments)
        {
            if (thread == default(ThreadId))
                throw new ArgumentException();

            byte[] packet = new byte[HeaderSize + ObjectIdSize + ThreadIdSize + ClassIdSize + MethodIdSize + sizeof(int)];
            WriteObjectId(packet, HeaderSize, @object);
            WriteObjectId(packet, HeaderSize + ObjectIdSize, thread);
            WriteReferenceTypeId(packet, HeaderSize + ObjectIdSize + ThreadIdSize, @class);
            WriteMethodId(packet, HeaderSize + ObjectIdSize + ThreadIdSize + ClassIdSize, method);
            WriteInt32(packet, HeaderSize + ObjectIdSize + ThreadIdSize + ClassIdSize + MethodIdSize, arguments.Length);

            List<byte> packetData = new List<byte>(packet);
            foreach (Value argument in arguments)
            {
                switch (argument.Tag)
                {
                case Tag.Byte:
                    throw new NotImplementedException();

                case Tag.Char:
                    throw new NotImplementedException();

                case Tag.Float:
                    throw new NotImplementedException();

                case Tag.Double:
                    throw new NotImplementedException();

                case Tag.Int:
                    throw new NotImplementedException();

                case Tag.Long:
                    throw new NotImplementedException();

                case Tag.Short:
                    throw new NotImplementedException();

                case Tag.Boolean:
                    throw new NotImplementedException();

                case Tag.Array:
                case Tag.Object:
                case Tag.String:
                case Tag.Thread:
                case Tag.ThreadGroup:
                case Tag.ClassLoader:
                case Tag.ClassObject:
                    throw new NotImplementedException();

                case Tag.Void:
                    throw new NotImplementedException();

                case Tag.Invalid:
                default:
                    throw new InvalidOperationException();
                }
            }

            byte[] optionsData = new byte[sizeof(int)];
            WriteInt32(optionsData, 0, (int)options);
            packetData.AddRange(optionsData);

            packet = packetData.ToArray();
            int id = GetMessageId();
            SerializeHeader(packet, id, ObjectReferenceCommand.InvokeMethod);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                returnValue = default(Value);
                thrownException = default(TaggedObjectId);
                return errorCode;
            }

            int offset = HeaderSize;
            returnValue = ReadValue(response, ref offset);
            thrownException = ReadTaggedObjectId(response, ref offset);
            return Error.None;
        }

        public Error DisableObjectCollection(ObjectId @object)
        {
            byte[] packet = new byte[HeaderSize + ObjectIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ObjectReferenceCommand.DisableCollection);
            WriteObjectId(packet, HeaderSize, @object);

            byte[] response = SendPacket(id, packet);
            return ReadErrorCode(response);
        }

        public Error EnableObjectCollection(ObjectId @object)
        {
            byte[] packet = new byte[HeaderSize + ObjectIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ObjectReferenceCommand.EnableCollection);
            WriteObjectId(packet, HeaderSize, @object);

            byte[] response = SendPacket(id, packet);
            return ReadErrorCode(response);
        }

        public Error GetIsObjectCollected(out bool result, ObjectId @object)
        {
            byte[] packet = new byte[HeaderSize + ObjectIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ObjectReferenceCommand.IsCollected);
            WriteObjectId(packet, HeaderSize, @object);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                result = false;
                return errorCode;
            }

            int offset = HeaderSize;
            result = ReadByte(response, ref offset) != 0;
            return Error.None;
        }

        public Error GetStringValue(out string stringValue, ObjectId stringObject)
        {
            byte[] packet = new byte[HeaderSize + ObjectIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, StringReferenceCommand.Value);
            WriteObjectId(packet, HeaderSize, stringObject);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                stringValue = null;
                return errorCode;
            }

            int offset = HeaderSize;
            stringValue = ReadString(response, ref offset);
            return Error.None;
        }

        public Error GetThreadName(out string name, ThreadId thread)
        {
            byte[] packet = new byte[HeaderSize + ReferenceTypeIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ThreadReferenceCommand.Name);
            WriteObjectId(packet, HeaderSize, thread);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                name = null;
                return errorCode;
            }

            int offset = HeaderSize;
            name = ReadString(response, ref offset);
            return Error.None;
        }

        public Error SuspendThread(ThreadId thread)
        {
            byte[] packet = new byte[HeaderSize + ThreadIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ThreadReferenceCommand.Suspend);
            WriteObjectId(packet, HeaderSize, thread);

            byte[] response = SendPacket(id, packet);
            return ReadErrorCode(response);
        }

        public Error ResumeThread(ThreadId thread)
        {
            byte[] packet = new byte[HeaderSize + ThreadIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ThreadReferenceCommand.Resume);
            WriteObjectId(packet, HeaderSize, thread);

            byte[] response = SendPacket(id, packet);
            return ReadErrorCode(response);
        }

        public Error GetThreadStatus(out ThreadStatus threadStatus, out SuspendStatus suspendStatus, ThreadId thread)
        {
            byte[] packet = new byte[HeaderSize + ThreadIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ThreadReferenceCommand.Status);
            WriteObjectId(packet, HeaderSize, thread);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                threadStatus = default(ThreadStatus);
                suspendStatus = default(SuspendStatus);
                return errorCode;
            }

            int offset = HeaderSize;
            threadStatus = (ThreadStatus)ReadInt32(response, ref offset);
            suspendStatus = (SuspendStatus)ReadInt32(response, ref offset);
            return Error.None;
        }

        public Error GetThreadGroup(out ThreadGroupId threadGroup, ThreadId thread)
        {
            throw new NotImplementedException();
        }

        public Error GetThreadFrames(out FrameLocationData[] frames, ThreadId thread, int startFrame, int length)
        {
            Error errorCode = GetRawThreadFrames(out frames, thread, startFrame, length);
            if (errorCode != Error.None)
                return errorCode;

            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = new FrameLocationData(new FrameId(startFrame + i), frames[i].Location);
            }

            return Error.None;
        }

        public Error GetRawThreadFrames(out FrameLocationData[] frames, ThreadId thread, int startFrame, int length)
        {
            byte[] packet = new byte[HeaderSize + ThreadIdSize + sizeof(int) + sizeof(int)];
            int id = GetMessageId();
            SerializeHeader(packet, id, ThreadReferenceCommand.Frames);
            WriteObjectId(packet, HeaderSize, thread);
            WriteInt32(packet, HeaderSize + ThreadIdSize, startFrame);
            WriteInt32(packet, HeaderSize + ThreadIdSize + sizeof(int), length);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                frames = null;
                return errorCode;
            }

            int offset = HeaderSize;
            int frameCount = ReadInt32(response, ref offset);
            frames = new FrameLocationData[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                FrameId frameId = ReadFrameId(response, ref offset);
                Location location = ReadLocation(response, ref offset);
                frames[i] = new FrameLocationData(frameId, location);
            }

            return Error.None;
        }

        public Error GetThreadFrameCount(out int frameCount, ThreadId thread)
        {
            byte[] packet = new byte[HeaderSize + ThreadIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ThreadReferenceCommand.FrameCount);
            WriteObjectId(packet, HeaderSize, thread);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                frameCount = 0;
                return errorCode;
            }

            int offset = HeaderSize;
            frameCount = ReadInt32(response, ref offset);
            return Error.None;
        }

        public Error GetThreadOwnedMonitors(out TaggedObjectId[] monitors, ThreadId thread)
        {
            throw new NotImplementedException();
        }

        public Error GetThreadCurrentContendedMonitor(out TaggedObjectId monitor, ThreadId thread)
        {
            throw new NotImplementedException();
        }

        public Error StopThread(ThreadId thread, ObjectId throwable)
        {
            throw new NotImplementedException();
        }

        public Error InterruptThread(ThreadId thread)
        {
            throw new NotImplementedException();
        }

        public Error GetThreadSuspendCount(out int suspendCount, ThreadId thread)
        {
            byte[] packet = new byte[HeaderSize + ThreadIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ThreadReferenceCommand.SuspendCount);
            WriteObjectId(packet, HeaderSize, thread);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                suspendCount = 0;
                return errorCode;
            }

            int offset = HeaderSize;
            suspendCount = ReadInt32(response, ref offset);
            return Error.None;
        }

        public Error GetThreadGroupName(out string groupName, ThreadGroupId group)
        {
            throw new NotImplementedException();
        }

        public Error GetThreadGroupParent(out ThreadGroupId parentGroup, ThreadGroupId group)
        {
            throw new NotImplementedException();
        }

        public Error GetThreadGroupChildren(out ThreadId[] childThreads, out ThreadGroupId[] childGroups, ThreadGroupId group)
        {
            throw new NotImplementedException();
        }

        public Error GetArrayLength(out int arrayLength, ArrayId arrayObject)
        {
            byte[] packet = new byte[HeaderSize + ObjectIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ArrayReferenceCommand.Length);
            WriteObjectId(packet, HeaderSize, arrayObject);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                arrayLength = 0;
                return errorCode;
            }

            int offset = HeaderSize;
            arrayLength = ReadInt32(response, ref offset);
            return Error.None;
        }

        public Error GetArrayValues(out Value[] values, ArrayId arrayObject, int firstIndex, int length)
        {
            if (length == 0)
            {
                // The JDWP implementation doesn't handle zero-length requests at the end of an array
                int arrayLength;
                Error lengthErrorCode = GetArrayLength(out arrayLength, arrayObject);
                if (lengthErrorCode == Error.None && arrayLength == firstIndex)
                {
                    values = new Value[0];
                    return Error.None;
                }
            }

            byte[] packet = new byte[HeaderSize + ObjectIdSize + (2 * sizeof(int))];
            int id = GetMessageId();
            SerializeHeader(packet, id, ArrayReferenceCommand.GetValues);
            WriteObjectId(packet, HeaderSize, arrayObject);
            WriteInt32(packet, HeaderSize + ObjectIdSize, firstIndex);
            WriteInt32(packet, HeaderSize + ObjectIdSize + sizeof(int), length);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                values = null;
                return errorCode;
            }

            int offset = HeaderSize;

            // "arrayregion"
            Tag tag = (Tag)ReadByte(response, ref offset);
            int count = ReadInt32(response, ref offset);
            values = new Value[count];
            for (int i = 0; i < count; i++)
            {
                switch (tag)
                {
                case Tag.Byte:
                case Tag.Char:
                case Tag.Float:
                case Tag.Double:
                case Tag.Int:
                case Tag.Long:
                case Tag.Short:
                case Tag.Boolean:
                    values[i] = ReadUntaggedValue(response, ref offset, tag);
                    break;

                case Tag.Array:
                case Tag.Object:
                case Tag.String:
                case Tag.Thread:
                case Tag.ThreadGroup:
                case Tag.ClassLoader:
                case Tag.ClassObject:
                    values[i] = ReadValue(response, ref offset);
                    break;

                case Tag.Invalid:
                case Tag.Void:
                default:
                    throw new InvalidOperationException();
                }
            }

            return Error.None;
        }

        public Error SetArrayValues(ArrayId arrayObject, int firstIndex, Value[] values)
        {
            throw new NotImplementedException();
        }

        public Error GetClassLoaderVisibleClasses(out TaggedReferenceTypeId[] classes, ClassLoaderId classLoaderObject)
        {
            throw new NotImplementedException();
        }

        private readonly ConcurrentDictionary<RequestId, List<RequestId>> _linkedRequests =
            new ConcurrentDictionary<RequestId, List<RequestId>>();
        private readonly ConcurrentDictionary<RequestId, RequestId> _requestRemap =
            new ConcurrentDictionary<RequestId, RequestId>();

        public Error SetEvent(out RequestId requestId, EventKind eventKind, SuspendPolicy suspendPolicy, EventRequestModifier[] modifiers)
        {
            if (eventKind == EventKind.SingleStep && modifiers.Length == 1 && modifiers[0].Thread == default(ThreadId))
            {
                ThreadId[] threads;
                Error threadsErrorCode = GetAllThreads(out threads);
                if (threadsErrorCode != Error.None)
                {
                    requestId = default(RequestId);
                    return threadsErrorCode;
                }

                requestId = default(RequestId);

                threadsErrorCode = Suspend();
                if (threadsErrorCode != Error.None)
                    return threadsErrorCode;

                List<RequestId> requests = new List<RequestId>();
                foreach (var thread in threads)
                {
                    EventRequestModifier modifier = modifiers[0];
                    modifier.Thread = thread;
                    threadsErrorCode = SetEvent(out requestId, eventKind, suspendPolicy, new[] { modifier });
                    if (threadsErrorCode != Error.None)
                        return threadsErrorCode;

                    requests.Add(requestId);
                }

                _linkedRequests[requestId] = requests;
                foreach (var request in requests)
                    _requestRemap[request] = requestId;

                threadsErrorCode = Resume();
                if (threadsErrorCode != Error.None)
                    return threadsErrorCode;

                return Error.None;
            }

            byte[] packet = new byte[HeaderSize + 6];
            packet[HeaderSize] = (byte)eventKind;
            packet[HeaderSize + 1] = (byte)suspendPolicy;
            WriteInt32(packet, HeaderSize + 2, modifiers.Length);

            List<byte> packetData = new List<byte>(packet);
            foreach (EventRequestModifier modifier in modifiers)
            {
                packetData.Add((byte)modifier.Kind);

                switch (modifier.Kind)
                {
                case ModifierKind.Count:
                    {
                        byte[] additionalData = new byte[sizeof(int)];
                        WriteInt32(additionalData, 0, modifier.Count);
                        packetData.AddRange(additionalData);
                    }
                    continue;

                case ModifierKind.Conditional:
                    {
                        byte[] additionalData = new byte[sizeof(int)];
                        WriteInt32(additionalData, 0, modifier.ExpressionId);
                        packetData.AddRange(additionalData);
                    }
                    continue;

                case ModifierKind.ThreadFilter:
                    {
                        byte[] additionalData = new byte[ThreadIdSize];
                        WriteObjectId(additionalData, 0, modifier.Thread);
                        packetData.AddRange(additionalData);
                    }
                    continue;

                case ModifierKind.ClassTypeFilter:
                    {
                        byte[] additionalData = new byte[ReferenceTypeIdSize];
                        WriteReferenceTypeId(additionalData, 0, modifier.Class);
                        packetData.AddRange(additionalData);
                    }
                    continue;

                case ModifierKind.ClassMatchFilter:
                case ModifierKind.ClassExcludeFilter:
                    {
                        byte[] stringData = Encoding.UTF8.GetBytes(modifier.ClassPattern);
                        byte[] sizeData = new byte[sizeof(int)];
                        WriteInt32(sizeData, 0, stringData.Length);
                        packetData.AddRange(sizeData);
                        packetData.AddRange(stringData);
                    }
                    continue;

                case ModifierKind.LocationFilter:
                    {
                        byte[] additionalData = new byte[LocationSize];
                        WriteLocation(additionalData, 0, modifier.Location);
                        packetData.AddRange(additionalData);
                    }
                    continue;

                case ModifierKind.ExceptionFilter:
                    {
                        byte[] additionalData = new byte[_referenceTypeIdSize.Value + 2];
                        WriteReferenceTypeId(additionalData, 0, modifier.ExceptionOrNull);
                        additionalData[_referenceTypeIdSize.Value] = (byte)(modifier.Caught ? 1 : 0);
                        additionalData[_referenceTypeIdSize.Value + 1] = (byte)(modifier.Uncaught ? 1 : 0);
                        packetData.AddRange(additionalData);
                    }
                    continue;

                case ModifierKind.FieldFilter:
                    {
                        byte[] additionalData = new byte[ReferenceTypeIdSize + FieldIdSize];
                        WriteReferenceTypeId(additionalData, 0, modifier.Class);
                        WriteFieldId(additionalData, ReferenceTypeIdSize, modifier.Field);
                        packetData.AddRange(additionalData);
                    }
                    continue;

                case ModifierKind.Step:
                    {
                        if (modifier.StepSize == StepSize.Statement)
                            throw new NotSupportedException();

                        byte[] additionalData = new byte[ThreadIdSize + (2 * sizeof(int))];
                        WriteObjectId(additionalData, 0, modifier.Thread);
                        WriteInt32(additionalData, ThreadIdSize, (int)modifier.StepSize);
                        WriteInt32(additionalData, ThreadIdSize + sizeof(int), (int)modifier.StepDepth);
                        packetData.AddRange(additionalData);
                    }
                    continue;

                case ModifierKind.InstanceFilter:
                    {
                        byte[] additionalData = new byte[ObjectIdSize];
                        WriteObjectId(additionalData, 0, modifier.Instance);
                        packetData.AddRange(additionalData);
                    }
                    continue;

                case ModifierKind.SourceNameMatchFilter:
                    {
                        byte[] stringData = Encoding.UTF8.GetBytes(modifier.SourceNamePattern);
                        byte[] sizeData = new byte[sizeof(int)];
                        WriteInt32(sizeData, 0, stringData.Length);
                        packetData.AddRange(sizeData);
                        packetData.AddRange(stringData);
                    }
                    continue;

                case ModifierKind.Invalid:
                default:
                    throw new InvalidOperationException();
                }
            }

            packet = packetData.ToArray();
            int id = GetMessageId();
            SerializeHeader(packet, id, EventRequestCommand.Set);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                requestId = default(RequestId);
                return errorCode;
            }

            int offset = HeaderSize;
            requestId = new RequestId(ReadInt32(response, ref offset));
            return Error.None;
        }

        public Error ClearEvent(EventKind eventKind, RequestId requestId)
        {
            List<RequestId> linkedRequests;
            if (_linkedRequests.TryRemove(requestId, out linkedRequests))
            {
                Suspend();

                foreach (RequestId request in linkedRequests)
                {
                    RequestId ignored;
                    _requestRemap.TryRemove(request, out ignored);

                    ClearEvent(eventKind, request);
                }

                return Resume();
            }

            byte[] packet = new byte[HeaderSize + sizeof(byte) + sizeof(int)];
            int id = GetMessageId();
            SerializeHeader(packet, id, EventRequestCommand.Clear);
            packet[HeaderSize] = (byte)eventKind;
            WriteInt32(packet, HeaderSize + sizeof(byte), requestId.Id);

            byte[] response = SendPacket(id, packet);
            return ReadErrorCode(response);
        }

        public Error ClearAllBreakpoints()
        {
            byte[] packet = new byte[HeaderSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, EventRequestCommand.ClearAllBreakpoints);

            byte[] response = SendPacket(id, packet);
            return ReadErrorCode(response);
        }

        public Error GetValues(out Value[] values, ThreadId thread, FrameId frame, int[] slots)
        {
            int frameIndex = (int)frame.Handle;
            FrameLocationData[] frames = new FrameLocationData[1];
            Error errorCode = GetRawThreadFrames(out frames, thread, frameIndex, 1);
            if (errorCode != Error.None)
            {
                values = null;
                return errorCode;
            }

            frame = frames[0].FrameId;

            VariableData[] variableData;
            errorCode = GetMethodVariableTable(out variableData, frames[0].Location.Class, frames[0].Location.Method);
            if (errorCode != Error.None)
            {
                values = null;
                return errorCode;
            }

            Tag[] tags = new Tag[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                tags[i] = Tag.Object;
                foreach (VariableData variable in variableData)
                {
                    if (variable.Slot != slots[i])
                        continue;

                    if (variable.CodeIndex > frames[0].Location.Index)
                        continue;

                    if (variable.CodeIndex + variable.Length <= frames[0].Location.Index)
                        continue;

                    if (string.IsNullOrEmpty(variable.Signature))
                        continue;

                    tags[i] = (Tag)variable.Signature[0];
                    break;
                }
            }

            byte[] packet = new byte[HeaderSize + ThreadIdSize + FrameIdSize + sizeof(int) + (slots.Length * (sizeof(int) + sizeof(byte)))];
            int id = GetMessageId();
            SerializeHeader(packet, id, StackFrameCommand.GetValues);
            WriteObjectId(packet, HeaderSize, thread);
            WriteFrameId(packet, HeaderSize + ThreadIdSize, frame);
            WriteInt32(packet, HeaderSize + ThreadIdSize + FrameIdSize, slots.Length);

            int baseOffset = HeaderSize + ThreadIdSize + FrameIdSize + sizeof(int);
            for (int i = 0; i < slots.Length; i++)
            {
                int slotOffset = baseOffset + i * (sizeof(int) + sizeof(byte));
                WriteInt32(packet, slotOffset, slots[i]);
                packet[slotOffset + sizeof(int)] = (byte)tags[i];
            }

            byte[] response = SendPacket(id, packet);
            errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                values = null;
                return errorCode;
            }

            int offset = HeaderSize;
            int valueCount = ReadInt32(response, ref offset);
            values = new Value[valueCount];
            for (int i = 0; i < valueCount; i++)
            {
                values[i] = ReadValue(response, ref offset);
            }

            return Error.None;
        }

        public Error SetValues(ThreadId thread, FrameId frame, int[] slots, Value[] values)
        {
            int frameIndex = (int)frame.Handle;
            FrameLocationData[] frames = new FrameLocationData[1];
            Error errorCode = GetRawThreadFrames(out frames, thread, frameIndex, 1);
            if (errorCode != Error.None)
                return errorCode;

            frame = frames[0].FrameId;

            throw new NotImplementedException();
        }

        public Error GetThisObject(out TaggedObjectId thisObject, ThreadId thread, FrameId frame)
        {
            int frameIndex = (int)frame.Handle;
            FrameLocationData[] frames = new FrameLocationData[1];
            Error errorCode = GetRawThreadFrames(out frames, thread, frameIndex, 1);
            if (errorCode != Error.None)
            {
                thisObject = default(TaggedObjectId);
                return errorCode;
            }

            frame = frames[0].FrameId;

            byte[] packet = new byte[HeaderSize + ThreadIdSize + FrameIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, StackFrameCommand.ThisObject);
            WriteObjectId(packet, HeaderSize, thread);
            WriteFrameId(packet, HeaderSize + ThreadIdSize, frame);

            byte[] response = SendPacket(id, packet);
            errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                thisObject = default(TaggedObjectId);
                return errorCode;
            }

            int offset = HeaderSize;
            thisObject = ReadTaggedObjectId(response, ref offset);
            return Error.None;
        }

        public Error PopFrames(ThreadId thread, FrameId frame)
        {
            int frameIndex = (int)frame.Handle;
            FrameLocationData[] frames = new FrameLocationData[1];
            Error errorCode = GetRawThreadFrames(out frames, thread, frameIndex, 1);
            if (errorCode != Error.None)
                return errorCode;

            frame = frames[0].FrameId;

            byte[] packet = new byte[HeaderSize + ThreadIdSize + FrameIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, StackFrameCommand.PopFrames);
            WriteObjectId(packet, HeaderSize, thread);
            WriteFrameId(packet, HeaderSize + ThreadIdSize, frame);

            byte[] response = SendPacket(id, packet);
            return ReadErrorCode(response);
        }

        public Error GetReflectedType(out TypeTag typeTag, out ReferenceTypeId typeId, ClassObjectId classObject)
        {
            byte[] packet = new byte[HeaderSize + ObjectIdSize];
            int id = GetMessageId();
            SerializeHeader(packet, id, ClassObjectReferenceCommand.ReflectedType);
            WriteObjectId(packet, HeaderSize, classObject);

            byte[] response = SendPacket(id, packet);
            Error errorCode = ReadErrorCode(response);
            if (errorCode != Error.None)
            {
                typeTag = default(TypeTag);
                typeId = default(ReferenceTypeId);
                return errorCode;
            }

            int offset = HeaderSize;
            typeTag = (TypeTag)ReadByte(response, ref offset);
            typeId = ReadReferenceTypeId(response, ref offset);
            return Error.None;
        }

        private static void SerializeHeader(byte[] packet, int id, VirtualMachineCommand command)
        {
            SerializeHeader(packet, packet.Length, id, 0, (byte)CommandSet.VirtualMachine, (byte)command);
        }

        private static void SerializeHeader(byte[] packet, int id, ReferenceTypeCommand command)
        {
            SerializeHeader(packet, packet.Length, id, 0, (byte)CommandSet.ReferenceType, (byte)command);
        }

        private static void SerializeHeader(byte[] packet, int id, ClassTypeCommand command)
        {
            SerializeHeader(packet, packet.Length, id, 0, (byte)CommandSet.ClassType, (byte)command);
        }

        private static void SerializeHeader(byte[] packet, int id, MethodCommand command)
        {
            SerializeHeader(packet, packet.Length, id, 0, (byte)CommandSet.Method, (byte)command);
        }

        private static void SerializeHeader(byte[] packet, int id, ObjectReferenceCommand command)
        {
            SerializeHeader(packet, packet.Length, id, 0, (byte)CommandSet.ObjectReference, (byte)command);
        }

        private static void SerializeHeader(byte[] packet, int id, StringReferenceCommand command)
        {
            SerializeHeader(packet, packet.Length, id, 0, (byte)CommandSet.StringReference, (byte)command);
        }

        private static void SerializeHeader(byte[] packet, int id, ThreadReferenceCommand command)
        {
            SerializeHeader(packet, packet.Length, id, 0, (byte)CommandSet.ThreadReference, (byte)command);
        }

        private static void SerializeHeader(byte[] packet, int id, ArrayReferenceCommand command)
        {
            SerializeHeader(packet, packet.Length, id, 0, (byte)CommandSet.ArrayReference, (byte)command);
        }

        private static void SerializeHeader(byte[] packet, int id, EventRequestCommand command)
        {
            SerializeHeader(packet, packet.Length, id, 0, (byte)CommandSet.EventRequest, (byte)command);
        }

        private static void SerializeHeader(byte[] packet, int id, StackFrameCommand command)
        {
            SerializeHeader(packet, packet.Length, id, 0, (byte)CommandSet.StackFrame, (byte)command);
        }

        private static void SerializeHeader(byte[] packet, int id, ClassObjectReferenceCommand command)
        {
            SerializeHeader(packet, packet.Length, id, 0, (byte)CommandSet.ClassObjectReference, (byte)command);
        }

        private static void SerializeHeader(byte[] packet, int length, int id, byte flags, byte commandSet, byte command)
        {
            WriteInt32(packet, 0, length);
            WriteInt32(packet, 4, id);
            packet[8] = flags;
            packet[9] = commandSet;
            packet[10] = command;
        }

        private static void WriteInt16(byte[] data, int offset, short value)
        {
            data[offset + 0] = (byte)(value >> 8);
            data[offset + 1] = (byte)value;
        }

        private static void WriteInt32(byte[] data, int offset, int value)
        {
            data[offset + 0] = (byte)(value >> 24);
            data[offset + 1] = (byte)(value >> 16);
            data[offset + 2] = (byte)(value >> 8);
            data[offset + 3] = (byte)value;
        }

        private static void WriteInt64(byte[] data, int offset, long value)
        {
            WriteInt32(data, offset, (int)(value >> 32));
            WriteInt32(data, offset + sizeof(int), (int)value);
        }

        private byte[] SendPacket(int id, byte[] packet)
        {
            TaskCompletionSource<byte[]> completionSource = new TaskCompletionSource<byte[]>();
            if (!_tasks.TryAdd(id, completionSource))
                throw new InvalidOperationException("Attempted to send a packet using the same ID as another in-progress operation.");

            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.SetBuffer(packet, 0, packet.Length);
            _socket.SendAsync(e);

            completionSource.Task.Wait(_cancellationTokenSource.Token);
            return completionSource.Task.Result;
        }

        private int GetMessageId()
        {
            return Interlocked.Increment(ref _nextMessageId);
        }

        private static string ReadString(byte[] response, ref int offset)
        {
            int length = ReadInt32(response, ref offset);
            string result = Encoding.UTF8.GetString(response, offset, length);
            offset += length;
            return result;
        }

        private static byte ReadByte(byte[] response, ref int offset)
        {
            byte result = response[offset];
            offset++;
            return result;
        }

        private static bool ReadBoolean(byte[] data, ref int offset)
        {
            return ReadByte(data, ref offset) != 0;
        }

        private static short ReadInt16(byte[] response, ref int offset)
        {
            return (short)ReadUInt16(response, ref offset);
        }

        private static ushort ReadUInt16(byte[] response, ref int offset)
        {
            ushort result = (ushort)(response[offset + 1] | (response[offset] << 8));
            offset += 2;
            return result;
        }

        private static char ReadChar(byte[] response, ref int offset)
        {
            return (char)ReadUInt16(response, ref offset);
        }

        private static int ReadInt32(byte[] response, ref int offset)
        {
            return (int)ReadUInt32(response, ref offset);
        }

        private static uint ReadUInt32(byte[] response, ref int offset)
        {
            ushort highOrder = ReadUInt16(response, ref offset);
            ushort lowOrder = ReadUInt16(response, ref offset);
            return (uint)(lowOrder | (highOrder << 16));
        }

        private static long ReadInt64(byte[] response, ref int offset)
        {
            return (long)ReadUInt64(response, ref offset);
        }

        private static ulong ReadUInt64(byte[] response, ref int offset)
        {
            uint highOrder = ReadUInt32(response, ref offset);
            uint lowOrder = ReadUInt32(response, ref offset);
            return (ulong)(lowOrder | ((long)highOrder << 32));
        }

        private static Error ReadErrorCode(byte[] response)
        {
            int errorCodeOffset = HeaderSize - 2;
            short value = ReadInt16(response, ref errorCodeOffset);
            return (Error)value;
        }

        private ReferenceTypeId ReadReferenceTypeId(byte[] packet, ref int offset)
        {
            if (!_referenceTypeIdSize.HasValue)
                throw new InvalidOperationException();

            switch (_referenceTypeIdSize.Value)
            {
            case 2:
                return new ReferenceTypeId(ReadInt16(packet, ref offset));

            case 4:
                return new ReferenceTypeId(ReadInt32(packet, ref offset));

            case 8:
                return new ReferenceTypeId(ReadInt64(packet, ref offset));

            default:
                throw new NotImplementedException();
            }
        }

        private void WriteReferenceTypeId(byte[] packet, int offset, ReferenceTypeId referenceTypeId)
        {
            if (!_referenceTypeIdSize.HasValue)
                throw new InvalidOperationException();

            switch (_referenceTypeIdSize.Value)
            {
            case 2:
                WriteInt16(packet, offset, (short)referenceTypeId.Handle);
                break;

            case 4:
                WriteInt32(packet, offset, (int)referenceTypeId.Handle);
                break;

            case 8:
                WriteInt64(packet, offset, referenceTypeId.Handle);
                break;

            default:
                throw new NotImplementedException();
            }
        }

        private FieldId ReadFieldId(byte[] packet, ref int offset)
        {
            if (!_fieldIdSize.HasValue)
                throw new InvalidOperationException();

            switch (_fieldIdSize.Value)
            {
            case 2:
                return new FieldId(ReadInt16(packet, ref offset));

            case 4:
                return new FieldId(ReadInt32(packet, ref offset));

            case 8:
                return new FieldId(ReadInt64(packet, ref offset));

            default:
                throw new NotImplementedException();
            }
        }

        private void WriteFieldId(byte[] packet, int offset, FieldId fieldId)
        {
            if (!_fieldIdSize.HasValue)
                throw new InvalidOperationException();

            switch (_fieldIdSize.Value)
            {
            case 2:
                WriteInt16(packet, offset, (short)fieldId.Handle);
                break;

            case 4:
                WriteInt32(packet, offset, (int)fieldId.Handle);
                break;

            case 8:
                WriteInt64(packet, offset, fieldId.Handle);
                break;

            default:
                throw new NotImplementedException();
            }
        }

        private MethodId ReadMethodId(byte[] packet, ref int offset)
        {
            if (!_methodIdSize.HasValue)
                throw new InvalidOperationException();

            switch (_methodIdSize.Value)
            {
            case 2:
                return new MethodId(ReadInt16(packet, ref offset));

            case 4:
                return new MethodId(ReadInt32(packet, ref offset));

            case 8:
                return new MethodId(ReadInt64(packet, ref offset));

            default:
                throw new NotImplementedException();
            }
        }

        private void WriteMethodId(byte[] packet, int offset, MethodId methodId)
        {
            if (!_methodIdSize.HasValue)
                throw new InvalidOperationException();

            switch (_methodIdSize.Value)
            {
            case 2:
                WriteInt16(packet, offset, (short)methodId.Handle);
                break;

            case 4:
                WriteInt32(packet, offset, (int)methodId.Handle);
                break;

            case 8:
                WriteInt64(packet, offset, methodId.Handle);
                break;

            default:
                throw new NotImplementedException();
            }
        }

        private FrameId ReadFrameId(byte[] packet, ref int offset)
        {
            switch (FrameIdSize)
            {
            case 2:
                return new FrameId(ReadInt16(packet, ref offset));

            case 4:
                return new FrameId(ReadInt32(packet, ref offset));

            case 8:
                return new FrameId(ReadInt64(packet, ref offset));

            default:
                throw new NotImplementedException();
            }
        }

        private void WriteFrameId(byte[] packet, int offset, FrameId frameId)
        {
            switch (FrameIdSize)
            {
            case 2:
                WriteInt16(packet, offset, (short)frameId.Handle);
                break;

            case 4:
                WriteInt32(packet, offset, (int)frameId.Handle);
                break;

            case 8:
                WriteInt64(packet, offset, frameId.Handle);
                break;

            default:
                throw new NotImplementedException();
            }
        }

        private ObjectId ReadObjectId(byte[] packet, ref int offset)
        {
            if (!_objectIdSize.HasValue)
                throw new InvalidOperationException();

            switch (_referenceTypeIdSize.Value)
            {
            case 2:
                return new ObjectId(ReadInt16(packet, ref offset));

            case 4:
                return new ObjectId(ReadInt32(packet, ref offset));

            case 8:
                return new ObjectId(ReadInt64(packet, ref offset));

            default:
                throw new NotImplementedException();
            }
        }

        private void WriteObjectId(byte[] packet, int offset, ObjectId objectId)
        {
            if (!_objectIdSize.HasValue)
                throw new InvalidOperationException();

            switch (_objectIdSize.Value)
            {
            case 2:
                WriteInt16(packet, offset, (short)objectId.Handle);
                break;

            case 4:
                WriteInt32(packet, offset, (int)objectId.Handle);
                break;

            case 8:
                WriteInt64(packet, offset, objectId.Handle);
                break;

            default:
                throw new NotImplementedException();
            }
        }

        private TaggedObjectId ReadTaggedObjectId(byte[] packet, ref int offset)
        {
            if (!_objectIdSize.HasValue)
                throw new InvalidOperationException();

            Tag tag = (Tag)ReadByte(packet, ref offset);
            ObjectId objectId = ReadObjectId(packet, ref offset);
            return new TaggedObjectId(tag, objectId, default(TaggedReferenceTypeId));
        }

        private Value ReadValue(byte[] response, ref int offset)
        {
            Tag tag = (Tag)ReadByte(response, ref offset);
            return ReadUntaggedValue(response, ref offset, tag);
        }

        private Value ReadUntaggedValue(byte[] response, ref int offset, Tag tag)
        {
            switch (tag)
            {
            case Tag.Byte:
                {
                    byte value = ReadByte(response, ref offset);
                    return new Value(tag, value);
                }

            case Tag.Char:
                {
                    char value = ReadChar(response, ref offset);
                    return new Value(tag, value);
                }

            case Tag.Float:
                {
                    uint value = ReadUInt32(response, ref offset);
                    return new Value(tag, value);
                }

            case Tag.Double:
                {
                    ulong value = ReadUInt64(response, ref offset);
                    return new Value(tag, (long)value);
                }

            case Tag.Int:
                {
                    int value = ReadInt32(response, ref offset);
                    return new Value(tag, value);
                }

            case Tag.Long:
                {
                    long value = ReadInt64(response, ref offset);
                    return new Value(tag, value);
                }

            case Tag.Short:
                {
                    short value = ReadInt16(response, ref offset);
                    return new Value(tag, value);
                }

            case Tag.Void:
                return new Value(tag, 0);

            case Tag.Boolean:
                {
                    byte value = ReadByte(response, ref offset);
                    return new Value(tag, value);
                }

            case Tag.Array:
            case Tag.Object:
            case Tag.String:
            case Tag.Thread:
            case Tag.ThreadGroup:
            case Tag.ClassLoader:
            case Tag.ClassObject:
                {
                    ObjectId objectId = ReadObjectId(response, ref offset);
                    return new Value(tag, objectId.Handle);
                }

            case Tag.Invalid:
            default:
                throw new NotImplementedException();
            }
        }

        private Location ReadLocation(byte[] packet, ref int offset)
        {
            TypeTag typeTag = (TypeTag)ReadByte(packet, ref offset);
            ClassId classId = (ClassId)ReadReferenceTypeId(packet, ref offset);
            MethodId methodId = ReadMethodId(packet, ref offset);
            ulong index = ReadUInt64(packet, ref offset);
            return new Location(typeTag, classId, methodId, index);
        }

        private void WriteLocation(byte[] data, int offset, Location location)
        {
            data[offset] = (byte)location.TypeTag;
            WriteReferenceTypeId(data, sizeof(byte), location.Class);
            WriteMethodId(data, sizeof(byte) + ReferenceTypeIdSize, location.Method);
            WriteInt64(data, sizeof(byte) + ReferenceTypeIdSize + MethodIdSize, (long)location.Index);
        }
    }
}
