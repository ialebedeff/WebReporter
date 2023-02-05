namespace CommunicationMessageQueueManager
{
    public class RequestManager
    {
        public RequestManager(MessageQueueManager messageQueueManager)
        {
            Database = new DatabaseRequestManager(messageQueueManager);
        }

        public DatabaseRequestManager Database { get; set; }
    }
}