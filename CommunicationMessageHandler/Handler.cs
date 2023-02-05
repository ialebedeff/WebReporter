using Microsoft.AspNetCore.SignalR.Client;

namespace CommunicationMessageHandler
{
    public class Handler
    {
        public Handler(HubConnection hubConnection)
        {
            HubConnection= hubConnection;
        }
        public HubConnection HubConnection { get; set; } = default!;
    }
}