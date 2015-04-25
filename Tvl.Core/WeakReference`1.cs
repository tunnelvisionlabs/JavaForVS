namespace Tvl
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class WeakReference<T> : WeakReference
        where T : class
    {
        private readonly int _hashCode;

        public WeakReference(T target)
            : base(target)
        {
            if (target != null)
                _hashCode = target.GetHashCode() ^ 0xCCCC;
        }

        public WeakReference(T target, bool trackResurrection)
            : base(target, trackResurrection)
        {
            if (target != null)
                _hashCode = target.GetHashCode() ^ 0xCCCC;
        }

        protected WeakReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets or sets the object (the target) referenced by the current WeakReference&lt;T&gt; object.
        /// </summary>
        /// <value>
        /// null if the object referenced by the current WeakReference&lt;T&gt; object has been garbage
        /// collected; otherwise, a reference to the object referenced by the current WeakReference&lt;T&gt;
        /// object.
        /// </value>
        /// <exception cref="System.InvalidOperationException">
        /// The reference to the target object is invalid. This exception can be thrown while setting
        /// this property if the value is a null reference or if the object has been finalized during
        /// the set operation.
        /// </exception>
        public virtual new T Target
        {
            get
            {
                return (T)base.Target;
            }

            set
            {
                base.Target = value;
            }
        }

        public override bool Equals(object obj)
        {
            WeakReference other = obj as WeakReference;
            if (object.ReferenceEquals(other, null))
                return false;

            if (object.ReferenceEquals(this, other))
                return true;

            object target = null;
            object otherTarget = null;
            try
            {
                target = this.Target;
                otherTarget = other.Target;
            }
            catch (InvalidOperationException)
            {
            }

            if (target == null || otherTarget == null)
                return false;

            return object.Equals(target, otherTarget);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
