using CommunicationMessageExecutor.Client;
using CommunicationMessageWorker;
using Entities;
using Microsoft.AspNetCore.SignalR.Client;

namespace CommunicationMessageHandler.Client
{
    public class DatabaseMessageHandler : MessageHandler<DatabaseCommand, List<Dictionary<string, object>>>
    {
        public DatabaseMessageHandler(HubConnection hubConnection) : base("DatabaseExecutionResult", hubConnection)
            => DatabaseExecutor = new DatabaseExecutor();
        public DatabaseExecutor DatabaseExecutor { get; private set; }
        public override async Task ExecuteAsync(Message<DatabaseCommand> message)
            => await SendAsync(await DatabaseExecutor.ExecuteAsync(message), message.MessageId);
    }
}