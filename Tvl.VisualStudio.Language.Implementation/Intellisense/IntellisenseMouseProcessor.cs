namespace Tvl.VisualStudio.Language.Intellisense.Implementation
{
    using System.Collections.Generic;
    using System.Linq;
    using DragEventArgs = System.Windows.DragEventArgs;
    using GiveFeedbackEventArgs = System.Windows.GiveFeedbackEventArgs;
    using IMouseProcessor = Microsoft.VisualStudio.Text.Editor.IMouseProcessor;
    using ITextView = Microsoft.VisualStudio.Text.Editor.ITextView;
    using MouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;
    using MouseEventArgs = System.Windows.Input.MouseEventArgs;
    using MouseWheelEventArgs = System.Windows.Input.MouseWheelEventArgs;
    using QueryContinueDragEventArgs = System.Windows.QueryContinueDragEventArgs;

    public class IntellisenseMouseProcessor : IMouseProcessor
    {
        private readonly ITextView _textView;

        public IntellisenseMouseProcessor(ITextView textView)
        {
            _textView = textView;
        }

        public ITextView TextView
        {
            get
            {
                return _textView;
            }
        }

        public void DefaultPreprocessMouseLeftButtonDown(ITvlIntellisenseController controller, MouseButtonEventArgs e)
        {
            if (controller != null)
            {
                controller.DismissCompletion();
                controller.DismissQuickInfo();
            }
        }

        public void DefaultPreprocessMouseRightButtonDown(ITvlIntellisenseController controller, MouseButtonEventArgs e)
        {
            if (controller != null)
            {
                controller.DismissCompletion();
                controller.DismissQuickInfo();
            }
        }

        #region IMouseProcessor Members

        void IMouseProcessor.PostprocessDragEnter(DragEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessDragEnter(e);
            }
        }

        void IMouseProcessor.PostprocessDragLeave(DragEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessDragLeave(e);
            }
        }

        void IMouseProcessor.PostprocessDragOver(DragEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessDragOver(e);
            }
        }

        void IMouseProcessor.PostprocessDrop(DragEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessDrop(e);
            }
        }

        void IMouseProcessor.PostprocessGiveFeedback(GiveFeedbackEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessGiveFeedback(e);
            }
        }

        void IMouseProcessor.PostprocessMouseDown(MouseButtonEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessMouseDown(e);
            }
        }

        void IMouseProcessor.PostprocessMouseEnter(MouseEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessMouseEnter(e);
            }
        }

        void IMouseProcessor.PostprocessMouseLeave(MouseEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessMouseLeave(e);
            }
        }

        void IMouseProcessor.PostprocessMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessMouseLeftButtonDown(e);
            }
        }

        void IMouseProcessor.PostprocessMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessMouseLeftButtonUp(e);
            }
        }

        void IMouseProcessor.PostprocessMouseMove(MouseEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessMouseMove(e);
            }
        }

        void IMouseProcessor.PostprocessMouseRightButtonDown(MouseButtonEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessMouseRightButtonDown(e);
            }
        }

        void IMouseProcessor.PostprocessMouseRightButtonUp(MouseButtonEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessMouseRightButtonUp(e);
            }
        }

        void IMouseProcessor.PostprocessMouseUp(MouseButtonEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessMouseUp(e);
            }
        }

        void IMouseProcessor.PostprocessMouseWheel(MouseWheelEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessMouseWheel(e);
            }
        }

        void IMouseProcessor.PostprocessQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PostprocessQueryContinueDrag(e);
            }
        }

        void IMouseProcessor.PreprocessDragEnter(DragEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessDragEnter(e);
            }
        }

        void IMouseProcessor.PreprocessDragLeave(DragEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessDragLeave(e);
            }
        }

        void IMouseProcessor.PreprocessDragOver(DragEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessDragOver(e);
            }
        }

        void IMouseProcessor.PreprocessDrop(DragEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessDrop(e);
            }
        }

        void IMouseProcessor.PreprocessGiveFeedback(GiveFeedbackEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessGiveFeedback(e);
            }
        }

        void IMouseProcessor.PreprocessMouseDown(MouseButtonEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessMouseDown(e);
            }
        }

        void IMouseProcessor.PreprocessMouseEnter(MouseEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessMouseEnter(e);
            }
        }

        void IMouseProcessor.PreprocessMouseLeave(MouseEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessMouseLeave(e);
            }
        }

        void IMouseProcessor.PreprocessMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessMouseLeftButtonDown(e);
                else
                    DefaultPreprocessMouseLeftButtonDown(controller, e);
            }
        }

        void IMouseProcessor.PreprocessMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessMouseLeftButtonUp(e);
            }
        }

        void IMouseProcessor.PreprocessMouseMove(MouseEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessMouseMove(e);
            }
        }

        void IMouseProcessor.PreprocessMouseRightButtonDown(MouseButtonEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessMouseRightButtonDown(e);
                else
                    DefaultPreprocessMouseRightButtonDown(controller, e);
            }
        }

        void IMouseProcessor.PreprocessMouseRightButtonUp(MouseButtonEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessMouseRightButtonUp(e);
            }
        }

        void IMouseProcessor.PreprocessMouseUp(MouseButtonEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessMouseUp(e);
            }
        }

        void IMouseProcessor.PreprocessMouseWheel(MouseWheelEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessMouseWheel(e);
            }
        }

        void IMouseProcessor.PreprocessQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            foreach (var controller in GetControllersForView(TextView))
            {
                IMouseProcessor customProcessor = controller.CustomMouseProcessor;
                if (customProcessor != null)
                    customProcessor.PreprocessQueryContinueDrag(e);
            }
        }

        #endregion

        private static IEnumerable<ITvlIntellisenseController> GetControllersForView(ITextView textView)
        {
            object controllersObject;
            if (!textView.Properties.TryGetProperty(typeof(ITvlIntellisenseController), out controllersObject))
                return Enumerable.Empty<ITvlIntellisenseController>();

            IEnumerable<ITvlIntellisenseController> controllers = controllersObject as IEnumerable<ITvlIntellisenseController>;
            if (controllers == null)
                return Enumerable.Empty<ITvlIntellisenseController>();

            return controllers.OfType<ITvlIntellisenseController>();
        }
    }
}
