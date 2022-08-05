using System.Collections.ObjectModel;

namespace SecretNest.MessageBus.Behaviors
{
    /// <summary>
    /// Contains publisher behaviors.
    /// </summary>
    public class PublisherBehaviorCollection : ReadOnlyCollection<PublisherBehaviorBase>
    {
        /// <summary>
        /// Initializes an instance of PublisherBehaviorCollection.
        /// </summary>
        /// <param name="behaviors">Behaviors.</param>
        public PublisherBehaviorCollection(IList<PublisherBehaviorBase> behaviors) : base(behaviors) { }

        /// <summary>
        /// Initializes an instance of PublisherBehaviorCollection.
        /// </summary>
        /// <param name="behaviors">Behaviors.</param>
        public PublisherBehaviorCollection(params PublisherBehaviorBase[] behaviors) : base(behaviors) { }

    }
}
