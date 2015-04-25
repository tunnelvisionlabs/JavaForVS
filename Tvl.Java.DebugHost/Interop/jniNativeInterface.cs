// The field 'field_name' is never used
#pragma warning disable 169
// Field 'field_name' is never assigned to, and will always have its default value null
#pragma warning disable 649

namespace Tvl.Java.DebugHost.Interop
{
    using System.Runtime.InteropServices;
    using IntPtr = System.IntPtr;

    [StructLayout(LayoutKind.Sequential)]
    internal sealed class jniNativeInterface
    {
        private IntPtr _reserved0;
        private IntPtr _reserved1;
        private IntPtr _reserved2;

        private IntPtr _reserved3;

        public JniUnsafeNativeMethods.GetVersion GetVersion;

        public JniUnsafeNativeMethods.DefineClass DefineClass;
        public JniUnsafeNativeMethods.FindClass FindClass;

        public JniUnsafeNativeMethods.FromReflectedMethod FromReflectedMethod;
        public JniUnsafeNativeMethods.FromReflectedField FromReflectedField;

        public JniUnsafeNativeMethods.ToReflectedMethod ToReflectedMethod;

        public JniUnsafeNativeMethods.GetSuperclass GetSuperclass;
        public JniUnsafeNativeMethods.IsAssignableFrom IsAssignableFrom;

        public JniUnsafeNativeMethods.ToReflectedField ToReflectedField;

        public JniUnsafeNativeMethods.Throw Throw;
        public JniUnsafeNativeMethods.ThrowNew ThrowNew;
        public JniUnsafeNativeMethods.ExceptionOccurred ExceptionOccurred;
        public JniUnsafeNativeMethods.ExceptionDescribe ExceptionDescribe;
        public JniUnsafeNativeMethods.ExceptionClear ExceptionClear;
        public JniUnsafeNativeMethods.FatalError FatalError;

        public JniUnsafeNativeMethods.PushLocalFrame PushLocalFrame;
        public JniUnsafeNativeMethods.PopLocalFrame PopLocalFrame;

        public JniUnsafeNativeMethods.NewGlobalRef NewGlobalRef;
        public JniUnsafeNativeMethods.DeleteGlobalRef DeleteGlobalRef;
        public JniUnsafeNativeMethods.DeleteLocalRef DeleteLocalRef;
        public JniUnsafeNativeMethods.IsSameObject IsSameObject;
        public JniUnsafeNativeMethods.NewLocalRef NewLocalRef;
        public JniUnsafeNativeMethods.EnsureLocalCapacity EnsureLocalCapacity;

        public JniUnsafeNativeMethods.AllocObject AllocObject;
        public JniUnsafeNativeMethods.NewObject NewObject;
        public JniUnsafeNativeMethods.NewObjectV NewObjectV;
        public JniUnsafeNativeMethods.NewObjectA NewObjectA;

        public JniUnsafeNativeMethods.GetObjectClass GetObjectClass;
        public JniUnsafeNativeMethods.IsInstanceOf IsInstanceOf;

        public JniUnsafeNativeMethods.GetMethodID GetMethodID;

        private IntPtr CallObjectMethod;
        private IntPtr CallObjectMethodV;
        public JniUnsafeNativeMethods.CallObjectMethodA CallObjectMethodA;

        private IntPtr CallBooleanMethod;
        private IntPtr CallBooleanMethodV;
        public JniUnsafeNativeMethods.CallBooleanMethodA CallBooleanMethodA;

        private IntPtr CallByteMethod;
        private IntPtr CallByteMethodV;
        public JniUnsafeNativeMethods.CallByteMethodA CallByteMethodA;

        private IntPtr CallCharMethod;
        private IntPtr CallCharMethodV;
        public JniUnsafeNativeMethods.CallCharMethodA CallCharMethodA;

        private IntPtr CallShortMethod;
        private IntPtr CallShortMethodV;
        public JniUnsafeNativeMethods.CallShortMethodA CallShortMethodA;

        private IntPtr CallIntMethod;
        private IntPtr CallIntMethodV;
        public JniUnsafeNativeMethods.CallIntMethodA CallIntMethodA;

        private IntPtr CallLongMethod;
        private IntPtr CallLongMethodV;
        public JniUnsafeNativeMethods.CallLongMethodA CallLongMethodA;

        private IntPtr CallFloatMethod;
        private IntPtr CallFloatMethodV;
        public JniUnsafeNativeMethods.CallFloatMethodA CallFloatMethodA;

        private IntPtr CallDoubleMethod;
        private IntPtr CallDoubleMethodV;
        public JniUnsafeNativeMethods.CallDoubleMethodA CallDoubleMethodA;

