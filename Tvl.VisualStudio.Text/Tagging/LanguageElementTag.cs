namespace Tvl.VisualStudio.Text.Tagging
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Windows.Media;
    using Microsoft.VisualStudio.Text;

    public class LanguageElementTag : ILanguageElementTag
    {
        public LanguageElementTag(string name, string category, ImageSource glyph, SnapshotSpan target)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(category != null, "category");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(category));

            this.Name = name;
            this.Category = category;
            this.Glyph = glyph;
            this.Target = target;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Category
        {
            get;
            private set;
        }

        public ImageSource Glyph
        {
            get;
            private set;
        }

        public SnapshotSpan Target
        {
            get;
            private set;
        }
    }
}
