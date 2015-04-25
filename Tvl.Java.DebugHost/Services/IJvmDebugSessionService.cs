namespace Tvl.Java.DebugHost.Services
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IJvmDebugSessionService
    {
        [OperationContract]
        void Attach();

        [OperationContract]
        void Detach();

        [OperationContract]
        void Terminate();
    }
}