        private IntPtr CallVoidMethod;
        private IntPtr CallVoidMethodV;
        public JniUnsafeNativeMethods.CallVoidMethodA CallVoidMethodA;

        private IntPtr CallNonvirtualObjectMethod;
        private IntPtr CallNonvirtualObjectMethodV;
        public JniUnsafeNativeMethods.CallNonvirtualObjectMethodA CallNonvirtualObjectMethodA;

        private IntPtr CallNonvirtualBooleanMethod;
        private IntPtr CallNonvirtualBooleanMethodV;
        public JniUnsafeNativeMethods.CallNonvirtualBooleanMethodA CallNonvirtualBooleanMethodA;

        private IntPtr CallNonvirtualByteMethod;
        private IntPtr CallNonvirtualByteMethodV;
        public JniUnsafeNativeMethods.CallNonvirtualByteMethodA CallNonvirtualByteMethodA;

        private IntPtr CallNonvirtualCharMethod;
        private IntPtr CallNonvirtualCharMethodV;
        public JniUnsafeNativeMethods.CallNonvirtualCharMethodA CallNonvirtualCharMethodA;

        private IntPtr CallNonvirtualShortMethod;
        private IntPtr CallNonvirtualShortMethodV;
        public JniUnsafeNativeMethods.CallNonvirtualShortMethodA CallNonvirtualShortMethodA;

        private IntPtr CallNonvirtualIntMethod;
        private IntPtr CallNonvirtualIntMethodV;
        public JniUnsafeNativeMethods.CallNonvirtualIntMethodA CallNonvirtualIntMethodA;

        private IntPtr CallNonvirtualLongMethod;
        private IntPtr CallNonvirtualLongMethodV;
        public JniUnsafeNativeMethods.CallNonvirtualLongMethodA CallNonvirtualLongMethodA;

        private IntPtr CallNonvirtualFloatMethod;
        private IntPtr CallNonvirtualFloatMethodV;
        public JniUnsafeNativeMethods.CallNonvirtualFloatMethodA CallNonvirtualFloatMethodA;

        private IntPtr CallNonvirtualDoubleMethod;
        private IntPtr CallNonvirtualDoubleMethodV;
        public JniUnsafeNativeMethods.CallNonvirtualDoubleMethodA CallNonvirtualDoubleMethodA;

        private IntPtr CallNonvirtualVoidMethod;
        private IntPtr CallNonvirtualVoidMethodV;
        public JniUnsafeNativeMethods.CallNonvirtualVoidMethodA CallNonvirtualVoidMethodA;

        public JniUnsafeNativeMethods.GetFieldID GetFieldID;

        public JniUnsafeNativeMethods.GetObjectField GetObjectField;
        public JniUnsafeNativeMethods.GetBooleanField GetBooleanField;
        public JniUnsafeNativeMethods.GetByteField GetByteField;
        public JniUnsafeNativeMethods.GetCharField GetCharField;
        public JniUnsafeNativeMethods.GetShortField GetShortField;
        public JniUnsafeNativeMethods.GetIntField GetIntField;
        public JniUnsafeNativeMethods.GetLongField GetLongField;
        public JniUnsafeNativeMethods.GetFloatField GetFloatField;
        public JniUnsafeNativeMethods.GetDoubleField GetDoubleField;

        public JniUnsafeNativeMethods.SetObjectField SetObjectField;
        public JniUnsafeNativeMethods.SetBooleanField SetBooleanField;
        public JniUnsafeNativeMethods.SetByteField SetByteField;
        public JniUnsafeNativeMethods.SetCharField SetCharField;
        public JniUnsafeNativeMethods.SetShortField SetShortField;
        public JniUnsafeNativeMethods.SetIntField SetIntField;
        public JniUnsafeNativeMethods.SetLongField SetLongField;
        public JniUnsafeNativeMethods.SetFloatField SetFloatField;
        public JniUnsafeNativeMethods.SetDoubleField SetDoubleField;

        public JniUnsafeNativeMethods.GetStaticMethodID GetStaticMethodID;

        private IntPtr CallStaticObjectMethod;
        private IntPtr CallStaticObjectMethodV;
        public JniUnsafeNativeMethods.CallStaticObjectMethodA CallStaticObjectMethodA;

        private IntPtr CallStaticBooleanMethod;
        private IntPtr CallStaticBooleanMethodV;
        public JniUnsafeNativeMethods.CallStaticBooleanMethodA CallStaticBooleanMethodA;

        private IntPtr CallStaticByteMethod;
        private IntPtr CallStaticByteMethodV;
        public JniUnsafeNativeMethods.CallStaticByteMethodA CallStaticByteMethodA;

        private IntPtr CallStaticCharMethod;
        private IntPtr CallStaticCharMethodV;
        public JniUnsafeNativeMethods.CallStaticCharMethodA CallStaticCharMethodA;

