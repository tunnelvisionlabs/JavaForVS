namespace Tvl.Java.DebugHost.Services
{
    using System.ServiceModel;
    using AsyncCallback = System.AsyncCallback;
    using IAsyncResult = System.IAsyncResult;

    public interface IJvmEvents
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginOnSingleStep(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, JvmRemoteLocation location, AsyncCallback callback, object asyncState);
        void EndOnSingleStep(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginHandleVMStart(JvmVirtualMachineRemoteHandle virtualMachine, AsyncCallback callback, object asyncState);
        void EndHandleVMStart(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginHandleVMInitialization(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, AsyncCallback callback, object asyncState);
        void EndHandleVMInitialization(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginHandleVMDeath(JvmVirtualMachineRemoteHandle virtualMachine, AsyncCallback callback, object asyncState);
        void EndHandleVMDeath(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginHandleThreadStart(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, AsyncCallback callback, object asyncState);
        void EndHandleThreadStart(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginHandleThreadEnd(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, AsyncCallback callback, object asyncState);
        void EndHandleThreadEnd(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginHandleClassLoad(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, JvmClassRemoteHandle @class, AsyncCallback callback, object asyncState);
        void EndHandleClassLoad(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginHandleClassPrepare(JvmVirtualMachineRemoteHandle virtualMachine, JvmThreadRemoteHandle thread, JvmClassRemoteHandle @class, AsyncCallback callback, object asyncState);
        void EndHandleClassPrepare(IAsyncResult result);
    }
}
