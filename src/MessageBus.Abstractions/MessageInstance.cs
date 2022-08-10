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
        /// <summary>
        /// Gets the type of the return value. <see langword="null" /> when the type of return value cannot be determined, from converter for example.
        /// </summary>
        public abstract Type? ReturnValueType { get; }

        /// <summary>
        /// Gets the return value of this executing.
        /// </summary>
        public abstract object? ReturnValueGeneric { get; }

        /// <summary>
        /// Gets the source of the return value. <see langword="null"/> when no return value is received from subscriber.
        /// </summary>
        public Guid? ReturnValueSourceSubscriberId { get; }

        protected MessageInstanceWithReturnValueBase(MessageInstance messageInstance, Guid? returnValueSourceSubscriberId) : base(messageInstance.ExecutingId, messageInstance.MessageName)
        {
            ReturnValueSourceSubscriberId = returnValueSourceSubscriberId;
        }
    }

    /// <summary>
    /// Contains the the id of this executing and the name of the message. Uses when void returns.
    /// </summary>
    public class MessageInstanceWithVoidReturnValue : MessageInstanceWithReturnValueBase
    {
        /// <summary>
        /// Initializes an instance of MessageInstanceWithReturnValue.
        /// </summary>
        /// <param name="messageInstance">The instance information of this executing.</param>
        /// <param name="returnValueSourceSubscriberId">The source of the return value.</param>
        public MessageInstanceWithVoidReturnValue(MessageInstance messageInstance, Guid? returnValueSourceSubscriberId) : base(messageInstance, returnValueSourceSubscriberId)
        {
        }

        /// <inheritdoc />
        public override Type? ReturnValueType => typeof(void);

        /// <inheritdoc />
        public override object? ReturnValueGeneric => null;
    }

    /// <summary>
    /// Contains the return value, the id of this executing and the name of the message.
    /// </summary>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    public class MessageInstanceWithReturnValue : MessageInstanceWithReturnValueBase
    {
        /// <summary>
        /// Gets the return value of this executing.
        /// </summary>
        public object? ReturnValue { get; }

        /// <summary>
        /// Initializes an instance of MessageInstanceWithReturnValue.
        /// </summary>
        /// <param name="messageInstance">The instance information of this executing.</param>
        /// <param name="returnValue">Return value of this executing.</param>
        /// <param name="returnValueSourceSubscriberId">The source of the return value.</param>
        public MessageInstanceWithReturnValue(MessageInstance messageInstance, object? returnValue, Guid? returnValueSourceSubscriberId) : base(messageInstance, returnValueSourceSubscriberId)
        {
            ReturnValue = returnValue;
        }

        /// <inheritdoc />
        public override Type? ReturnValueType => null;

        /// <inheritdoc />
        public override object? ReturnValueGeneric => ReturnValue;
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
        /// <param name="returnValueSourceSubscriberId">The source of the return value.</param>
        public MessageInstanceWithReturnValue(MessageInstance messageInstance, TReturn? returnValue, Guid? returnValueSourceSubscriberId) : base(messageInstance, returnValueSourceSubscriberId)
        {
            ReturnValue = returnValue;
        }

        /// <inheritdoc />
        public override Type? ReturnValueType => typeof(TReturn);

        /// <inheritdoc />
        public override object? ReturnValueGeneric => ReturnValue;
    }
}
