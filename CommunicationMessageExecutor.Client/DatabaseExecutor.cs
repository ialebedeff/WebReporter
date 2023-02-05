using CommunicationMessageWorker;
using Entities;
using Managers;

namespace CommunicationMessageExecutor.Client
{
    public class DatabaseExecutor : MessageExecutor<DatabaseCommand, List<Dictionary<string, object>>>
    {
        public override async Task<List<Dictionary<string, object>>> ExecuteAsync(Message<DatabaseCommand> message)
            => await MessageManager.Instance.Database.ExecuteCommandAsync(message);
    }
}