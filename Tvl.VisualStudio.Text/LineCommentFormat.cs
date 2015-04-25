namespace Tvl.VisualStudio.Text
{
    public class LineCommentFormat : CommentFormat
    {
        private readonly string _startText;

        public LineCommentFormat(string startText)
        {
            _startText = startText;
        }

        public string StartText
        {
            get
            {
                return _startText;
            }
        }
    }
}
