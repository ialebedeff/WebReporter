using Entities;
using Microsoft.AspNetCore.SignalR.Client;

namespace CommunicationMessageWorker
{
    public class MessageHandler<TInput> where TInput : class
    {
        public string Method { get; private set; }
        public HubConnection Connection { get; private set; }
        public MessageHandler(
            string serverMethodName,
            HubConnection hubConnection)
        {
            Method = serverMethodName;
            Connection = hubConnection;
        }
        public virtual Task ExecuteAsync(Message<TInput> message)
            => Task.CompletedTask;
    }
    public class MessageHandler<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        public string Method { get; private set; }
        private HubConnection Connection { get; set; }
        public MessageHandler(
            string serverMethodName,
            HubConnection hubConnection)
        {
            Method = serverMethodName;
            Connection = hubConnection;
        }
        public virtual Task ExecuteAsync(Message<TInput> message)
            => Task.CompletedTask;
        public virtual async Task SendAsync(TOutput output, string messageId)
            => await Connection.SendAsync(Method, new Message<TOutput>(output) { MessageId = messageId });
    }
}