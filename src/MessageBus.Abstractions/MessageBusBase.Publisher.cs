using SecretNest.MessageBus.Behaviors;

namespace SecretNest.MessageBus
{
    /// <summary>
    /// Defines the methods of MessageBus. This is an abstract class.
    /// </summary>
    public abstract partial class MessageBusBase
    {
        /// <summary>
        /// Register a publisher with parameter and return value.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="behaviors">The instances of publisher behaviors. Default is <see langword="none"/>.</param>
        /// <returns>Publisher ticket.</returns>
        public abstract PublisherTicket<TParameter, TReturn> RegisterPublisher<TParameter, TReturn>(
            string messageName,
            PublisherBehaviorCollection? behaviors = default);

        /// <summary>
        /// Register a publisher with parameter and return value.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="behaviors">The instances of publisher behaviors.</param>
        /// <returns>Publisher ticket.</returns>
        public abstract PublisherTicket<TParameter, TReturn> RegisterPublisher<TParameter, TReturn>(
            string messageName,
            params PublisherBehaviorBase[] behaviors);


        /// <summary>
        /// Register a publisher with parameter without return value.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="behaviors">The instances of publisher behaviors. Default is <see langword="none"/>.</param>
        /// <returns>Publisher ticket.</returns>
        public abstract PublisherTicket<TParameter> RegisterVoidPublisher<TParameter>(
            string messageName,
            PublisherBehaviorCollection? behaviors = default);

        /// <summary>
        /// Register a publisher with parameter without return value.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageName">The name of this message.</param>
        /// <param name="behaviors">The instances of publisher behaviors.</param>
        /// <returns>Publisher ticket.</returns>
        public abstract PublisherTicket<TParameter> RegisterVoidPublisher<TParameter>(
            string messageName,
            params PublisherBehaviorBase[] behaviors);


        // ReSharper disable once CommentTypo
        /// <summary>
        /// Unregister a publisher.
        /// </summary>
        /// <param name="publisherTicket">The ticket of the publisher to be unregistered.</param>
        // ReSharper disable once IdentifierTypo
        public abstract void UnregisterPublisher(PublisherTicketBase publisherTicket);

        // ReSharper disable once CommentTypo
        /// <summary>
        /// Unregister a publisher.
        /// </summary>
        /// <param name="publisherId">The id of the publisher to be unregistered.</param>
        // ReSharper disable once IdentifierTypo
        public abstract void UnregisterPublisher(Guid publisherId);


    }
}
