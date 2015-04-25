namespace Tvl.Java.DebugHost.Interop
{
    using System.Collections.Concurrent;
    using System.Runtime.InteropServices;

    public sealed class JniEnvironment
    {
        private static readonly ConcurrentDictionary<JNIEnvHandle, JniEnvironment> _instances =
            new ConcurrentDictionary<JNIEnvHandle, JniEnvironment>();

        private readonly JNIEnvHandle _handle;
        private readonly jniNativeInterface _rawInterface;

        public JniEnvironment(JNIEnvHandle handle)
        {
            _handle = handle;
            _rawInterface = (jniNativeInterface)Marshal.PtrToStructure(Marshal.ReadIntPtr(handle.Handle), typeof(jniNativeInterface));
        }

        internal jniNativeInterface RawInterface
        {
            get
            {
                return _rawInterface;
            }
        }

        public static implicit operator JNIEnvHandle(JniEnvironment jniEnvironment)
        {
            return jniEnvironment._handle;
        }

        internal static JniEnvironment GetOrCreateInstance(JNIEnvHandle handle)
        {
            bool created;
            return GetOrCreateInstance(handle, out created);
        }

        internal static JniEnvironment GetOrCreateInstance(JNIEnvHandle handle, out bool created)
        {
            bool wasCreated = false;
            JniEnvironment environment = _instances.GetOrAdd(handle,
                i =>
                {
                    wasCreated = true;
                    return CreateVirtualMachine(i);
                });

            created = wasCreated;
            return environment;
        }

        private static JniEnvironment CreateVirtualMachine(JNIEnvHandle handle)
        {
            return new JniEnvironment(handle);
        }

        public int GetVersion()
        {
            int version = RawInterface.GetVersion(this);
            HandleException();
            return version;
        }

        public jclass DefineClass(string name, jobject loader, byte[] data)
        {
            jclass result = RawInterface.DefineClass(this, name, loader, data, data.Length);
            HandleException();
            return result;
        }

        public jclass FindClass(string name)
        {
            jclass result = RawInterface.FindClass(this, name);
            HandleException();
            return result;
        }

        public jclass GetObjectClass(jobject @object)
        {
            jclass result = RawInterface.GetObjectClass(this, @object);
            HandleException();
            return result;
        }

        public jclass GetSuperclass(jclass @class)
        {
            jclass result = RawInterface.GetSuperclass(this, @class);
            HandleException();
            return result;
        }

        public bool IsInstanceOf(jobject @object, jclass @class)
        {
            byte result = RawInterface.IsInstanceOf(this, @object, @class);
            HandleException();
            return result != 0;
        }

        public void ExceptionClear()
        {
            RawInterface.ExceptionClear(this);
        }

        public jthrowable ExceptionOccurred()
        {
            return RawInterface.ExceptionOccurred(this);
        }

        public bool IsSameObject(jobject x, jobject y)
        {
            byte result = RawInterface.IsSameObject(this, x, y);
            HandleException();
            return result != 0;
        }

        public jobject NewLocalReference(jobject @object)
        {
            jobject result = RawInterface.NewLocalRef(this, @object);
            HandleException();
            return result;
        }

        public void DeleteLocalReference(jobject @object)
        {
            RawInterface.DeleteLocalRef(this, @object);
            HandleException();
        }

        public jobject NewGlobalReference(jobject @object)
        {
            jobject result = RawInterface.NewGlobalRef(this, @object);
            HandleException();
            return result;
        }

        public void DeleteGlobalReference(jobject @object)
        {
            RawInterface.DeleteGlobalRef(this, @object);
            HandleException();
        }

        public jweak NewWeakGlobalReference(jobject @object)
        {
            jweak result = RawInterface.NewWeakGlobalRef(this, @object);
            HandleException();
            return result;
        }

        public void DeleteWeakGlobalReference(jweak @object)
        {
            RawInterface.DeleteWeakGlobalRef(this, @object);
            HandleException();
        }

        public int GetStringUTFLength(jobject stringHandle)
        {
            int result = RawInterface.GetStringUTFLength(this, stringHandle);
            HandleException();
            return result;
        }

        public void GetStringUTFRegion(jobject stringHandle, int start, int length, byte[] buffer)
        {
            RawInterface.GetStringUTFRegion(this, stringHandle, start, length, buffer);
            HandleException();
        }

        public int GetArrayLength(jobject arrayHandle)
        {
            int result = RawInterface.GetArrayLength(this, arrayHandle);
            HandleException();
            return result;
        }

        public jobject NewString(string value)
        {
            if (value == null)
                return jobject.Null;

            jobject result = RawInterface.NewString(this, value, value.Length);
            HandleException();
            return result;
        }

        public jobject NewObject(jclass @class, jmethodID ctorMethodId, params jvalue[] args)
        {
            jobject result = RawInterface.NewObjectA(this, @class, ctorMethodId, args);
            HandleException();
            return result;
        }

        public jmethodID GetMethodId(jclass @class, string name, string signature)
        {
            jmethodID result = RawInterface.GetMethodID(this, @class, name, signature);
            HandleException();
            return result;
        }

        internal bool GetStaticBooleanField(jclass classHandle, jfieldID fieldId)
        {
            bool result = RawInterface.GetStaticBooleanField(this, classHandle, fieldId) != 0;
            HandleException();
            return result;
        }

        internal byte GetStaticByteField(jclass classHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetStaticByteField(this, classHandle, fieldId);
            HandleException();
            return result;
        }

        internal char GetStaticCharField(jclass classHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetStaticCharField(this, classHandle, fieldId);
            HandleException();
            return result;
        }

        internal double GetStaticDoubleField(jclass classHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetStaticDoubleField(this, classHandle, fieldId);
            HandleException();
            return result;
        }

        internal float GetStaticFloatField(jclass classHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetStaticFloatField(this, classHandle, fieldId);
            HandleException();
            return result;
        }

        internal int GetStaticIntField(jclass classHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetStaticIntField(this, classHandle, fieldId);
            HandleException();
            return result;
        }

        internal long GetStaticLongField(jclass classHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetStaticLongField(this, classHandle, fieldId);
            HandleException();
            return result;
        }

        internal short GetStaticShortField(jclass classHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetStaticShortField(this, classHandle, fieldId);
            HandleException();
            return result;
        }

        internal jobject GetStaticObjectField(jclass classHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetStaticObjectField(this, classHandle, fieldId);
            HandleException();
            return result;
        }

        internal bool GetBooleanField(jobject objectHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetBooleanField(this, objectHandle, fieldId) != 0;
            HandleException();
            return result;
        }

        internal byte GetByteField(jobject objectHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetByteField(this, objectHandle, fieldId);
            HandleException();
            return result;
        }

        internal char GetCharField(jobject objectHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetCharField(this, objectHandle, fieldId);
            HandleException();
            return result;
        }

        internal double GetDoubleField(jobject objectHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetDoubleField(this, objectHandle, fieldId);
            HandleException();
            return result;
        }

        internal float GetFloatField(jobject objectHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetFloatField(this, objectHandle, fieldId);
            HandleException();
            return result;
        }

        internal int GetIntField(jobject objectHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetIntField(this, objectHandle, fieldId);
            HandleException();
            return result;
        }

        internal long GetLongField(jobject objectHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetLongField(this, objectHandle, fieldId);
            HandleException();
            return result;
        }

        internal short GetShortField(jobject objectHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetShortField(this, objectHandle, fieldId);
            HandleException();
            return result;
        }

        internal jobject GetObjectField(jobject objectHandle, jfieldID fieldId)
        {
            var result = RawInterface.GetObjectField(this, objectHandle, fieldId);
            HandleException();
            return result;
        }

        internal bool CallStaticBooleanMethodA(jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallStaticBooleanMethodA(this, classHandle, methodId, args) != 0;
            HandleException();
            return result;
        }

        internal byte CallStaticByteMethodA(jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallStaticByteMethodA(this, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal char CallStaticCharMethodA(jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallStaticCharMethodA(this, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal double CallStaticDoubleMethodA(jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallStaticDoubleMethodA(this, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal float CallStaticFloatMethodA(jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallStaticFloatMethodA(this, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal int CallStaticIntMethodA(jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallStaticIntMethodA(this, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal long CallStaticLongMethodA(jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallStaticLongMethodA(this, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal short CallStaticShortMethodA(jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallStaticShortMethodA(this, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal jobject CallStaticObjectMethodA(jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallStaticObjectMethodA(this, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal bool CallBooleanMethodA(jobject instanceHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallBooleanMethodA(this, instanceHandle, methodId, args) != 0;
            HandleException();
            return result;
        }

        internal byte CallByteMethodA(jobject instanceHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallByteMethodA(this, instanceHandle, methodId, args);
            HandleException();
            return result;
        }

        internal char CallCharMethodA(jobject instanceHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallCharMethodA(this, instanceHandle, methodId, args);
            HandleException();
            return result;
        }

        internal double CallDoubleMethodA(jobject instanceHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallDoubleMethodA(this, instanceHandle, methodId, args);
            HandleException();
            return result;
        }

        internal float CallFloatMethodA(jobject instanceHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallFloatMethodA(this, instanceHandle, methodId, args);
            HandleException();
            return result;
        }

        internal int CallIntMethodA(jobject instanceHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallIntMethodA(this, instanceHandle, methodId, args);
            HandleException();
            return result;
        }

        internal long CallLongMethodA(jobject instanceHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallLongMethodA(this, instanceHandle, methodId, args);
            HandleException();
            return result;
        }

        internal short CallShortMethodA(jobject instanceHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallShortMethodA(this, instanceHandle, methodId, args);
            HandleException();
            return result;
        }

        internal void CallVoidMethodA(jobject instanceHandle, jmethodID methodId, params jvalue[] args)
        {
            RawInterface.CallVoidMethodA(this, instanceHandle, methodId, args);
            HandleException();
        }

        internal jobject CallObjectMethodA(jobject instanceHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallObjectMethodA(this, instanceHandle, methodId, args);
            HandleException();
            return result;
        }

        internal bool CallNonvirtualBooleanMethodA(jobject instanceHandle, jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallNonvirtualBooleanMethodA(this, instanceHandle, classHandle, methodId, args) != 0;
            HandleException();
            return result;
        }

        internal byte CallNonvirtualByteMethodA(jobject instanceHandle, jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallNonvirtualByteMethodA(this, instanceHandle, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal char CallNonvirtualCharMethodA(jobject instanceHandle, jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallNonvirtualCharMethodA(this, instanceHandle, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal double CallNonvirtualDoubleMethodA(jobject instanceHandle, jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallNonvirtualDoubleMethodA(this, instanceHandle, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal float CallNonvirtualFloatMethodA(jobject instanceHandle, jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallNonvirtualFloatMethodA(this, instanceHandle, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal int CallNonvirtualIntMethodA(jobject instanceHandle, jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallNonvirtualIntMethodA(this, instanceHandle, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal long CallNonvirtualLongMethodA(jobject instanceHandle, jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallNonvirtualLongMethodA(this, instanceHandle, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal short CallNonvirtualShortMethodA(jobject instanceHandle, jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallNonvirtualShortMethodA(this, instanceHandle, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal void CallNonvirtualVoidMethodA(jobject instanceHandle, jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            RawInterface.CallNonvirtualVoidMethodA(this, instanceHandle, classHandle, methodId, args);
            HandleException();
        }

        internal jobject CallNonvirtualObjectMethodA(jobject instanceHandle, jclass classHandle, jmethodID methodId, params jvalue[] args)
        {
            var result = RawInterface.CallNonvirtualObjectMethodA(this, instanceHandle, classHandle, methodId, args);
            HandleException();
            return result;
        }

        internal void GetBooleanArrayRegion(jobject arrayHandle, int start, int len, bool[] buf)
        {
            RawInterface.GetBooleanArrayRegion(this, arrayHandle, start, len, buf);
            HandleException();
        }

        internal void GetByteArrayRegion(jobject arrayHandle, int start, int len, byte[] buf)
        {
            RawInterface.GetByteArrayRegion(this, arrayHandle, start, len, buf);
            HandleException();
        }

        internal void GetCharArrayRegion(jobject arrayHandle, int start, int len, char[] buf)
        {
            RawInterface.GetCharArrayRegion(this, arrayHandle, start, len, buf);
            HandleException();
        }

        internal void GetDoubleArrayRegion(jobject arrayHandle, int start, int len, double[] buf)
        {
            RawInterface.GetDoubleArrayRegion(this, arrayHandle, start, len, buf);
            HandleException();
        }

        internal void GetFloatArrayRegion(jobject arrayHandle, int start, int len, float[] buf)
        {
            RawInterface.GetFloatArrayRegion(this, arrayHandle, start, len, buf);
            HandleException();
        }

        internal void GetIntArrayRegion(jobject arrayHandle, int start, int len, int[] buf)
        {
            RawInterface.GetIntArrayRegion(this, arrayHandle, start, len, buf);
            HandleException();
        }

        internal void GetLongArrayRegion(jobject arrayHandle, int start, int len, long[] buf)
        {
            RawInterface.GetLongArrayRegion(this, arrayHandle, start, len, buf);
            HandleException();
        }

        internal void GetShortArrayRegion(jobject arrayHandle, int start, int len, short[] buf)
        {
            RawInterface.GetShortArrayRegion(this, arrayHandle, start, len, buf);
            HandleException();
        }

        internal jobject GetObjectArrayElement(jobject arrayHandle, int index)
        {
            var result = RawInterface.GetObjectArrayElement(this, arrayHandle, index);
            HandleException();
            return result;
        }

        internal jobject NewBooleanArray(int length)
        {
            var result = RawInterface.NewBooleanArray(this, length);
            HandleException();
            return result;
        }

        internal jobject NewByteArray(int length)
        {
            var result = RawInterface.NewByteArray(this, length);
            HandleException();
            return result;
        }

        internal jobject NewCharArray(int length)
        {
            var result = RawInterface.NewCharArray(this, length);
            HandleException();
            return result;
        }

        internal jobject NewDoubleArray(int length)
        {
            var result = RawInterface.NewDoubleArray(this, length);
            HandleException();
            return result;
        }

        internal jobject NewFloatArray(int length)
        {
            var result = RawInterface.NewFloatArray(this, length);
            HandleException();
            return result;
        }

        internal jobject NewIntegerArray(int length)
        {
            var result = RawInterface.NewIntArray(this, length);
            HandleException();
            return result;
        }

        internal jobject NewLongArray(int length)
        {
            var result = RawInterface.NewLongArray(this, length);
            HandleException();
            return result;
        }

        internal jobject NewShortArray(int length)
        {
            var result = RawInterface.NewShortArray(this, length);
            HandleException();
            return result;
        }

        internal jobject NewObjectArray(int length, jclass elementType, jobject initialElement)
        {
            var result = RawInterface.NewObjectArray(this, length, elementType, initialElement);
            HandleException();
            return result;
        }

        private void HandleException()
        {
            jthrowable exception = ExceptionOccurred();
            if (exception != jthrowable.Null)
                throw new JniException(this, exception);
        }
    }
}
