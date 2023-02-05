namespace Managers
{
    public class MessageManager
    {
        public MessageManager()
        {
            Database = new DatabaseManager();
        }
        private static Lazy<MessageManager> _instance = new Lazy<MessageManager>(new MessageManager());
        public static MessageManager Instance => _instance.Value;
        public DatabaseManager Database { get; set; }
    }
}