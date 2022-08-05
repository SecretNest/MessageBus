namespace SecretNest.MessageBus.Behaviors
{
    /// <summary>
    /// Represents a behavior of the publisher. This is an abstract class.
    /// </summary>
    public abstract class PublisherBehaviorBase
    {
        protected PublisherBehaviorBase()
        {
            BehaviorType = GetType();
        }

        /// <summary>
        /// Gets the type of this behavior.
        /// </summary>
        public Type BehaviorType { get; }

    }
}
