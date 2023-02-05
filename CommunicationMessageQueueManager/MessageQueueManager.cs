using CommunicationService;
using Entities;
using Microsoft.AspNetCore.SignalR.Client;

namespace CommunicationMessageQueueManager
{
    public class MessageQueueManager
    {
        public static bool IsInitialized => Instance != null;
        public static MessageQueueManager? Instance { get; private set; }
        public MessageQueueManager(SignalClient signalClient)
        {
            SignalClient = signalClient;
            UnworkedMessages = new Dictionary<string, object>();
            Instance = this;
        }
        public Dictionary<string, object> UnworkedMessages { get; set; }
        public SignalClient SignalClient { get; private set; }
        public async Task SendAsync<TInput>(string methodName, TInput message)
            where TInput : class => await SignalClient.HubConnection.SendAsync(methodName, new Message<TInput>(message));
        public async Task<Message<TOutput>?> LongPollAsync<TInput, TOutput>(string methodName, TInput input)
            where TInput : class
            where TOutput : class
        {
            using (CancellationTokenSource source = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
            {
                var message = new Message<TInput>(input);

                await SignalClient.HubConnection.SendAsync(methodName, message);

                return await WaitForResponseAsync<TOutput>(message.MessageId, source.Token);
            }
        }
        public void InsertUnworkedMessage<TInput>(Message<TInput> message)
            => UnworkedMessages.Add(message.MessageId, message);
        private async Task<Message<TOutput>?> WaitForResponseAsync<TOutput>(string messageId, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (UnworkedMessages.ContainsKey(messageId))
                {
                    var message = UnworkedMessages[messageId];

                    UnworkedMessages.Remove(messageId);

                    return (Message<TOutput>)message;
                }

                await Task.Delay(25);
            }

            return null;
        }
    }
}