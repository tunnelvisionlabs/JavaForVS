namespace Tvl.VisualStudio.Language.Java.Debugger.Events
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;

    [ComVisible(true)]
    public class DebugExpressionEvaluationCompleteEvent : DebugEvent, IDebugExpressionEvaluationCompleteEvent2
    {
        private readonly IDebugExpression2 _expression;
        private readonly IDebugProperty2 _property;

        public DebugExpressionEvaluationCompleteEvent(enum_EVENTATTRIBUTES attributes, IDebugExpression2 expression, IDebugProperty2 property)
            : base(attributes)
        {
            Contract.Requires<ArgumentNullException>(expression != null, "expression");
            Contract.Requires<ArgumentNullException>(property != null, "property");

            _expression = expression;
            _property = property;
        }

        public override Guid EventGuid
        {
            get
            {
                return typeof(IDebugExpressionEvaluationCompleteEvent2).GUID;
            }
        }

        public int GetExpression(out IDebugExpression2 ppExpr)
        {
            ppExpr = _expression;
            return VSConstants.S_OK;
        }

        public int GetResult(out IDebugProperty2 ppResult)
        {
            ppResult = _property;
            return VSConstants.S_OK;
        }
    }
}
