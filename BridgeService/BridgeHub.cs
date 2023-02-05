using Entities;
using Microsoft.AspNetCore.SignalR;

namespace BridgeService
{
    public class BridgeHub : Hub
    {
        public BridgeHub() { }
        public async Task ExecuteDatabaseCommand(Message<DatabaseCommand> message)
            => await this.Clients.All.SendAsync("ExecuteDatabaseCommand", message);
        public async Task DatabaseExecutionResult(Message<List<Dictionary<string, object>>> message)
            => await this.Clients.All.SendAsync("DatabaseExecutionResult", message);
    }
}