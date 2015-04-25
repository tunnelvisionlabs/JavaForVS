namespace Tvl.VisualStudio.Language.Java.SourceData.Emit
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public class CodeFileBuilder : CodeElementBuilder, ITypeBuilderContainer
    {
        public CodeFileBuilder(string fileName)
            : base(fileName)
        {
            Contract.Requires<ArgumentNullException>(fileName != null, "fileName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fileName));
        }

        // adds a "package x...;" statement
        public CodePackageStatementBuilder DefinePackageStatement(string name, Interval span, Interval seek)
        {
            var builder = new CodePackageStatementBuilder(this, name, span, seek);
            Children.Add(builder);
            return builder;
        }

        // adds an "import ..." statement
        public CodeImportBuilder DefineImport(string name, Interval span, Interval seek)
        {
            var builder = new CodeImportBuilder(this, name, span, seek);
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

        internal override CodeElement BuildElement(CodeElement parent)
        {
            CodePhysicalFile file = new CodePhysicalFile(this.Name.ToLowerInvariant());
            foreach (var child in Children)
            {
                var childElement = child.BuildElement(file);
                file.AddChild(childElement);
            }

            return file;
        }

        protected override CodeElement CreateCodeElement(string name, string fullName, CodeLocation location, CodeElement parent)
        {
            throw new NotSupportedException();
        }
    }
}
