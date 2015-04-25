namespace Tvl.Java.DebugInterface.Client
{
    using Tvl.Java.DebugInterface.Types;

    internal static class DebugErrorHandler
    {
        public static void ThrowOnFailure(Error error)
        {
            if (error == Error.None)
                return;

            switch (error)
            {
            case Error.None:
                return;

            case Error.InvalidThread:
                break;

            case Error.InvalidThreadGroup:
                break;

            case Error.InvalidPriority:
                break;

            case Error.ThreadNotSuspended:
                break;

            case Error.ThreadSuspended:
                break;

            case Error.InvalidObject:
                break;

            case Error.InvalidClass:
                break;

            case Error.ClassNotPrepared:
                break;

            case Error.InvalidMethodid:
                break;

            case Error.InvalidLocation:
                break;

            case Error.InvalidFieldid:
                break;

            case Error.InvalidFrameid:
                break;

            case Error.NoMoreFrames:
                break;

            case Error.OpaqueFrame:
                break;

            case Error.NotCurrentFrame:
                break;

            case Error.TypeMismatch:
                break;

            case Error.InvalidSlot:
                break;

            case Error.Duplicate:
                break;

            case Error.NotFound:
                break;

            case Error.InvalidMonitor:
                break;

            case Error.NotMonitorOwner:
                break;

            case Error.Interrupt:
                break;

            case Error.InvalidClassFormat:
                break;

            case Error.CircularClassDefinition:
                break;

            case Error.FailsVerification:
                break;

            case Error.AddMethodNotImplemented:
                break;

            case Error.SchemaChangeNotImplemented:
                break;

            case Error.InvalidTypestate:
                break;

            case Error.HierarchyChangeNotImplemented:
                break;

            case Error.DeleteMethodNotImplemented:
                break;

            case Error.UnsupportedVersion:
                break;

            case Error.NamesDontMatch:
                break;

            case Error.ClassModifiersChangeNotImplemented:
                break;

            case Error.MethodModifiersChangeNotImplemented:
                break;

            case Error.NotImplemented:
                break;

            case Error.NullPointer:
                break;

            case Error.AbsentInformation:
                throw new MissingInformationException();

            case Error.InvalidEventType:
                break;

            case Error.IllegalArgument:
                break;

            case Error.OutOfMemory:
                break;

            case Error.AccessDenied:
                break;

            case Error.VmDead:
                break;

            case Error.Internal:
                break;

            case Error.UnattachedThread:
                break;

            case Error.InvalidTag:
                break;

            case Error.AlreadyInvoking:
                break;

            case Error.InvalidIndex:
                break;

            case Error.InvalidLength:
                break;

            case Error.InvalidString:
                break;

            case Error.InvalidClassLoader:
                break;

            case Error.InvalidArray:
                break;

            case Error.TransportLoad:
                break;

            case Error.TransportInit:
                break;

            case Error.NativeMethod:
                break;

            case Error.InvalidCount:
                break;

            default:
                break;
            }

            throw new InternalException((int)error, error.ToString());
        }
    }
}
