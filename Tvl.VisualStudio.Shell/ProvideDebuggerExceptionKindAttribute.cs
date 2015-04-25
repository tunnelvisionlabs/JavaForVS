namespace Tvl.VisualStudio.Shell
{
    using System;
    using Contract = System.Diagnostics.Contracts.Contract;
    using enum_EXCEPTION_STATE = Microsoft.VisualStudio.Debugger.Interop.enum_EXCEPTION_STATE;
    using RegistrationAttribute = Microsoft.VisualStudio.Shell.RegistrationAttribute;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ProvideDebuggerExceptionKindAttribute : RegistrationAttribute
    {
        private readonly Guid _debugEngine;
        private readonly string _exceptionKind;
        private int _code;
        private enum_EXCEPTION_STATE _state = ProvideDebuggerExceptionAttribute.DefaultState;

        public ProvideDebuggerExceptionKindAttribute(string debugEngineGuid, string exceptionKind)
        {
            Contract.Requires<ArgumentNullException>(debugEngineGuid != null, "debugEngineGuid");
            Contract.Requires<ArgumentNullException>(exceptionKind != null, "exceptionKind");

            _debugEngine = Guid.Parse(debugEngineGuid);
            _exceptionKind = exceptionKind;
        }

        public Guid DebugEngine
        {
            get
            {
                return _debugEngine;
            }
        }

        public string ExceptionKind
        {
            get
            {
                return _exceptionKind;
            }
        }

        public int Code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
            }
        }

        public enum_EXCEPTION_STATE State
        {
            get
            {
                return _state;
            }

            set
            {
                _state = value;
            }
        }

        private string RegKeyBaseName
        {
            get
            {
                return string.Format(@"AD7Metrics\Exception\{0:B}\{1}", DebugEngine, ExceptionKind);
            }
        }

        public override void Register(RegistrationContext context)
        {
            using (Key key = context.CreateKey(RegKeyBaseName))
            {
                key.SetValue("Code", Code);
                key.SetValue("State", (int)State);
            }
        }

        public override void Unregister(RegistrationContext context)
        {
            context.RemoveValue(RegKeyBaseName, "Code");
            context.RemoveValue(RegKeyBaseName, "State");
        }
    }
}
