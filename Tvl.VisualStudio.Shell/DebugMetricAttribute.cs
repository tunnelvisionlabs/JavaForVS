namespace Tvl.VisualStudio.Shell
{
    using System;
    using System.Diagnostics.Contracts;

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DebugMetricAttribute : Attribute
    {
        public DebugMetricAttribute(string name)
            : this(null, name)
        {
        }

        public DebugMetricAttribute(string subkey, string name)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));

            if (!string.IsNullOrEmpty(subkey))
                SubKey = subkey;

            Name = name;
        }

        public string SubKey
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }
    }
}
