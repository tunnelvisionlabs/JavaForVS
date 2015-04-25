namespace Tvl.VisualStudio.Language.Java.SourceData
{
    using System;

    partial class NameResolutionContext
    {
        private class TypeContext : NameResolutionContext
        {
            private readonly CodeType _type;

            public TypeContext(IntelliSenseCache cache, CodeType type)
                : base(cache)
            {
                _type = type;
            }

            public override CodeElement[] GetMatchingElements()
            {
                return new CodeElement[] { _type };
            }

            public override NameResolutionContext Filter(string name, string @operator, bool caseSensitive)
            {
                throw new NotImplementedException();
            }
        }
    }
}
