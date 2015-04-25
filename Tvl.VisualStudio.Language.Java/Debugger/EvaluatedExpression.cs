namespace Tvl.VisualStudio.Language.Java.Debugger
{
    using System;
    using System.Diagnostics.Contracts;
    using Tvl.Java.DebugInterface;

    public class EvaluatedExpression
    {
        private readonly string _name;
        private readonly string _fullName;
        private readonly ILocalVariable _localVariable;
        private readonly IObjectReference _referencer;
        private readonly IField _field;
        private readonly IMethod _method;
        private readonly int? _index;
        private readonly IValue _value;
        private readonly IType _valueType;
        private readonly bool _strongReference;
        private readonly bool _hasSideEffects;

        public EvaluatedExpression(string name, string fullName, IValue value, bool hasSideEffects)
            : this(name, fullName, default(ILocalVariable), default(IObjectReference), default(IField), default(IMethod), default(int?), value, value != null ? value.GetValueType() : null, false, hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));
        }

        public EvaluatedExpression(string name, string fullName, IStrongValueHandle<IValue> value, bool hasSideEffects)
            : this(name, fullName, default(ILocalVariable), default(IObjectReference), default(IField), default(IMethod), default(int?), value != null ? value.Value : default(IValue), value != null ? value.Value.GetValueType() : default(IType), true, hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));
        }

        public EvaluatedExpression(string name, string fullName, IType valueType, IValue value, bool hasSideEffects)
            : this(name, fullName, default(ILocalVariable), default(IObjectReference), default(IField), default(IMethod), default(int?), value, valueType, false, hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));
        }

        public EvaluatedExpression(string name, string fullName, IType valueType, IStrongValueHandle<IValue> value, bool hasSideEffects)
            : this(name, fullName, default(ILocalVariable), default(IObjectReference), default(IField), default(IMethod), default(int?), value != null ? value.Value : null, valueType, true, hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));
        }

        public EvaluatedExpression(string name, string fullName, ILocalVariable variable, IValue value, bool hasSideEffects)
            : this(name, fullName, default(ILocalVariable), default(IObjectReference), default(IField), default(IMethod), default(int?), value, variable.GetLocalType(), false, hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));
        }

        public EvaluatedExpression(string name, string fullName, ILocalVariable variable, IStrongValueHandle<IValue> value, bool hasSideEffects)
            : this(name, fullName, variable, default(IObjectReference), default(IField), default(IMethod), default(int?), value != null ? value.Value : null, variable.GetLocalType(), true, hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));
        }

        public EvaluatedExpression(string name, string fullName, ILocalVariable variable, int index, IValue value, bool hasSideEffects)
            : this(name, fullName, variable, default(IObjectReference), default(IField), default(IMethod), index, value, variable.GetLocalType(), false, hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));
        }

        public EvaluatedExpression(string name, string fullName, ILocalVariable variable, int index, IStrongValueHandle<IValue> value, bool hasSideEffects)
            : this(name, fullName, variable, default(IObjectReference), default(IField), default(IMethod), index, value != null ? value.Value : null, variable.GetLocalType(), true, hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));
        }

        public EvaluatedExpression(string name, string fullName, IObjectReference referencer, IField field, IValue value, bool hasSideEffects)
            : this(name, fullName, default(ILocalVariable), referencer, field, default(IMethod), default(int?), value, field.GetFieldType(), false, hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentNullException>(field != null, "field");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));
        }

        public EvaluatedExpression(string name, string fullName, IObjectReference referencer, IField field, IStrongValueHandle<IValue> value, bool hasSideEffects)
            : this(name, fullName, default(ILocalVariable), referencer, field, default(IMethod), default(int?), value != null ? value.Value : null, field.GetFieldType(), true, hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentNullException>(value != null, "value");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));
        }

        public EvaluatedExpression(string name, string fullName, IObjectReference referencer, IField field, int index, IValue value, bool hasSideEffects)
            : this(name, fullName, default(ILocalVariable), referencer, field, default(IMethod), index, value, field.GetFieldType(), false, hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentNullException>(field != null, "field");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));
        }

        public EvaluatedExpression(string name, string fullName, IObjectReference referencer, IField field, int index, IStrongValueHandle<IValue> value, bool hasSideEffects)
            : this(name, fullName, default(ILocalVariable), referencer, field, default(IMethod), index, value != null ? value.Value : null, field.GetFieldType(), true, hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentNullException>(value != null, "value");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));
        }

        public EvaluatedExpression(string name, string fullName, IObjectReference referencer, IMethod method, bool hasSideEffects)
            : this(name, fullName, default(ILocalVariable), referencer, default(IField), method, default(int?), default(IValue), default(IType), false, hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentNullException>(method != null, "method");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));
        }

        public EvaluatedExpression(string name, string fullName, ILocalVariable localVariable, IObjectReference referencer, IField field, IMethod method, int? index, IValue value, IType valueType, bool strongReference, bool hasSideEffects)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(fullName != null, "fullName");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(name));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(fullName));

            _name = name;
            _fullName = fullName;
            _localVariable = localVariable;
            _referencer = referencer;
            _field = field;
            _method = method;
            _index = index;
            _value = value;
            _valueType = valueType;
            _strongReference = strongReference;
            _hasSideEffects = hasSideEffects;
        }

        public string Name
        {
            get
            {
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

        public ILocalVariable LocalVariable
        {
            get
            {
                return _localVariable;
            }
        }

        public IObjectReference Referencer
        {
            get
            {
                return _referencer;
            }
        }

        public IField Field
        {
            get
            {
                return _field;
            }
        }

        public IMethod Method
        {
            get
            {
                return _method;
            }
        }

        public int? Index
        {
            get
            {
                return _index;
            }
        }

        public IValue Value
        {
            get
            {
                return _value;
            }
        }

        /* The property could be a java.lang.Object property even if the value it hold is a java.lang.String
         * (or any other object type). _valueType is the type of this property and is assignable from
         * _value.GetValueType().
         */
        public IType ValueType
        {
            get
            {
                return _valueType;
            }
        }

        public bool StrongReference
        {
            get
            {
                return _strongReference;
            }
        }

        public bool HasSideEffects
        {
            get
            {
                return _hasSideEffects;
            }
        }
    }
}
