namespace Tvl.VisualStudio.Language.Java.SourceData.Emit
{
    using System.Diagnostics.Contracts;
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public abstract class CodeTypeBuilder : CodeElementBuilder, ITypeBuilderContainer
    {
        public CodeTypeBuilder(CodeElementBuilder parent, string name, Interval span, Interval seek)
            : base(parent, name, span, seek)
        {
            Contract.Requires(parent != null);
            Contract.Requires(!string.IsNullOrEmpty(name));
        }

        public CodeFieldBuilder DefineField(string name, Interval span, Interval seek)
        {
            var builder = new CodeFieldBuilder(this, name, span, seek);
            Children.Add(builder);
            return builder;
        }

        public CodeMethodBuilder DefineMethod(string name, Interval span, Interval seek)
        {
            var builder = new CodeMethodBuilder(this, name, span, seek);
            Children.Add(builder);
            return builder;
        }

        public CodeClassBuilder DefineClass(string name, Interval span, Interval seek)
        {
            var builder = new CodeClassBuilder(this, name, span, seek);
            Children.Add(builder);
            return builder;
        }

        public CodeInterfaceBuilder DefineInterface(string name, Interval span, Interval seek)
        {
            var builder = new CodeInterfaceBuilder(this, name, span, seek);
            Children.Add(builder);
            return builder;
        }

        public CodeEnumBuilder DefineEnum(string name, Interval span, Interval seek)
        {
            var builder = new CodeEnumBuilder(this, name, span, seek);
            Children.Add(builder);
            return builder;
        }

        public CodeAttributeInterfaceBuilder DefineAnnotationInterface(string name, Interval span, Interval seek)
        {
            var builder = new CodeAttributeInterfaceBuilder(this, name, span, seek);
            Children.Add(builder);
            return builder;
        }

        protected override string NestedNameSeparator
        {
            get
            {
                return "+";
            }
        }
    }
}
