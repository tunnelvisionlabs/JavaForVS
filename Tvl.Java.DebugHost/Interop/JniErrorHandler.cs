namespace Tvl.Java.DebugHost.Interop
{
    using System;

    internal static class JniErrorHandler
    {
        public static void ThrowOnFailure(int result)
        {
            if (result != 0)
                throw new Exception("JNI Exception Occurred.");
        }
    }
}
