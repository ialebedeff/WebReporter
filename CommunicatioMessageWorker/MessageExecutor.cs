using Entities;

namespace CommunicationMessageWorker
{
    public class MessageExecutor<TInput, TOutput>
    {
        public virtual Task<TOutput> ExecuteAsync(Message<TInput> message)
            => Task.FromResult<TOutput>(default!);
    }
    public class MessageExecutor<TInput>
    {

    }
}