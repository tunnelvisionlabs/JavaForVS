namespace Tvl.VisualStudio.Language.Java.SourceData.Emit
{
    using Tvl.VisualStudio.Language.Parsing.Collections;

    public interface ITypeBuilderContainer
    {
        CodeClassBuilder DefineClass(string name, Interval span, Interval seek);

        CodeInterfaceBuilder DefineInterface(string name, Interval span, Interval seek);

        CodeEnumBuilder DefineEnum(string name, Interval span, Interval seek);

        CodeAttributeInterfaceBuilder DefineAnnotationInterface(string name, Interval span, Interval seek);
    }
}
