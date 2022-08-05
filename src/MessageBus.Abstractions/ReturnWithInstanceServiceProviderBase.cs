namespace SecretNest.MessageBus
{
    /// <summary>
    /// Contains the instance information of this executing. This is an abstract class.
    /// </summary>
    public abstract class ReturnWithInstanceServiceProviderBase
    {
        /// <summary>
        /// Gets the instance information of this executing.
        /// </summary>
        public abstract MessageInstance MessageInstance { get; }
    }

    /// <summary>
    /// Contains the return value and the instance information of this executing. This is an abstract class.
    /// </summary>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    public abstract class ReturnWithInstanceServiceProviderBase<TReturn> : ReturnWithInstanceServiceProviderBase
    {
        /// <summary>
        /// Gets the return value of this executing.
        /// </summary>
        public abstract TReturn InstanceReturn { get; }
    }
}
