using CommunicationMessageHandler;
using CommunicationMessageHandler.Client;
using CommunicationMessageWorker;
using Entities;
using Managers;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunicationService
{
    public class SignalClient
    {
        public Action<HubConnection> Configuration { get; private set; }
        public HubConnection HubConnection { get; private set; }
        public SignalClient(
            IConfiguration configuration,
            Action<HubConnection> connection)
        {
            HubConnection = new HubConnectionBuilder()
                .WithUrl(configuration.GetRequiredSection("Server")
                                      .GetRequiredSection("BridgeHub")
                                      .Value ?? throw new NullReferenceException("Данные о подключения к серверу не найдены"))
                .WithAutomaticReconnect()
                .Build();
           
            Configuration = connection;
            Configuration.Invoke(HubConnection);
        }
        public SignalClient(
           string serverUrl,
           Action<HubConnection> connection)
        {
            HubConnection = new HubConnectionBuilder()
                .WithUrl(serverUrl ?? throw new NullReferenceException("Данные о подключения к серверу не найдены"))
                .WithAutomaticReconnect()
                .AddJsonProtocol()
                .Build();

            Configuration = connection;
            Configuration.Invoke(HubConnection);
        }

        public async Task StartAsync()
        {
            if (HubConnection.State is HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
    }
}