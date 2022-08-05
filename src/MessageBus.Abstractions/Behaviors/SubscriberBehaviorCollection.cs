using System.Collections.ObjectModel;

namespace SecretNest.MessageBus.Behaviors
{
    /// <summary>
    /// Contains subscriber behaviors.
    /// </summary>
    public class SubscriberBehaviorCollection : ReadOnlyCollection<SubscriberBehaviorBase>
    {
        /// <summary>
        /// Initializes an instance of SubscriberBehaviorCollection.
        /// </summary>
        /// <param name="behaviors">Behaviors.</param>
        public SubscriberBehaviorCollection(IList<SubscriberBehaviorBase> behaviors) : base(behaviors) { }

        /// <summary>
        /// Initializes an instance of SubscriberBehaviorCollection.
        /// </summary>
        /// <param name="behaviors">Behaviors.</param>
        public SubscriberBehaviorCollection(params SubscriberBehaviorBase[] behaviors) : base(behaviors) { }

    }
}
