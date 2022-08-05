namespace SecretNest.MessageBus.Behaviors
{
    /// <summary>
    /// Checks the return value of the subscriber to determine whether subsequent subscribers need to be executed. This is an abstract class.
    /// </summary>
    public abstract class SubscriberDealingResultCheckHelperBehaviorBase : SubscriberBehaviorBase
    {
    }

    /// <summary>
    /// Checks the return value of the subscriber to determine whether subsequent subscribers need to be executed. This is an abstract class.
    /// </summary>
    /// <typeparam name="TResult">The type of the return code of the subscriber.</typeparam>
    public abstract class
        SubscriberDealingResultCheckHelperBehaviorBase<TResult> : SubscriberDealingResultCheckHelperBehaviorBase
    {
        /// <summary>
        /// Checks the return value of the subscriber to determine whether subsequent subscribers need to be executed
        /// </summary>
        /// <param name="value">The return code of the subscriber.</param>
        /// <returns>Whether subsequent subscribers need to be executed.</returns>
        public abstract bool ShouldContinue(TResult value);
    }
}
