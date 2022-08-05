namespace SecretNest.MessageBus.Behaviors
{
    /// <summary>
    /// Represents a behavior of the subscriber. This is an abstract class.
    /// </summary>
    public abstract class SubscriberBehaviorBase
    {
        protected SubscriberBehaviorBase()
        {
            BehaviorType = GetType();
        }

        /// <summary>
        /// Gets the type of this behavior.
        /// </summary>
        public Type BehaviorType { get; }
    }
}
