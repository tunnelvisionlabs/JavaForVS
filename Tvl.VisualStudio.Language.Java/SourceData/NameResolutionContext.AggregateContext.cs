namespace Tvl.VisualStudio.Language.Java.SourceData
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    partial class NameResolutionContext
    {
        private class AggregateContext : NameResolutionContext
        {
            private readonly NameResolutionContext[] _contexts;

            public AggregateContext(IntelliSenseCache cache, IEnumerable<NameResolutionContext> contexts)
                : base(cache)
            {
                Contract.Requires(cache != null);
                Contract.Requires(contexts != null);

                _contexts = contexts.ToArray();
            }

            public override CodeElement[] GetMatchingElements()
            {
                return _contexts.SelectMany(i => i.GetMatchingElements()).ToArray();
            }

            public override NameResolutionContext Filter(string name, string @operator, bool caseSensitive)
            {
                return NameResolutionContext.Aggregate(_cache, _contexts.Select(i => i.Filter(name, @operator, caseSensitive)));
            }
        }
    }
}
