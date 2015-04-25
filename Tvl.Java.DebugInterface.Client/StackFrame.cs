namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using FrameId = Tvl.Java.DebugInterface.Types.FrameId;

    internal sealed class StackFrame : Mirror, IStackFrame
    {
        private readonly FrameId _frameId;
        private readonly ThreadReference _thread;
        private readonly Location _location;

        internal StackFrame(VirtualMachine virtualMachine, Types.FrameId frameId, ThreadReference thread, Location location)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
            Contract.Requires<ArgumentNullException>(thread != null, "thread");
            Contract.Requires<ArgumentNullException>(location != null, "location");

            _thread = thread;
            _frameId = frameId;
            _location = location;
        }

        public FrameId FrameId
        {
            get
            {
                return _frameId;
            }
        }

        public ThreadReference Thread
        {
            get
            {
                return _thread;
            }
        }

        public Location Location
        {
            get
            {
                return _location;
            }
        }

        #region IStackFrame Members

        public bool GetHasVariableInfo()
        {
            return _location.Method.GetHasVariableInfo();
        }

        public ReadOnlyCollection<IValue> GetArgumentValues()
        {
            ILocalVariable[] arguments = GetLocation().GetMethod().GetArguments().ToArray();
            IValue[] values = Array.ConvertAll(arguments, this.GetValue);
            return new ReadOnlyCollection<IValue>(values);
        }

        public IValue GetValue(ILocalVariable variable)
        {
            LocalVariable localVariable = variable as LocalVariable;
            if (localVariable == null)
                throw new VirtualMachineMismatchException();

            int[] slots = { localVariable.Slot };
            Types.Value[] values;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetValues(out values, _thread.ThreadId, _frameId, slots));
            return VirtualMachine.GetMirrorOf(values[0]);
        }

        public IDictionary<ILocalVariable, IValue> GetValues(IEnumerable<ILocalVariable> variables)
        {
            Dictionary<ILocalVariable, IValue> result = new Dictionary<ILocalVariable, IValue>();
            foreach (var variable in variables)
                result.Add(variable, GetValue(variable));

            return result;
        }

        public void SetValue(ILocalVariable variable, IValue value)
        {
            LocalVariable localVariable = variable as LocalVariable;
            if (localVariable == null)
                throw new VirtualMachineMismatchException();

            Value trueValue = value as Value;
            if (trueValue == null && value != null)
                throw new VirtualMachineMismatchException();

            int[] slots = { localVariable.Slot };
            Types.Value[] values = { Value.ToNetworkValue(trueValue) };
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.SetValues(_thread.ThreadId, _frameId, slots, values));
        }

        public IObjectReference GetThisObject()
        {
            Types.TaggedObjectId thisObject;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetThisObject(out thisObject, _thread.ThreadId, FrameId));
            return VirtualMachine.GetMirrorOf(thisObject);
        }

        public IThreadReference GetThread()
        {
            return _thread;
        }

        public ILocalVariable GetVisibleVariableByName(string name)
        {
            return GetVisibleVariables().SingleOrDefault(i => i.GetName() == name);
        }

        public ReadOnlyCollection<ILocalVariable> GetVisibleVariables()
        {
            List<ILocalVariable> visible = new List<ILocalVariable>(GetLocation().GetMethod().GetVariables().Where(i => i.GetIsVisible(this)));
            return visible.AsReadOnly();
        }

        #endregion

        #region ILocatable Members

        public ILocation GetLocation()
        {
            return _location;
        }

        #endregion
    }
}
