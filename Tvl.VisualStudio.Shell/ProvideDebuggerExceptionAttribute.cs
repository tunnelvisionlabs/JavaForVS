namespace Tvl.VisualStudio.Shell
{
    using System;
    using Contract = System.Diagnostics.Contracts.Contract;
    using enum_EXCEPTION_STATE = Microsoft.VisualStudio.Debugger.Interop.enum_EXCEPTION_STATE;
    using RegistrationAttribute = Microsoft.VisualStudio.Shell.RegistrationAttribute;
    using VSConstants = Microsoft.VisualStudio.VSConstants;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ProvideDebuggerExceptionAttribute : RegistrationAttribute
    {
        public const enum_EXCEPTION_STATE DefaultState =
            enum_EXCEPTION_STATE.EXCEPTION_STOP_USER_UNCAUGHT
            | enum_EXCEPTION_STATE.EXCEPTION_STOP_SECOND_CHANCE;

        private readonly Guid _debugEngine;
        private readonly string _exceptionKind;
        private readonly string _exceptionNamespace;
        private readonly string _exceptionName;
        private int _code;
        private enum_EXCEPTION_STATE _state = DefaultState;

        public ProvideDebuggerExceptionAttribute(string debugEngine, string exceptionKind, string exceptionName)
            : this(debugEngine, exceptionKind, null, exceptionName)
        {
        }

        public ProvideDebuggerExceptionAttribute(Type exception)
            : this(VSConstants.DebugEnginesGuids.ManagedOnly_string, "Common Language Runtime Exceptions", exception.Namespace, exception.FullName)
        {
            _state = DefaultState | enum_EXCEPTION_STATE.EXCEPTION_JUST_MY_CODE_SUPPORTED;
        }

        public ProvideDebuggerExceptionAttribute(string debugEngineGuid, string exceptionKind, string exceptionNamespace, string exceptionName)
        {
            Contract.Requires<ArgumentNullException>(debugEngineGuid != null, "debugEngineGuid");
            Contract.Requires<ArgumentNullException>(exceptionKind != null, "exceptionKind");
            Contract.Requires<ArgumentNullException>(exceptionName != null, "exceptionName");

            _debugEngine = Guid.Parse(debugEngineGuid);
            _exceptionKind = exceptionKind;
            _exceptionName = exceptionName;
            _exceptionNamespace = exceptionNamespace;
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

        public string ExceptionNamespace
        {
            get
            {
                return _exceptionNamespace;
            }
        }

        public string ExceptionName
        {
            get
            {
                return _exceptionName;
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
                Key parentKey = key;
                if (!string.IsNullOrEmpty(ExceptionNamespace))
                    parentKey = key.CreateSubkey(ExceptionNamespace);

                try
                {
                    if (!string.IsNullOrEmpty(ExceptionNamespace))
                    {
                        parentKey.SetValue("Code", 0);
                        parentKey.SetValue("State", (int)DefaultState);
                    }

                    using (Key child = parentKey.CreateSubkey(ExceptionName))
                    {
                        child.SetValue("Code", Code);
                        child.SetValue("State", (int)State);
                    }
                }
                finally
                {
                    if (!string.IsNullOrEmpty(ExceptionNamespace))
                        parentKey.Close();
                }
            }
        }

        public override void Unregister(RegistrationContext context)
        {
            string name;
            if (!string.IsNullOrEmpty(ExceptionNamespace))
                name = string.Format(@"{0}\{1}", ExceptionNamespace, ExceptionName);
            else
                name = ExceptionName;

            string regKeyName = string.Format(@"AD7Metrics\Exception\{0:B}", DebugEngine, ExceptionKind, name);
            context.RemoveKey(regKeyName);
        }
    }
}
