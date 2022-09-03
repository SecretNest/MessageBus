using System;
using System.Threading.Tasks;
using System.Threading;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBusBase
    {
        /// <summary>
        /// Executes as publisher specified by message name with the argument provided.
        /// </summary>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="argument">Argument.</param>
        /// <param name="options">The instance of publisher options. Default is <see langword="null"/>.</param>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        public abstract void ExecuteOnce<TParameter>(string messageName, TParameter? argument, MessageBusPublisherOptions<TParameter>? options = default);

        /// <summary>
        /// Asynchronously executes as publisher specified by message name with the argument provided.
        /// </summary>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="argument">Argument.</param>
        /// <param name="options">The instance of publisher options. Default is <see langword="null"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        public abstract Task ExecuteOnceAsync<TParameter>(string messageName, TParameter? argument, MessageBusPublisherOptions<TParameter>? options = default, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Executes as publisher specified by message name with the argument provided and get the instance information of this executing.
        /// </summary>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="argument">Argument.</param>
        /// <param name="messageInstance">When the method finishes, contains the instance information of this executing.</param>
        /// <param name="options">The instance of publisher options. Default is <see langword="null"/>.</param>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        public abstract void ExecuteOnceAndGetMessageInstance<TParameter>(string messageName, TParameter? argument, out MessageInstance messageInstance, MessageBusPublisherOptions<TParameter>? options = default);

        /// <summary>
        /// Asynchronously executes as publisher specified by message name with the argument provided and get the instance information of this executing.
        /// </summary>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="argument">Argument.</param>
        /// <param name="options">The instance of publisher options. Default is <see langword="null"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation, which wraps the instance information of this executing.</returns>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        public abstract Task<MessageInstance> ExecuteOnceAndGetMessageInstanceAsync<TParameter>(string messageName, TParameter? argument, MessageBusPublisherOptions<TParameter>? options = default, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Executes as publisher specified by message name with the argument provided and get the return value.
        /// </summary>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="argument">Argument.</param>
        /// <param name="options">The instance of publisher options. Default is <see langword="null"/>.</param>
        /// <returns>Return value.</returns>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        public abstract TReturn? ExecuteOnceWithReturn<TParameter, TReturn>(string messageName, TParameter? argument, MessageBusPublisherOptions<TParameter, TReturn>? options = default);

        /// <summary>
        /// Asynchronously executes as publisher specified by message name with the argument provided and get the return value.
        /// </summary>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="argument">Argument.</param>
        /// <param name="options">The instance of publisher options. Default is <see langword="null"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation, which wraps the return value.</returns>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        public abstract Task<TReturn?> ExecuteOnceWithReturnAsync<TParameter, TReturn>(string messageName, TParameter? argument, MessageBusPublisherOptions<TParameter, TReturn>? options = default, CancellationToken? cancellationToken = default);

        /// <summary>
        /// Executes as publisher specified by message name with the argument provided and get the return value with the instance information of this executing.
        /// </summary>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="argument">Argument.</param>
        /// <param name="messageInstance">When the method returns, contains the instance information of this executing.</param>
        /// <param name="options">The instance of publisher options. Default is <see langword="null"/>.</param>
        /// <returns>Return value.</returns>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        public abstract TReturn? ExecuteOnceAndGetMessageInstanceWithReturn<TParameter, TReturn>(string messageName, TParameter? argument, out MessageInstance messageInstance, MessageBusPublisherOptions<TParameter, TReturn>? options = default);

        /// <summary>
        /// Asynchronously executes as publisher specified by message name with the argument provided and get the return value with the instance information of this executing.
        /// </summary>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="argument">Argument.</param>
        /// <param name="options">The instance of publisher options. Default is <see langword="null"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see langword="null"/>.</param>
        /// <returns>A task that represents the asynchronous operation, which wraps the return value with the instance information of this executing.</returns>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        public abstract Task<MessageInstanceWithExecutorResult<TReturn>> ExecuteOnceAndGetMessageInstanceWithReturnAsync<TParameter, TReturn>(string messageName, TParameter? argument, MessageBusPublisherOptions<TParameter, TReturn>? options = default, CancellationToken? cancellationToken = default);
    }
}