        private IntPtr CallStaticShortMethod;
        private IntPtr CallStaticShortMethodV;
        public JniUnsafeNativeMethods.CallStaticShortMethodA CallStaticShortMethodA;

        private IntPtr CallStaticIntMethod;
        private IntPtr CallStaticIntMethodV;
        public JniUnsafeNativeMethods.CallStaticIntMethodA CallStaticIntMethodA;

        private IntPtr CallStaticLongMethod;
        private IntPtr CallStaticLongMethodV;
        public JniUnsafeNativeMethods.CallStaticLongMethodA CallStaticLongMethodA;

        private IntPtr CallStaticFloatMethod;
        private IntPtr CallStaticFloatMethodV;
        public JniUnsafeNativeMethods.CallStaticFloatMethodA CallStaticFloatMethodA;

        private IntPtr CallStaticDoubleMethod;
        private IntPtr CallStaticDoubleMethodV;
        public JniUnsafeNativeMethods.CallStaticDoubleMethodA CallStaticDoubleMethodA;

        private IntPtr CallStaticVoidMethod;
        private IntPtr CallStaticVoidMethodV;
        public JniUnsafeNativeMethods.CallStaticVoidMethodA CallStaticVoidMethodA;

        public JniUnsafeNativeMethods.GetStaticFieldID GetStaticFieldID;
        public JniUnsafeNativeMethods.GetStaticObjectField GetStaticObjectField;
        public JniUnsafeNativeMethods.GetStaticBooleanField GetStaticBooleanField;
        public JniUnsafeNativeMethods.GetStaticByteField GetStaticByteField;
        public JniUnsafeNativeMethods.GetStaticCharField GetStaticCharField;
        public JniUnsafeNativeMethods.GetStaticShortField GetStaticShortField;
        public JniUnsafeNativeMethods.GetStaticIntField GetStaticIntField;
        public JniUnsafeNativeMethods.GetStaticLongField GetStaticLongField;
        public JniUnsafeNativeMethods.GetStaticFloatField GetStaticFloatField;
        public JniUnsafeNativeMethods.GetStaticDoubleField GetStaticDoubleField;

        public JniUnsafeNativeMethods.SetStaticObjectField SetStaticObjectField;
        public JniUnsafeNativeMethods.SetStaticBooleanField SetStaticBooleanField;
        public JniUnsafeNativeMethods.SetStaticByteField SetStaticByteField;
        public JniUnsafeNativeMethods.SetStaticCharField SetStaticCharField;
        public JniUnsafeNativeMethods.SetStaticShortField SetStaticShortField;
        public JniUnsafeNativeMethods.SetStaticIntField SetStaticIntField;
        public JniUnsafeNativeMethods.SetStaticLongField SetStaticLongField;
        public JniUnsafeNativeMethods.SetStaticFloatField SetStaticFloatField;
        public JniUnsafeNativeMethods.SetStaticDoubleField SetStaticDoubleField;

        public JniUnsafeNativeMethods.NewString NewString;
        public JniUnsafeNativeMethods.GetStringLength GetStringLength;
        public JniUnsafeNativeMethods.GetStringChars GetStringChars;
        public JniUnsafeNativeMethods.ReleaseStringChars ReleaseStringChars;

        public JniUnsafeNativeMethods.NewStringUTF NewStringUTF;
        public JniUnsafeNativeMethods.GetStringUTFLength GetStringUTFLength;
        public JniUnsafeNativeMethods.GetStringUTFChars GetStringUTFChars;
        public JniUnsafeNativeMethods.ReleaseStringUTFChars ReleaseStringUTFChars;

        public JniUnsafeNativeMethods.GetArrayLength GetArrayLength;

        public JniUnsafeNativeMethods.NewObjectArray NewObjectArray;
        public JniUnsafeNativeMethods.GetObjectArrayElement GetObjectArrayElement;
        public JniUnsafeNativeMethods.SetObjectArrayElement SetObjectArrayElement;

        public JniUnsafeNativeMethods.NewBooleanArray NewBooleanArray;
        public JniUnsafeNativeMethods.NewByteArray NewByteArray;
        public JniUnsafeNativeMethods.NewCharArray NewCharArray;
        public JniUnsafeNativeMethods.NewShortArray NewShortArray;
        public JniUnsafeNativeMethods.NewIntArray NewIntArray;
        public JniUnsafeNativeMethods.NewLongArray NewLongArray;
        public JniUnsafeNativeMethods.NewFloatArray NewFloatArray;
        public JniUnsafeNativeMethods.NewDoubleArray NewDoubleArray;

