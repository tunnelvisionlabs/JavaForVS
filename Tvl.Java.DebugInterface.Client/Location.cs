namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;

    internal sealed class Location : Mirror, ILocation
    {
        private readonly Method _method;
        private readonly long _codeIndex;

        private int? _lineNumber;

        internal Location(VirtualMachine virtualMachine, Method method, long codeIndex)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
            Contract.Requires<ArgumentNullException>(method != null, "method");

            _method = method;
            _codeIndex = codeIndex;
        }

        internal Location(VirtualMachine virtualMachine, Method method, long codeIndex, int lineNumber)
            : this(virtualMachine, method, codeIndex)
        {
            Contract.Requires(virtualMachine != null);
            Contract.Requires(method != null);

            _lineNumber = lineNumber;
        }

        public long CodeIndex
        {
            get
            {
                return _codeIndex;
            }
        }

        public Method Method
        {
            get
            {
                return _method;
            }
        }

        public long GetCodeIndex()
        {
            return _codeIndex;
        }

        public IReferenceType GetDeclaringType()
        {
            return _method.DeclaringType;
        }

        public int GetLineNumber()
        {
            return GetLineNumber(Method.DeclaringType.GetDefaultStratum());
        }

        public int GetLineNumber(string stratum)
        {
            if (stratum != "Java")
                throw new MissingInformationException();

            if (_lineNumber == null)
            {
                // locations returned by _method.GetLineLocations() have their line number cached.
                ReadOnlyCollection<ILocation> locations = _method.GetLineLocations();
                ILocation location = locations.Last(i => i.GetCodeIndex() <= _codeIndex);
                _lineNumber = location.GetLineNumber();
            }

            return _lineNumber.Value;
        }

        public IMethod GetMethod()
        {
            return _method;
        }

        public string GetSourceName()
        {
            return _method.DeclaringType.GetSourceName();
        }

        public string GetSourceName(string stratum)
        {
            return _method.DeclaringType.GetSourceNames(stratum).Single();
        }

        public string GetSourcePath()
        {
            return GetSourcePath(_method.DeclaringType.GetDefaultStratum());
        }

        public string GetSourcePath(string stratum)
        {
            return _method.DeclaringType.GetSourcePaths(stratum).Single();
        }

        public int CompareTo(ILocation other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ILocation other)
        {
            Location location = other as Location;
            if (location == null)
                return false;

            return this.VirtualMachine.Equals(location.VirtualMachine)
                && this.Method.Equals(location.Method)
                && this.CodeIndex == location.CodeIndex;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Location);
        }

        public override int GetHashCode()
        {
            return this.VirtualMachine.GetHashCode() ^ this.Method.GetHashCode() ^ this.CodeIndex.GetHashCode();
        }
    }
}
