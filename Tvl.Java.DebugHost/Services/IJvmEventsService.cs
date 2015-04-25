namespace Tvl.Java.DebugHost.Services
{
    using System.ServiceModel;

    [ServiceContract(CallbackContract = typeof(IJvmEvents), SessionMode = SessionMode.Required)]
    public interface IJvmEventsService
    {
        [OperationContract]
        void Subscribe(JvmEventType eventType);

        [OperationContract]
        void Unsubscribe(JvmEventType eventType);
    }
}
