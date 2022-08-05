namespace SecretNest.MessageBus.Behaviors
{
    /// <summary>
    /// Specifies the default return value for publisher when no subscriber executed with accepted value returns. This is an abstract class.
    /// </summary>
    public abstract class PublisherDefaultReturnBehaviorBase : PublisherBehaviorBase
    {
    }

    /// <summary>
    /// Specifies the default return value for publisher when no subscriber executed with accepted value returns.
    /// </summary>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    public class PublisherDefaultReturnBehavior<TReturn> : PublisherDefaultReturnBehaviorBase
    {
        /// <summary>
        /// Initializes an instance of PublisherDefaultReturnBehavior.
        /// </summary>
        /// <param name="defaultReturn">The default return value.</param>
        public PublisherDefaultReturnBehavior(TReturn defaultReturn)
        {
            DefaultReturn = defaultReturn;
        }

        /// <summary>
        /// Gets the default return value.
        /// </summary>
        public TReturn DefaultReturn { get; }
    }
}
