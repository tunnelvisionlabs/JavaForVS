namespace Tvl.Java.DebugInterface.Types.Loader
{
    using System;
    using System.Diagnostics.Contracts;

    public sealed class UnknownAttributeInfo : AttributeInfo
    {
        private string _name;

        public UnknownAttributeInfo(string name, ushort attributeNameIndex, byte[] info)
            : base(attributeNameIndex, info)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));

            _name = name;
        }

        public override string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
