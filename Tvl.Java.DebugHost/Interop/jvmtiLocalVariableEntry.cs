// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System;

    internal struct jvmtiLocalVariableEntry
    {
        public readonly jlocation StartLocation;
        public readonly int Length;
        public readonly IntPtr _name;
        public readonly IntPtr _signature;
        public readonly IntPtr _genericSignature;
        public readonly int Slot;

        public string Name
        {
            get
            {
                unsafe
                {
                    if (_name == IntPtr.Zero)
                        return null;

                    return ModifiedUTF8Encoding.GetString((byte*)_name);
                }
            }
        }

        public string Signature
        {
            get
            {
                unsafe
                {
                    if (_signature == IntPtr.Zero)
                        return null;

                    return ModifiedUTF8Encoding.GetString((byte*)_signature);
                }
            }
        }

        public string GenericSignature
        {
            get
            {
                unsafe
                {
                    if (_genericSignature == IntPtr.Zero)
                        return null;

                    return ModifiedUTF8Encoding.GetString((byte*)_genericSignature);
                }
            }
        }
    }
}
