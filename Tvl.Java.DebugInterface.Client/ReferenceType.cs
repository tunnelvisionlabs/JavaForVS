namespace Tvl.Java.DebugInterface.Client
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Tvl.Java.DebugInterface.Types;
    using AccessModifiers = Tvl.Java.DebugInterface.AccessModifiers;
    using File = System.IO.File;
    using Path = System.IO.Path;

    internal abstract class ReferenceType : JavaType, IReferenceType
    {
        private readonly TaggedReferenceTypeId _taggedTypeId;

        // cached information
        private string _signature;
        private string _genericSignature;
        private AccessModifiers? _modifiers;
        private string _sourceName;
        private string _sourceDebugExtension;

        private ConstantPoolEntry[] _constantPool;
        private Field[] _fields;
        private Field[] _allFields;
        private Field[] _visibleFields;
        private Location[] _lineLocations;
        private Method[] _methods;
        private Method[] _allMethods;
        private Method[] _visibleMethods;
        private Version _version;

        private ClassLoaderReference _classLoader;

        protected ReferenceType(VirtualMachine virtualMachine, TaggedReferenceTypeId taggedTypeId)
            : base(virtualMachine)
        {
            Contract.Requires(virtualMachine != null);
            _taggedTypeId = taggedTypeId;
        }

        public ReferenceTypeId ReferenceTypeId
        {
            get
            {
                return _taggedTypeId.TypeId;
            }
        }

        public TaggedReferenceTypeId TaggedReferenceTypeId
        {
            get
            {
                return _taggedTypeId;
            }
        }

        public sealed override string GetSignature()
        {
            if (_signature == null)
            {
                string signature;
                string genericSignature;
                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetSignature(out signature, out genericSignature, ReferenceTypeId));
                _signature = signature;
            }

            return _signature;
        }

        #region IReferenceType Members

        public ReadOnlyCollection<IField> GetFields(bool includeInherited)
        {
            if (includeInherited && _allFields == null)
            {
                List<Field> allFields = new List<Field>(GetFields(false).Cast<Field>());

                HashSet<ReferenceType> inheritedTypes = new HashSet<ReferenceType>();
                GetInheritedTypes(this, inheritedTypes);
                allFields.AddRange(inheritedTypes.SelectMany(type => type.GetFields(false)).Cast<Field>());

                _allFields = allFields.ToArray();
            }
            else if (!includeInherited && _fields == null)
            {
                DeclaredFieldData[] fieldData;
                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetFields(out fieldData, ReferenceTypeId));
                Field[] fields = Array.ConvertAll(fieldData, field => VirtualMachine.GetMirrorOf(this, field));
                _fields = fields;
            }

            return new ReadOnlyCollection<IField>(includeInherited ? _allFields : _fields);
        }

        public ReadOnlyCollection<ILocation> GetLineLocations()
        {
            string stratum = GetDefaultStratum();
            IEnumerable<string> sourceNames = GetSourceNames(stratum);
            ILocation[] locations = sourceNames.SelectMany(i => GetLineLocations(stratum, i)).ToArray();
            return new ReadOnlyCollection<ILocation>(locations);
        }

        public ReadOnlyCollection<ILocation> GetLineLocations(string stratum, string sourceName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IMethod> GetMethods(bool includeInherited)
        {
            if (includeInherited && _allMethods == null)
            {
                List<Method> allMethods = new List<Method>(GetMethods(false).Cast<Method>());

                HashSet<ReferenceType> inheritedTypes = new HashSet<ReferenceType>();
                GetInheritedTypes(this, inheritedTypes);
                allMethods.AddRange(inheritedTypes.SelectMany(type => type.GetMethods(false)).Cast<Method>());

                _allMethods = allMethods.ToArray();
            }
            else if (!includeInherited && _methods == null)
            {
                DeclaredMethodData[] methodData;
                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetMethods(out methodData, ReferenceTypeId));
                Method[] methods = Array.ConvertAll(methodData, method => VirtualMachine.GetMirrorOf(this, method));
                _methods = methods;
            }

            return new ReadOnlyCollection<IMethod>(includeInherited ? _allMethods : _methods);
        }

        protected static void GetInheritedTypes(ReferenceType type, HashSet<ReferenceType> inheritedTypes)
        {
            List<ReferenceType> immediateBases = new List<ReferenceType>();

            ClassType classtype = type as ClassType;
            if (classtype != null)
            {
                ClassType basetype = (ClassType)classtype.GetSuperclass();
                if (basetype != null)
                    immediateBases.Add(basetype);

                immediateBases.AddRange(classtype.GetInterfaces(false).Cast<ReferenceType>());
            }

            InterfaceType interfacetype = type as InterfaceType;
            if (interfacetype != null)
            {
                immediateBases.AddRange(interfacetype.GetSuperInterfaces().Cast<ReferenceType>());
            }

            foreach (var baseType in immediateBases)
            {
                if (inheritedTypes.Add(baseType))
                    GetInheritedTypes(baseType, inheritedTypes);
            }
        }

        public ReadOnlyCollection<string> GetAvailableStrata()
        {
            return new ReadOnlyCollection<string>(new[] { "Java" });
        }

        public IClassLoaderReference GetClassLoader()
        {
            if (_classLoader == null)
            {
                ClassLoaderId classLoader;
                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetClassLoader(out classLoader, ReferenceTypeId));
                _classLoader = VirtualMachine.GetMirrorOf(classLoader);
            }

            return _classLoader;
        }

        public IClassObjectReference GetClassObject()
        {
            ClassObjectId classObject;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetClassObject(out classObject, ReferenceTypeId));
            return VirtualMachine.GetMirrorOf(classObject);
        }

        public ReadOnlyCollection<ConstantPoolEntry> GetConstantPool()
        {
            if (_constantPool == null)
            {
                int constantPoolCount;
                byte[] data;
                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetConstantPool(out constantPoolCount, out data, this.ReferenceTypeId));

                List<ConstantPoolEntry> entryList = new List<ConstantPoolEntry>();
                int currentPosition = 0;
                for (int i = 0; i < constantPoolCount - 1; i++)
                {
                    entryList.Add(ConstantPoolEntry.FromBytes(data, ref currentPosition));
                    switch (entryList.Last().Type)
                    {
                    case ConstantType.Double:
                    case ConstantType.Long:
                        // these entries take 2 slots
                        entryList.Add(ConstantPoolEntry.Reserved);
                        i++;
                        break;

                    default:
                        break;
                    }
                }

                _constantPool = entryList.ToArray();
            }

            return new ReadOnlyCollection<ConstantPoolEntry>(_constantPool);
        }

        public int GetConstantPoolCount()
        {
            return GetConstantPool().Count;
        }

        public string GetDefaultStratum()
        {
            return "Java";
        }

        public bool GetFailedToInitialize()
        {
            ClassStatus status;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetReferenceTypeStatus(out status, ReferenceTypeId));
            return (status & ClassStatus.Error) != 0;
        }

        public IField GetFieldByName(string fieldName)
        {
            return GetVisibleFields().SingleOrDefault(i => i.GetName() == fieldName);
        }

        public string GetGenericSignature()
        {
            if (_genericSignature == null)
            {
                string signature;
                string genericSignature;
                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetSignature(out signature, out genericSignature, ReferenceTypeId));
                _genericSignature = genericSignature;
            }

            return _genericSignature;
        }

        public IValue GetValue(IField field)
        {
            Field localField = field as Field;
            if (localField == null)
                throw new VirtualMachineMismatchException();

            Types.Value[] values;
            FieldId[] fields = { localField.FieldId };
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetReferenceTypeValues(out values, ReferenceTypeId, fields));
            return VirtualMachine.GetMirrorOf(values.Single());
        }

        public IDictionary<IField, IValue> GetValues(IEnumerable<IField> fields)
        {
            IField[] fieldsArray = fields.ToArray();

            FieldId[] fieldIds = new FieldId[fieldsArray.Length];
            // verify each field comes from this VM
            for (int i = 0; i < fieldsArray.Length; i++)
            {
                Field field = fieldsArray[i] as Field;
                if (field == null)
                    throw new VirtualMachineMismatchException();

                fieldIds[i] = field.FieldId;
            }

            Types.Value[] values;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetReferenceTypeValues(out values, ReferenceTypeId, fieldIds));

            Dictionary<IField, IValue> result = new Dictionary<IField, IValue>();
            for (int i = 0; i < fieldIds.Length; i++)
                result[fieldsArray[i]] = VirtualMachine.GetMirrorOf(values[i]);

            return result;
        }

        public ReadOnlyCollection<IObjectReference> GetInstances(long maxInstances)
        {
            throw new NotImplementedException();
        }

        public bool GetIsAbstract()
        {
            return (GetModifiers() & AccessModifiers.Abstract) != 0;
        }

        public bool GetIsFinal()
        {
            return (GetModifiers() & AccessModifiers.Final) != 0;
        }

        public bool GetIsInitialized()
        {
            ClassStatus status;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetReferenceTypeStatus(out status, ReferenceTypeId));
            return (status & ClassStatus.Initialized) != 0;
        }

        public bool GetIsPrepared()
        {
            ClassStatus status;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetReferenceTypeStatus(out status, ReferenceTypeId));
            return (status & ClassStatus.Prepared) != 0;
        }

        public bool GetIsStatic()
        {
            return (GetModifiers() & AccessModifiers.Static) != 0;
        }

        public bool GetIsVerified()
        {
            ClassStatus status;
            DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetReferenceTypeStatus(out status, ReferenceTypeId));
            return (status & ClassStatus.Verified) != 0;
        }

        public ReadOnlyCollection<ILocation> GetLocationsOfLine(int lineNumber)
        {
            string stratum = GetDefaultStratum();
            ReadOnlyCollection<string> paths = GetSourcePaths(stratum);
            return paths.SelectMany(i => GetLocationsOfLine(stratum, i, lineNumber)).ToList().AsReadOnly();
        }

        public ReadOnlyCollection<ILocation> GetLocationsOfLine(string stratum, string sourceName, int lineNumber)
        {
            if (stratum != "Java")
                return new ReadOnlyCollection<ILocation>(new ILocation[0]);

            List<ILocation> locations = new List<ILocation>();
            foreach (var method in GetMethods(false))
            {
                if (method.GetIsNative())
                    continue;

                if (method.GetIsAbstract())
                    continue;

                locations.AddRange(method.GetLocationsOfLine(stratum, sourceName, lineNumber));
            }

            return locations.AsReadOnly();
        }

        public int GetMajorVersion()
        {
            return GetVersion().Major;
        }

        public int GetMinorVersion()
        {
            return GetVersion().Minor;
        }

        private Version GetVersion()
        {
            if (_version == null)
            {
                int majorVersion;
                int minorVersion;
                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetClassFileVersion(out majorVersion, out minorVersion, this.ReferenceTypeId));
                _version = new Version(majorVersion, minorVersion);
            }

            return _version;
        }

        public ReadOnlyCollection<IMethod> GetMethodsByName(string name)
        {
            List<IMethod> methods = new List<IMethod>(GetVisibleMethods().Where(i => i.GetName() == name));
            return methods.AsReadOnly();
        }

        public ReadOnlyCollection<IMethod> GetMethodsByName(string name, string signature)
        {
            List<IMethod> methods = new List<IMethod>(GetVisibleMethods().Where(i => i.GetName() == name && i.GetSignature() == signature));
            return methods.AsReadOnly();
        }

        public ReadOnlyCollection<IReferenceType> GetNestedTypes()
        {
            throw new NotImplementedException();
        }

        public string GetSourceDebugExtension()
        {
            if (_sourceDebugExtension == null)
            {
                string sourceDebugExtension;
                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetSourceDebugExtension(out sourceDebugExtension, ReferenceTypeId));
                _sourceDebugExtension = sourceDebugExtension;
            }

            return _sourceDebugExtension;
        }

        public string GetSourceName()
        {
            if (_sourceName == null)
            {
                string sourceFile;
                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetSourceFile(out sourceFile, ReferenceTypeId));
                _sourceName = sourceFile;
            }

            return _sourceName;
        }

        public ReadOnlyCollection<string> GetSourceNames(string stratum)
        {
            if (stratum != "Java")
                return new ReadOnlyCollection<string>(new string[0]);

            return new ReadOnlyCollection<string>(new[] { GetSourceName() });
        }

        public ReadOnlyCollection<string> GetSourcePaths(string stratum)
        {
            if (stratum != "Java")
                return new ReadOnlyCollection<string>(new string[0]);

            if (_taggedTypeId.TypeTag == TypeTag.Class || _taggedTypeId.TypeTag == TypeTag.Interface)
            {
                string signature = GetSignature().Substring(1);
                string relativeFolder = null;
                if (signature.IndexOf('/') >= 0)
                    relativeFolder = signature.Substring(0, signature.LastIndexOf('/')).Replace('/', Path.DirectorySeparatorChar);

                List<string> paths = new List<string>(GetSourceNames(stratum).Select(i => string.IsNullOrEmpty(relativeFolder) ? i : Path.Combine(relativeFolder, i)));
                for (int i = paths.Count - 1; i >= 0; i--)
                {
                    string path = paths[i];
                    List<string> qualifiedPaths = VirtualMachine.SourcePaths.Select(j => Path.Combine(j, path)).Where(File.Exists).ToList();
                    if (qualifiedPaths.Count > 0)
                    {
                        paths.RemoveAt(i);
                        paths.InsertRange(i, qualifiedPaths);
                    }
                }

                return paths.AsReadOnly();
            }
            else
            {
                // TODO: get actual source paths
                return GetSourceNames(stratum);
            }
        }

        public ReadOnlyCollection<IField> GetVisibleFields()
        {
            if (_visibleFields == null)
            {
                List<Field> visibleFields = new List<Field>(GetFields(false).Cast<Field>());
                List<Field> inheritedVisibleFields = new List<Field>();

                List<ReferenceType> immediateBases = GetImmediateBaseTypes();
                foreach (var baseType in immediateBases)
                {
                    inheritedVisibleFields.AddRange(baseType.GetVisibleFields().Cast<Field>());
                }

                inheritedVisibleFields.RemoveAll(i => i.GetIsPrivate());
                inheritedVisibleFields.RemoveAll(i => visibleFields.Any(j => i.GetName() == j.GetName()));
                // handle inheriting multiple ambiguous fields
                visibleFields.AddRange(inheritedVisibleFields.GroupBy(i => i.GetName()).SelectMany(i => i.Count() == 1 ? i : Enumerable.Empty<Field>()));
                _visibleFields = visibleFields.ToArray();
            }

            return new ReadOnlyCollection<IField>(_visibleFields);

            //if (_visibleFields == null)
            //{
            //    HashSet<Field> visibleFields = new HashSet<Field>(TypeComponentNameAndSignatureEqualityComparer.Default);

            //    ClassType classType = this as ClassType;
            //    if (classType == null)
            //        throw new NotImplementedException();

            //    for (/*classType = this*/; classType != null; classType = (ClassType)classType.GetSuperclass())
            //    {
            //        visibleFields.UnionWith(classType.GetFields(false).Cast<Field>());
            //    }

            //    _visibleFields = visibleFields.ToArray();
            //}

            //return new ReadOnlyCollection<IField>(_visibleFields);
        }

        public ReadOnlyCollection<IMethod> GetVisibleMethods()
        {
            if (_visibleMethods == null)
            {
                List<Method> visibleMethods = new List<Method>(GetMethods(false).Cast<Method>());
                List<Method> inheritedVisibleMethods = new List<Method>();

                List<ReferenceType> immediateBases = GetImmediateBaseTypes();
                foreach (var baseType in immediateBases)
                {
                    inheritedVisibleMethods.AddRange(baseType.GetVisibleMethods().Cast<Method>());
                }

                inheritedVisibleMethods.RemoveAll(i => i.GetIsPrivate());
                inheritedVisibleMethods.RemoveAll(i => visibleMethods.Any(j => i.GetName() == j.GetName() && i.GetSignature() == j.GetSignature() && i.GetIsStatic() == j.GetIsStatic()));
                visibleMethods.AddRange(inheritedVisibleMethods);
                _visibleMethods = visibleMethods.ToArray();
            }

            return new ReadOnlyCollection<IMethod>(_visibleMethods);

            //if (_visibleMethods == null)
            //{
            //    HashSet<Method> visibleMethods = new HashSet<Method>(TypeComponentNameAndSignatureEqualityComparer.Default);

            //    ClassType classType = this as ClassType;
            //    if (classType == null)
            //        throw new NotImplementedException();

            //    for (/*classType = this*/; classType != null; classType = (ClassType)classType.GetSuperclass())
            //    {
            //        visibleMethods.UnionWith(classType.GetMethods(false).Cast<Method>());
            //    }

            //    _visibleMethods = visibleMethods.ToArray();
            //}

            //return new ReadOnlyCollection<IMethod>(_visibleMethods);
        }

        #endregion

        #region IAccessible Members

        public bool GetIsPackagePrivate()
        {
            return (GetModifiers() & (AccessModifiers.Private | AccessModifiers.Protected | AccessModifiers.Public)) == 0;
        }

        public bool GetIsPrivate()
        {
            return (GetModifiers() & AccessModifiers.Private) != 0;
        }

        public bool GetIsProtected()
        {
            return (GetModifiers() & AccessModifiers.Protected) != 0;
        }

        public bool GetIsPublic()
        {
            return (GetModifiers() & AccessModifiers.Public) != 0;
        }

        public AccessModifiers GetModifiers()
        {
            if (_modifiers == null)
            {
                Types.AccessModifiers modifiers;
                DebugErrorHandler.ThrowOnFailure(VirtualMachine.ProtocolService.GetModifiers(out modifiers, ReferenceTypeId));
                _modifiers = (AccessModifiers)modifiers;
            }

            return _modifiers.Value;
        }

        #endregion

        #region IEquatable<IReferenceType> Members

        public bool Equals(IReferenceType other)
        {
            ReferenceType otherType = other as ReferenceType;
            if (otherType == null)
                return false;

            return this.VirtualMachine.Equals(otherType.VirtualMachine)
                && this.ReferenceTypeId == otherType.ReferenceTypeId;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ReferenceType);
        }

        public override int GetHashCode()
        {
            return VirtualMachine.GetHashCode() ^ ReferenceTypeId.GetHashCode();
        }

        #endregion

        private List<ReferenceType> GetImmediateBaseTypes()
        {
            List<ReferenceType> immediateBases = new List<ReferenceType>();

            ClassType classtype = this as ClassType;
            if (classtype != null)
            {
                ClassType basetype = (ClassType)classtype.GetSuperclass();
                if (basetype != null)
                    immediateBases.Add(basetype);

                immediateBases.AddRange(classtype.GetInterfaces(false).Cast<ReferenceType>());
            }

            InterfaceType interfacetype = this as InterfaceType;
            if (interfacetype != null)
            {
                immediateBases.AddRange(interfacetype.GetSuperInterfaces().Cast<ReferenceType>());
            }

            return immediateBases;
        }
    }
}
