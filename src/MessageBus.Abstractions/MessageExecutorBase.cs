using System.Threading;
using System.Threading.Tasks;

namespace SecretNest.MessageBus
{
    /// <summary>
    /// Handles the executing requests of publisher. This is an abstract class. 
    /// </summary>
    public abstract class MessageExecutorBase
    {
    }

    /// <summary>
    /// Handles the executing requests of publisher. This is an abstract class. 
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public abstract class MessageExecutorBase<TParameter> : MessageExecutorBase
    {
        /// <summary>
        /// Executes with the argument provided.
        /// </summary>
        /// <param name="argument">Argument.</param>
        public abstract void Execute(TParameter argument);

        /// <summary>
        /// Asynchronously executes with the argument provided.
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public abstract Task ExecuteAsync(TParameter? argument, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Executes with the argument provided and get the instance information of this executing.
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="messageInstance">When the method finishes, contains the instance information of this executing.</param>
        public abstract void ExecuteAndGetMessageInstance(TParameter? argument, out MessageInstance messageInstance);

        /// <summary>
        /// Asynchronously executes with the argument provided and get the instance information of this executing.
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation, which wraps the instance information of this executing.</returns>
        public abstract Task<MessageInstance> ExecuteAndGetMessageInstanceAsync(TParameter argument, CancellationToken? cancellationToken = default);
    }

    /// <summary>
    /// Handles the executing requests of publisher. This is an abstract class. 
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    public abstract class MessageExecutorBase<TParameter, TReturn> : MessageExecutorBase
    {
        /// <summary>
        /// Executes with the argument provided and get the return value.
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <returns>Return value.</returns>
        public abstract TReturn? Execute(TParameter? argument);

        /// <summary>
        /// Asynchronously executes with the argument provided and get the return value.
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation, which wraps the return value.</returns>
        public abstract Task<TReturn?> ExecuteAsync(TParameter? argument, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Executes with the argument provided and get the return value with the instance information of this executing.
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="messageInstance">When the method returns, contains the instance information of this executing.</param>
        /// <return>Return value.</return>
        public abstract TReturn? ExecuteAndGetMessageInstance(TParameter? argument, out MessageInstance messageInstance);

        /// <summary>
        /// Asynchronously executes with the argument provided and get the return value with the instance information of this executing.
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see langword="null"/>.</param>
        /// <return>A task that represents the asynchronous operation, which wraps the return value with the instance information of this executing.</return>
        public abstract Task<MessageInstanceWithExecutorResult<TReturn>> ExecuteAndGetMessageInstanceAsync(TParameter? argument, CancellationToken? cancellationToken = default);
    }

    /// <summary>
    /// Contains the return value for publisher and the instance information of this executing.
    /// </summary>
    /// <typeparam name="TReturn"></typeparam>
    public class MessageInstanceWithExecutorResult<TReturn>
    {
        /// <summary>
        /// Gets the instance information of this executing.
        /// </summary>
        public MessageInstance MessageInstance { get; }

        /// <summary>
        /// Gets the return value for publisher.
        /// </summary>
        public TReturn? ReturnValue { get; }

        /// <summary>
        /// Initializes an instance of MessageInstanceWithExecutorResult.
        /// </summary>
        /// <param name="messageInstance">The instance information of this executing.</param>
        /// <param name="returnValue">The return value for publisher.</param>
        public MessageInstanceWithExecutorResult(MessageInstance messageInstance, TReturn? returnValue)
        {
            MessageInstance = messageInstance;
            ReturnValue = returnValue;
        }
    }
}