        public JniUnsafeNativeMethods.GetBooleanArrayElements GetBooleanArrayElements;
        public JniUnsafeNativeMethods.GetByteArrayElements GetByteArrayElements;
        public JniUnsafeNativeMethods.GetCharArrayElements GetCharArrayElements;
        public JniUnsafeNativeMethods.GetShortArrayElements GetShortArrayElements;
        public JniUnsafeNativeMethods.GetIntArrayElements GetIntArrayElements;
        public JniUnsafeNativeMethods.GetLongArrayElements GetLongArrayElements;
        public JniUnsafeNativeMethods.GetFloatArrayElements GetFloatArrayElements;
        public JniUnsafeNativeMethods.GetDoubleArrayElements GetDoubleArrayElements;

        public JniUnsafeNativeMethods.ReleaseBooleanArrayElements ReleaseBooleanArrayElements;
        public JniUnsafeNativeMethods.ReleaseByteArrayElements ReleaseByteArrayElements;
        public JniUnsafeNativeMethods.ReleaseCharArrayElements ReleaseCharArrayElements;
        public JniUnsafeNativeMethods.ReleaseShortArrayElements ReleaseShortArrayElements;
        public JniUnsafeNativeMethods.ReleaseIntArrayElements ReleaseIntArrayElements;
        public JniUnsafeNativeMethods.ReleaseLongArrayElements ReleaseLongArrayElements;
        public JniUnsafeNativeMethods.ReleaseFloatArrayElements ReleaseFloatArrayElements;
        public JniUnsafeNativeMethods.ReleaseDoubleArrayElements ReleaseDoubleArrayElements;

        public JniUnsafeNativeMethods.GetBooleanArrayRegion GetBooleanArrayRegion;
        public JniUnsafeNativeMethods.GetByteArrayRegion GetByteArrayRegion;
        public JniUnsafeNativeMethods.GetCharArrayRegion GetCharArrayRegion;
        public JniUnsafeNativeMethods.GetShortArrayRegion GetShortArrayRegion;
        public JniUnsafeNativeMethods.GetIntArrayRegion GetIntArrayRegion;
        public JniUnsafeNativeMethods.GetLongArrayRegion GetLongArrayRegion;
        public JniUnsafeNativeMethods.GetFloatArrayRegion GetFloatArrayRegion;
        public JniUnsafeNativeMethods.GetDoubleArrayRegion GetDoubleArrayRegion;

        public JniUnsafeNativeMethods.SetBooleanArrayRegion SetBooleanArrayRegion;
        public JniUnsafeNativeMethods.SetByteArrayRegion SetByteArrayRegion;
        public JniUnsafeNativeMethods.SetCharArrayRegion SetCharArrayRegion;
        public JniUnsafeNativeMethods.SetShortArrayRegion SetShortArrayRegion;
        public JniUnsafeNativeMethods.SetIntArrayRegion SetIntArrayRegion;
        public JniUnsafeNativeMethods.SetLongArrayRegion SetLongArrayRegion;
        public JniUnsafeNativeMethods.SetFloatArrayRegion SetFloatArrayRegion;
        public JniUnsafeNativeMethods.SetDoubleArrayRegion SetDoubleArrayRegion;

        public JniUnsafeNativeMethods.RegisterNatives RegisterNatives;
        public JniUnsafeNativeMethods.UnregisterNatives UnregisterNatives;

        public JniUnsafeNativeMethods.MonitorEnter MonitorEnter;
        public JniUnsafeNativeMethods.MonitorExit MonitorExit;

        public JniUnsafeNativeMethods.GetJavaVM GetJavaVM;

        public JniUnsafeNativeMethods.GetStringRegion GetStringRegion;
        public JniUnsafeNativeMethods.GetStringUTFRegion GetStringUTFRegion;

        public JniUnsafeNativeMethods.GetPrimitiveArrayCritical GetPrimitiveArrayCritical;
        public JniUnsafeNativeMethods.ReleasePrimitiveArrayCritical ReleasePrimitiveArrayCritical;

        public JniUnsafeNativeMethods.GetStringCritical GetStringCritical;
        public JniUnsafeNativeMethods.ReleaseStringCritical ReleaseStringCritical;

        public JniUnsafeNativeMethods.NewWeakGlobalRef NewWeakGlobalRef;
        public JniUnsafeNativeMethods.DeleteWeakGlobalRef DeleteWeakGlobalRef;

        public JniUnsafeNativeMethods.ExceptionCheck ExceptionCheck;

        public JniUnsafeNativeMethods.NewDirectByteBuffer NewDirectByteBuffer;
        public JniUnsafeNativeMethods.GetDirectBufferAddress GetDirectBufferAddress;
        public JniUnsafeNativeMethods.GetDirectBufferCapacity GetDirectBufferCapacity;

        /* New JNI 1.6 Features */

        public JniUnsafeNativeMethods.GetObjectRefType GetObjectRefType;
    }
}
