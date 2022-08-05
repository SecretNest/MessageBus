namespace SecretNest.MessageBus.Behaviors
{
    /// <summary>
    /// Checks the return value of the subscriber to determine whether subsequent subscribers need to be executed. This is an abstract class.
    /// </summary>
    /// <remarks>The default behavior is stop processing subsequent subscribers.</remarks>
    public abstract class SubscriberResultCheckingHelperBehaviorBase : SubscriberBehaviorBase
    {
        /// <summary>
        /// Returns whether subsequent subscribers need to be executed after this subscriber without return value is called.
        /// </summary>
        /// <returns>Whether subsequent subscribers need to be executed.</returns>
        /// <remarks>This method is called after subscriber with no return value is called.</remarks>
        public abstract bool ShouldContinue();
    }

    /// <summary>
    /// Checks the return value of the subscriber to determine whether subsequent subscribers need to be executed. This is an abstract class.
    /// </summary>
    /// <typeparam name="TResult">The type of the return code of the subscriber.</typeparam>
    /// <remarks>The default behavior is stop processing subsequent subscribers.</remarks>
    public abstract class
        SubscriberResultCheckingHelperBehaviorBase<TResult> : SubscriberResultCheckingHelperBehaviorBase
    {
        /// <summary>
        /// Checks the return value of the subscriber to determine whether subsequent subscribers need to be executed.
        /// </summary>
        /// <param name="value">The return code of the subscriber.</param>
        /// <returns>Whether subsequent subscribers need to be executed.</returns>
        /// <remarks>This method is called after value is returned by subscriber delegate.</remarks>
        public abstract bool ShouldContinue(TResult value);
    }
}
