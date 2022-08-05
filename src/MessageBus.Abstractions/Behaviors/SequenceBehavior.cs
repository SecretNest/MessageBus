namespace SecretNest.MessageBus.Behaviors
{
    /// <summary>
    /// Specifies the sequence number of this subscriber when executing.
    /// </summary>
    public class SequenceBehavior : SubscriberBehaviorBase
    {
        /// <summary>
        /// Initializes an instance of SequenceBehavior.
        /// </summary>
        /// <param name="sequence">Sequence value.</param>
        public SequenceBehavior(int sequence)
        {
            Sequence = sequence;
        }

        /// <summary>
        /// Gets the sequence value.
        /// </summary>
        public int Sequence { get; }
    }
}
