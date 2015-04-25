namespace Tvl.VisualStudio.Language.Java.SourceData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    partial class NameResolutionContext
    {
        private class GlobalContext : NameResolutionContext
        {
            public GlobalContext(IntelliSenseCache cache)
                : base(cache)
            {
            }

            public override CodeElement[] GetMatchingElements()
            {
                // just going to be the top-level package names plus all types in java.lang
                CodeElement[] packages = _cache.GetPackages();
                CodePackage[] javaLangPackage = _cache.ResolvePackage("java.lang", true);
                CodeType[] globalTypes = javaLangPackage.SelectMany(i => i.Children).OfType<CodeType>().ToArray();

                CodeElement[] result = new CodeElement[packages.Length + globalTypes.Length];
                Array.Copy(packages, result, packages.Length);
                Array.Copy(globalTypes, 0, result, packages.Length, globalTypes.Length);

                return result;
            }

#if false // valid?*/
            @"
#endif

            public override NameResolutionContext Filter(string name, string @operator, bool caseSensitive)
            {
                if (@operator != null)
                    return NameResolutionContext.Error(this, name, @operator, caseSensitive);

                StringComparer comparer = caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
                IEnumerable<CodeElement> elements = GetMatchingElements().Where(i => comparer.Equals(i.Name, name));
                return NameResolutionContext.Aggregate(_cache, elements.Select(
                    element =>
                    {
                        CodeType type = element as CodeType;
                        if (type != null)
                            return NameResolutionContext.Type(_cache, type);

                        return NameResolutionContext.Package(_cache, (CodePackage)element);
                    }));
            }
        }
    }
}
