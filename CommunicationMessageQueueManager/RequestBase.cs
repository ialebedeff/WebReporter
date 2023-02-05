namespace CommunicationMessageQueueManager
{
    public class RequestBase 
    {
        public RequestBase(MessageQueueManager messageQueueManager)
        {
            MessageQueueManager = messageQueueManager;
        }
        public MessageQueueManager MessageQueueManager { get; set; }
    }
}