namespace Tvl.Interop
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    [CLSCompliant(false), StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct Variant
    {

        /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType"]/*' />
        public enum VariantType
        {
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_EMPTY"]/*' />
            VT_EMPTY = 0,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_NULL"]/*' />
            VT_NULL = 1,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_I2"]/*' />
            VT_I2 = 2,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_I4"]/*' />
            VT_I4 = 3,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_R4"]/*' />
            VT_R4 = 4,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_R8"]/*' />
            VT_R8 = 5,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_CY"]/*' />
            VT_CY = 6,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_DATE"]/*' />
            VT_DATE = 7,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_BSTR"]/*' />
            VT_BSTR = 8,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_DISPATCH"]/*' />
            VT_DISPATCH = 9,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_ERROR"]/*' />
            VT_ERROR = 10,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_BOOL"]/*' />
            VT_BOOL = 11,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_VARIANT"]/*' />
            VT_VARIANT = 12,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_UNKNOWN"]/*' />
            VT_UNKNOWN = 13,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_DECIMAL"]/*' />
            VT_DECIMAL = 14,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_I1"]/*' />
            VT_I1 = 16,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_UI1"]/*' />
            VT_UI1 = 17,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_UI2"]/*' />
            VT_UI2 = 18,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_UI4"]/*' />
            VT_UI4 = 19,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_I8"]/*' />
            VT_I8 = 20,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_UI8"]/*' />
            VT_UI8 = 21,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_INT"]/*' />
            VT_INT = 22,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_UINT"]/*' />
            VT_UINT = 23,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_VOID"]/*' />
            VT_VOID = 24,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_HRESULT"]/*' />
            VT_HRESULT = 25,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_PTR"]/*' />
            VT_PTR = 26,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_SAFEARRAY"]/*' />
            VT_SAFEARRAY = 27,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_CARRAY"]/*' />
            VT_CARRAY = 28,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_USERDEFINED"]/*' />
            VT_USERDEFINED = 29,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_LPSTR"]/*' />
            VT_LPSTR = 30,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_LPWSTR"]/*' />
            VT_LPWSTR = 31,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_FILETIME"]/*' />
            VT_FILETIME = 64,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_BLOB"]/*' />
            VT_BLOB = 65,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_STREAM"]/*' />
            VT_STREAM = 66,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_STORAGE"]/*' />
            VT_STORAGE = 67,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_STREAMED_OBJECT"]/*' />
            VT_STREAMED_OBJECT = 68,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_STORED_OBJECT"]/*' />
            VT_STORED_OBJECT = 69,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_BLOB_OBJECT"]/*' />
            VT_BLOB_OBJECT = 70,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_CF"]/*' />
            VT_CF = 71,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_CLSID"]/*' />
            VT_CLSID = 72,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_VECTOR"]/*' />
            VT_VECTOR = 0x1000,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_ARRAY"]/*' />
            VT_ARRAY = 0x2000,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_BYREF"]/*' />
            VT_BYREF = 0x4000,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_RESERVED"]/*' />
            VT_RESERVED = 0x8000,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_ILLEGAL"]/*' />
            VT_ILLEGAL = 0xffff,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_ILLEGALMASKED"]/*' />
            VT_ILLEGALMASKED = 0xfff,
            /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.VariantType.VT_TYPEMASK"]/*' />
            VT_TYPEMASK = 0xfff
        };

        private ushort vt;

        /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.Vt"]/*' />
        public VariantType Vt
        {
            get
            {
                return (VariantType)vt;
            }
            set
            {
                vt = (ushort)value;
            }
        }
        short reserved1;
        short reserved2;
        short reserved3;

        private long value;

        /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.Value"]/*' />
        public long Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.ToVariant"]/*' />
        public static Variant ToVariant(IntPtr ptr)
        {
            // Marshal.GetObjectForNativeVariant is doing way too much work.
            // it is safer and more efficient to map only those things we 
            // care about.

            try
            {
                Variant v = (Variant)Marshal.PtrToStructure(ptr, typeof(Variant));
                return v;
            }
            catch (ArgumentException e)
            {
                Debug.Assert(false, e.Message);
            }
            return new Variant();
        }

        /// <include file='doc\Utilities.uex' path='docs/doc[@for="Variant.ToChar"]/*' />
        public char ToChar()
        {
            if (this.Vt == VariantType.VT_UI2)
            {
                ushort cv = (ushort)(this.value & 0xffff);
                return Convert.ToChar(cv);
            }
            return '\0';
        }

    }
}
