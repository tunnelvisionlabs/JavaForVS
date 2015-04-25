namespace Tvl.VisualStudio.Text
{
    public class BlockCommentFormat : CommentFormat
    {
        private readonly string _startText;
        private readonly string _endText;
        private readonly bool _allowNesting;

        public BlockCommentFormat(string startText, string endText, bool allowNesting = false)
        {
            _startText = startText;
            _endText = endText;
            _allowNesting = allowNesting;
        }

        public string StartText
        {
            get
            {
                return _startText;
            }
        }

        public string EndText
        {
            get
            {
                return _endText;
            }
        }

        public bool AllowNesting
        {
            get
            {
                return _allowNesting;
            }
        }
    }
}
