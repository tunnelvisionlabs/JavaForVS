namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    internal sealed class TypeComponentNameAndSignatureEqualityComparer : IEqualityComparer<ITypeComponent>
    {
        private static readonly TypeComponentNameAndSignatureEqualityComparer _default = new TypeComponentNameAndSignatureEqualityComparer();

        public static TypeComponentNameAndSignatureEqualityComparer Default
        {
            get
            {
                return _default;
            }
        }

        #region IEqualityComparer<ITypeComponent> Members

        public bool Equals(ITypeComponent x, ITypeComponent y)
        {
            if (x == null)
                return y == null;

            if (y == null)
                return false;

            return StringComparer.Ordinal.Equals(GetFullSignature(x), GetFullSignature(y));
        }

        public int GetHashCode(ITypeComponent obj)
        {
            if (obj == null)
                return 0;

            return StringComparer.Ordinal.GetHashCode(GetFullSignature(obj));
        }

        #endregion

        private static string GetFullSignature(ITypeComponent y)
        {
            Contract.Requires<ArgumentNullException>(y != null, "y");
            return y.GetName() + " " + y.GetSignature();
        }
    }
}
