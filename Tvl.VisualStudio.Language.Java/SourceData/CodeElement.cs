namespace Tvl.VisualStudio.Language.Java.SourceData
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public abstract class CodeElement
    {
        private static readonly CodeElement _intrinsic = new IntrinsicElement();

        private readonly string _name;
        private readonly string _fullName;
        private readonly CodeLocation _location;
        private readonly CodeElement _parent;
        private readonly List<CodeElement> _children;

        private bool _isFrozen;

        internal CodeElement(string name, string fullName, CodeLocation location, CodeElement parent)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(!string.IsNullOrEmpty(fullName));
            Contract.Requires(location != null);

            _name = name;
            _fullName = fullName;
            _location = location;
            _parent = parent;
            _children = new List<CodeElement>();
        }

        public static CodeElement Intrinsic
        {
            get
            {
                return _intrinsic;
            }
        }

        public bool IsIntrinsic
        {
            get
            {
                return object.ReferenceEquals(this, _intrinsic);
            }
        }

        public string Name
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

                return _name;
            }
        }

        public string FullName
        {
            get
            {
                return _fullName;
            }
        }

        public CodeLocation Location
        {
            get
            {
                Contract.Ensures(Contract.Result<CodeLocation>() != null);

                return _location;
            }
        }

        public CodeElement Parent
        {
            get
            {
                return _parent;
            }
        }

        public virtual IEnumerable<CodeElement> Children
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<CodeElement>>() != null);

                return _children.AsReadOnly();
            }
        }

        internal bool IsFrozen
        {
            get
            {
                return _isFrozen;
            }
        }

        public abstract void AugmentQuickInfoSession(IList<object> content);

        public IEnumerable<CodeElement> GetDescendents(bool includeSelf = false)
        {
            var descendents = Children.SelectMany(i => i.GetDescendents(true));
            if (includeSelf)
                descendents = Enumerable.Repeat(this, 1).Concat(descendents);

            return descendents;
        }

        internal virtual void AddChild(CodeElement child)
        {
            Contract.Requires<InvalidOperationException>(!IsFrozen);
            Contract.Requires(child != null);

            _children.Add(child);
        }

        internal void Freeze()
        {
            lock (_children)
            {
                foreach (var child in Children)
                    child.Freeze();

                _isFrozen = true;
            }
        }

        public override string ToString()
        {
            return FullName;
        }

        private class IntrinsicElement : CodeElement
        {
            public IntrinsicElement()
                : base("__INTRINSIC__", "__INTRINSIC__", CodeLocation.Intrinsic, null)
            {
            }

            public override void AugmentQuickInfoSession(IList<object> content)
            {
            }
        }
    }
}
