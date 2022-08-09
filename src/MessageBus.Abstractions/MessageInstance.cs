namespace SecretNest.MessageBus
{
    /// <summary>
    /// Contains the id of this executing and the name of the message.
    /// </summary>
    public class MessageInstance
    {
        /// <summary>
        /// Initializes and instance of MessageInstance.
        /// </summary>
        /// <param name="executingId">Id of this executing.</param>
        /// <param name="messageName">Name of the message.</param>
        public MessageInstance(Guid executingId, string messageName)
        {
            ExecutingId = executingId;
            MessageName = messageName;
        }

        /// <summary>
        /// Gets the id of this executing.
        /// </summary>
        public Guid ExecutingId { get; }

        /// <summary>
        /// Gets the name of the message.
        /// </summary>
        public string MessageName { get; }
    }

    /// <summary>
    /// Contains the return value, the id of this executing and the name of the message. This is an abstract class.
    /// </summary>
    public abstract class MessageInstanceWithReturnValueBase : MessageInstance
    {
        protected MessageInstanceWithReturnValueBase(MessageInstance messageInstance) : base(messageInstance.ExecutingId, messageInstance.MessageName)
        {
        }
    }

    /// <summary>
    /// Contains the return value, the id of this executing and the name of the message.
    /// </summary>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    public class MessageInstanceWithReturnValue<TReturn> : MessageInstanceWithReturnValueBase
    {
        /// <summary>
        /// Gets the return value of this executing.
        /// </summary>
        public TReturn? ReturnValue { get; }

        /// <summary>
        /// Initializes an instance of MessageInstanceWithReturnValue.
        /// </summary>
        /// <param name="messageInstance">The instance information of this executing.</param>
        /// <param name="returnValue">Return value of this executing.</param>
        public MessageInstanceWithReturnValue(MessageInstance messageInstance, TReturn? returnValue) : base(messageInstance)
        {
            ReturnValue = returnValue;
        }
    }
}
