namespace Tvl.VisualStudio.Language.Java.SourceData.Emit
{
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public class CodeMethodBuilder : CodeMemberBuilder
    {
        private CodeTypeReferenceBuilder _returnType;

        public CodeMethodBuilder(CodeElementBuilder parent, string name, Interval span, Interval seek)
            : base(parent, name, span, seek)
        {
        }

        public CodeParameterBuilder DefineParameter(string name, Interval span, Interval seek)
        {
            var builder = new CodeParameterBuilder(this, name, span, seek);
            Children.Add(builder);
            return builder;
        }

        public void SetReturnType(CodeTypeReferenceBuilder returnType)
        {
            _returnType = returnType;
        }

        protected override CodeElement CreateCodeElement(string name, string fullName, CodeLocation location, CodeElement parent)
        {
            CodeMethod method = new CodeMethod(name, fullName, location, parent);
            return method;
        }
    }
}
