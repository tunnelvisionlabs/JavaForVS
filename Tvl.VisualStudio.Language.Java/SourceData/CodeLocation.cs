namespace Tvl.VisualStudio.Language.Java.SourceData
{
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public class CodeLocation
    {
        private static readonly string _abstractLocation = "<ABSTRACT>";
        private static readonly CodeLocation _abstract = new CodeLocation(_abstractLocation);
        private static readonly string _intrinsicLocation = "<INTRINSIC>";
        private static readonly CodeLocation _intrinsic = new CodeLocation(_intrinsicLocation);

        private readonly string _fileName;
        private readonly Interval? _span;
        private readonly Interval? _seek;

        public CodeLocation(string fileName)
        {
            _fileName = fileName;
        }

        public CodeLocation(string fileName, Interval span, Interval seek)
        {
            _fileName = fileName;
            _span = span;
            _seek = seek;
        }

        public static CodeLocation Abstract
        {
            get
            {
                return _intrinsic;
            }
        }

        public static CodeLocation Intrinsic
        {
            get
            {
                return _intrinsic;
            }
        }

        public bool IsAbstract
        {
            get
            {
                return object.ReferenceEquals(this, _abstract);
            }
        }

        public bool IsIntrinsic
        {
            get
            {
                return object.ReferenceEquals(this, _intrinsic);
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
        }

        public Interval? Span
        {
            get
            {
                return _span;
            }
        }

        public Interval? Seek
        {
            get
            {
                return _seek;
            }
        }
    }
}
