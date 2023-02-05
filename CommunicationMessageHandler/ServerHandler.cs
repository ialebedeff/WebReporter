using Microsoft.AspNetCore.SignalR.Client;
using System.Runtime.InteropServices;

namespace CommunicationMessageHandler
{
    public class ServerHandler : Handler
    {
        public ServerHandler(HubConnection hubConnection) : base(hubConnection)
        {
        }
    }
}