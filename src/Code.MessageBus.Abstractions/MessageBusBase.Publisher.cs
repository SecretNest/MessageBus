using System;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBusBase
    {
        /// <summary>
        /// Register a publisher with parameter and return value.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="options">The instance of publisher options. Default is <see langword="null"/>.</param>
        /// <returns>Publisher ticket.</returns>
        public abstract PublisherTicket<TParameter, TReturn> RegisterPublisher<TParameter, TReturn>(
            string messageName,
            MessageBusPublisherOptions<TParameter, TReturn>? options = default);

        /// <summary>
        /// Register a publisher with parameter without return value.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="options">The instance of publisher options. Default is <see langword="null"/>.</param>
        /// <returns>Publisher ticket.</returns>
        public abstract PublisherTicket<TParameter> RegisterPublisher<TParameter>(
            string messageName,
            MessageBusPublisherOptions<TParameter>? options = default);


        // ReSharper disable once CommentTypo
        /// <summary>
        /// Unregister a publisher.
        /// </summary>
        /// <param name="publisherTicket">The ticket of the publisher to be unregistered.</param>
        /// <returns><see langword="true"/> when the publisher is located and removed.</returns>
        // ReSharper disable once IdentifierTypo
        public abstract bool UnregisterPublisher(PublisherTicketBase publisherTicket);

        // ReSharper disable once CommentTypo
        /// <summary>
        /// Unregister a publisher.
        /// </summary>
        /// <param name="publisherId">The id of the publisher to be unregistered.</param>
        /// <returns><see langword="true"/> when the publisher is located and removed.</returns>
        // ReSharper disable once IdentifierTypo
        public abstract bool UnregisterPublisher(Guid publisherId);
    }
}
