namespace Entities
{
    public class Message<T>
    {
        public string ConnectionId { get; set; } = null!;
        public string MessageId { get; set; } = null!;
        public T Data { get; set; } = default!;
        /// <summary>
        /// Инициализация ответа клиентом
        /// </summary>
        /// <param name="messageId">Id сообщения, на которое происходит ответ</param>
        /// <param name="data">Данные</param>
        public Message(string messageId, T data)
        {
            MessageId = messageId;
            Data = data;
        }
        /// <summary>
        /// Инициализация сервером
        /// </summary>
        /// <param name="data">Данные</param>
        public Message(T data)
        {
            MessageId = Guid
                .NewGuid()
                .ToString();

            Data = data;
        }
        public Message() { }
    }
    public class DatabaseCommand
    {
        public DatabaseCommand() { }
        public DatabaseCommand(string command, DatabaseConnectionInfo connection)
        {
            Command = command;
            Connection = connection;
        }
        public string Command { get; set; } = default!; 
        public DatabaseConnectionInfo Connection { get; set; } = default!;
    }
    
    public class DatabaseConnectionInfo
    {
        public DatabaseConnectionInfo() { }
        public DatabaseConnectionInfo(string server, string database, string user, string password)
        {
            Server = server;
            Database = database;
            User = user;
            Password = password;
        }
        public string Server { get; set; } = default!;
        public string Database { get; set; } = default!;
        public string User { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}