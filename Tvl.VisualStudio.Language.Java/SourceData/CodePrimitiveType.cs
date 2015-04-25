namespace Tvl.VisualStudio.Language.Java.SourceData
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class CodePrimitiveType : CodeType
    {
        private static readonly CodePrimitiveType _boolean = new CodePrimitiveType("boolean");
        private static readonly CodePrimitiveType _char = new CodePrimitiveType("char");
        private static readonly CodePrimitiveType _byte = new CodePrimitiveType("byte");
        private static readonly CodePrimitiveType _short = new CodePrimitiveType("short");
        private static readonly CodePrimitiveType _int = new CodePrimitiveType("int");
        private static readonly CodePrimitiveType _long = new CodePrimitiveType("long");
        private static readonly CodePrimitiveType _float = new CodePrimitiveType("float");
        private static readonly CodePrimitiveType _double = new CodePrimitiveType("double");
        private static readonly CodePrimitiveType _void = new CodePrimitiveType("void");

        private CodePrimitiveType(string name)
            : base(name, name, CodeLocation.Intrinsic, CodeElement.Intrinsic)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
        }

        public static CodePrimitiveType Boolean
        {
            get
            {
                return CodePrimitiveType._boolean;
            }
        }

        public static CodePrimitiveType Char
        {
            get
            {
                return CodePrimitiveType._char;
            }
        }

        public static CodePrimitiveType Byte
        {
            get
            {
                return CodePrimitiveType._byte;
            }
        }

        public static CodePrimitiveType Short
        {
            get
            {
                return CodePrimitiveType._short;
            }
        }

        public static CodePrimitiveType Int
        {
            get
            {
                return CodePrimitiveType._int;
            }
        }

        public static CodePrimitiveType Long
        {
            get
            {
                return CodePrimitiveType._long;
            }
        }

        public static CodePrimitiveType Float
        {
            get
            {
                return CodePrimitiveType._float;
            }
        }

        public static CodePrimitiveType Double
        {
            get
            {
                return CodePrimitiveType._double;
            }
        }

        public static CodePrimitiveType Void
        {
            get
            {
                return CodePrimitiveType._void;
            }
        }

        public override void AugmentQuickInfoSession(IList<object> content)
        {
            content.Add("(primitive) " + Name);
        }
    }
}
