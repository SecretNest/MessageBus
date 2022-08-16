namespace SecretNest.MessageBus.MessageNameMatching
{
    /// <summary>
    /// Checks whether the message name specified by publisher registering process is complied with the subscriber required. This is an abstract class.
    /// </summary>
    public abstract class MessageNameMatcherBase
    {
        /// <summary>
        /// Gets the message name or pattern of the subscriber.
        /// </summary>
        public abstract string SubscriberMessageNamePattern { get; }

        /// <summary>
        /// Checks whether the message name specified by publisher registering process is complied with the subscriber required.
        /// </summary>
        /// <param name="publisherMessageName">The message name specified by publisher registering process.</param>
        /// <returns>Whether the message name is complied.</returns>
        public abstract bool IsComplied(string publisherMessageName);
    }
}
