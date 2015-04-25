namespace Tvl.VisualStudio.Language.Java.SourceData
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    public class CodeImportStatement : CodeElement
    {
        private bool _importOnDemand;
        private bool _staticImport;

        public CodeImportStatement(string name, string fullName, CodeLocation location, CodeElement parent)
            : base(name, fullName, location, parent)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(!string.IsNullOrEmpty(fullName));
            Contract.Requires(location != null);
            Contract.Requires(parent != null);
        }

        public bool ImportOnDemand
        {
            get
            {
                return _importOnDemand;
            }

            internal set
            {
                Contract.Requires<InvalidOperationException>(!IsFrozen);
                _importOnDemand = value;
            }
        }

        public bool StaticImport
        {
            get
            {
                return _staticImport;
            }

            internal set
            {
                Contract.Requires<InvalidOperationException>(!IsFrozen);
                _staticImport = value;
            }
        }

        public override void AugmentQuickInfoSession(IList<object> content)
        {
        }
    }
}
