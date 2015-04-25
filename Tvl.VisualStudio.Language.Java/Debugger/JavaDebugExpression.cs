namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Antlr.Runtime.Tree;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Debugger.Interop;
    using Tvl.Java.DebugInterface;
    using Tvl.VisualStudio.Language.Java.Debugger.Events;

    [ComVisible(true)]
    public class JavaDebugExpression : IDebugExpression2
    {
        private readonly JavaDebugExpressionContext _context;
        private readonly CommonTree _expression;
        private readonly string _expressionText;

        public JavaDebugExpression(JavaDebugExpressionContext context, CommonTree expression, string expressionText)
        {
            Contract.Requires<ArgumentNullException>(context != null, "context");
            Contract.Requires<ArgumentNullException>(expression != null, "expression");
            Contract.Requires<ArgumentNullException>(expressionText != null, "expressionText");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(expressionText));

            _context = context;
            _expression = expression;
            _expressionText = expressionText;
        }

        #region IDebugExpression2 Members

        public int Abort()
        {
            throw new NotImplementedException();
        }

        public int EvaluateAsync(enum_EVALFLAGS dwFlags, IDebugEventCallback2 pExprCallback)
        {
            // don't use pExprCallback!

            IDebugEventCallback2 callback = _context.StackFrame.Thread.Program.Callback;
            Task<IDebugProperty2> evaluateTask = Task.Factory.StartNew(() => EvaluateImpl(dwFlags)).HandleNonCriticalExceptions();
            Task successCompletionTask = evaluateTask.ContinueWith(task => SendEvaluationCompleteEvent(task, callback), TaskContinuationOptions.OnlyOnRanToCompletion).HandleNonCriticalExceptions();
            Task failureCompletionTask = evaluateTask.ContinueWith(task => SendEvaluationCompleteEvent(null, callback), TaskContinuationOptions.NotOnRanToCompletion).HandleNonCriticalExceptions();
            return VSConstants.S_OK;
        }

        /// <summary>
        /// This method evaluates the expression synchronously.
        /// </summary>
        /// <param name="dwFlags">[in] A combination of flags from the EVALFLAGS enumeration that control expression evaluation.</param>
        /// <param name="dwTimeout">[in] Maximum time, in milliseconds, to wait before returning from this method. Use INFINITE to wait indefinitely.</param>
        /// <param name="pExprCallback">[in]This parameter is always a null value.</param>
        /// <param name="ppResult">[out] Returns the IDebugProperty2 object that contains the result of the expression evaluation.</param>
        /// <returns>
        /// If successful, returns S_OK; otherwise returns an error code. Some typical error codes are:
        ///  * E_EVALUATE_BUSY_WITH_EVALUATION  Another expression is currently being evaluated, and simultaneous
        ///                                     expression evaluation is not supported.
        ///  * E_EVALUATE_TIMEOUT               Evaluation timed out.
        /// </returns>
        /// <remarks>
        /// For synchronous evaluation, it is not necessary to send an event back to Visual Studio upon completion of the evaluation.
        /// </remarks>
        public int EvaluateSync(enum_EVALFLAGS dwFlags, uint dwTimeout, IDebugEventCallback2 pExprCallback, out IDebugProperty2 ppResult)
        {
            ppResult = null;

            Task<IDebugProperty2> task = Task.Factory.StartNew(() => EvaluateImpl(dwFlags)).HandleNonCriticalExceptions();
            if (!task.Wait((int)dwTimeout))
                return AD7Constants.E_EVALUATE_TIMEOUT;

            if (task.Status != TaskStatus.RanToCompletion || task.Result == null)
                return VSConstants.E_FAIL;

            ppResult = task.Result;
            return VSConstants.S_OK;
        }

        #endregion

        private IDebugProperty2 EvaluateImpl(enum_EVALFLAGS flags)
        {
            ITreeNodeStream input = new CommonTreeNodeStream(_expression);
            DebugExpressionEvaluatorWalker walker = new DebugExpressionEvaluatorWalker(input, _context.StackFrame.StackFrame);
            EvaluatedExpression evaluatedExpression = walker.standaloneExpression();

            // make sure the Name and FullName of the evaluated expression match what the user typed
            evaluatedExpression = new EvaluatedExpression(
                _expressionText,
                _expressionText,
                evaluatedExpression.LocalVariable,
                evaluatedExpression.Referencer,
                evaluatedExpression.Field,
                evaluatedExpression.Method,
                evaluatedExpression.Index,
                evaluatedExpression.Value,
                evaluatedExpression.ValueType,
                evaluatedExpression.StrongReference,
                evaluatedExpression.HasSideEffects);

            return new JavaDebugProperty(null, evaluatedExpression);
        }

        private void SendEvaluationCompleteEvent(Task<IDebugProperty2> task, IDebugEventCallback2 callback)
        {
            var thread = _context.StackFrame.Thread;
            var program = thread.Program;
            var engine = program.DebugEngine;
            var process = program.Process;

            IDebugProperty2 property;
            if (task != null)
                property = task.Result;
            else
                property = new JavaDebugProperty(null, new EvaluatedExpression("?", "?", default(IValue), false));

            DebugEvent debugEvent = new DebugExpressionEvaluationCompleteEvent(enum_EVENTATTRIBUTES.EVENT_ASYNCHRONOUS, this, property);
            callback.Event(engine, process, program, thread, debugEvent);
        }
    }
}
