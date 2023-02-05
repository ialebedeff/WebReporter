using CommunicationMessageHandler.Client;
using Microsoft.AspNetCore.SignalR.Client;

namespace CommunicationMessageHandler
{
    public class ClientHandler : Handler
    {
        public ClientHandler(HubConnection hubConnection) : base(hubConnection)
        {
            Database = new DatabaseMessageHandler(hubConnection);
        }
        public DatabaseMessageHandler Database { get; set; }
    }
}