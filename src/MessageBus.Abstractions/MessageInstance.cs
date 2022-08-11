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

        /// <summary>
        /// Gets whether the accepted return value of this executing is generated from subscriber.
        /// </summary>
        public virtual bool IsSubscriberReturnValueAccepted => false;
    }

    /// <summary>
    /// Contains the return value, the id of this executing and the name of the message. This is an abstract class.
    /// </summary>
    public abstract class MessageInstanceWithReturnValueBase : MessageInstance
    {
        /// <summary>
        /// Gets the type of the return value. <see langword="null" /> when the type of return value cannot be determined, from converter for example.
        /// </summary>
        public abstract Type? SubscriberReturnValueType { get; }

        /// <summary>
        /// Gets the return value of this executing from subscriber.
        /// </summary>
        public abstract object? SubscriberReturnValueGeneric { get; }

        /// <summary>
        /// Gets whether the accepted return value of this executing is generated from subscriber. It set to <see langword="true" />.
        /// </summary>
        public sealed override bool IsSubscriberReturnValueAccepted => true;

        /// <summary>
        /// Gets the source of the return value. <see langword="null"/> when no return value is received from subscriber.
        /// </summary>
        public Guid? ReturnValueSourceSubscriberId { get; }

        protected MessageInstanceWithReturnValueBase(Guid executingId, string messageName, Guid? returnValueSourceSubscriberId) : base(executingId, messageName)
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
        /// <param name="executingId">Id of this executing.</param>
        /// <param name="messageName">Name of the message.</param>
        /// <param name="returnValueSourceSubscriberId">The source of the return value.</param>
        public MessageInstanceWithVoidReturnValue(Guid executingId, string messageName, Guid? returnValueSourceSubscriberId) : base(executingId, messageName, returnValueSourceSubscriberId)
        {
        }

        /// <inheritdoc />
        public override Type? SubscriberReturnValueType => typeof(void);

        /// <inheritdoc />
        public override object? SubscriberReturnValueGeneric => null;
    }

    /// <summary>
    /// Contains the return value from subscriber, the id of this executing and the name of the message.
    /// </summary>
    public class MessageInstanceWithReturnValue : MessageInstanceWithReturnValueBase
    {
        /// <summary>
        /// Gets the return value of this executing from subscriber.
        /// </summary>
        public object? SubscriberReturnValue { get; }

        /// <summary>
        /// Initializes an instance of MessageInstanceWithReturnValue.
        /// </summary>
        /// <param name="executingId">Id of this executing.</param>
        /// <param name="messageName">Name of the message.</param>
        /// <param name="returnValue">Return value of this executing.</param>
        /// <param name="returnValueSourceSubscriberId">The source of the return value.</param>
        public MessageInstanceWithReturnValue(Guid executingId, string messageName, object? returnValue, Guid? returnValueSourceSubscriberId) : base(executingId, messageName, returnValueSourceSubscriberId)
        {
            SubscriberReturnValue = returnValue;
        }

        /// <inheritdoc />
        public override Type? SubscriberReturnValueType => null;

        /// <inheritdoc />
        public override object? SubscriberReturnValueGeneric => SubscriberReturnValue;
    }

    /// <summary>
    /// Contains the return value from subscriber, the id of this executing and the name of the message.
    /// </summary>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    public class MessageInstanceWithReturnValue<TReturn> : MessageInstanceWithReturnValueBase
    {
        /// <summary>
        /// Gets the return value of this executing from subscriber.
        /// </summary>
        public TReturn? SubscriberReturnValue { get; }

        /// <summary>
        /// Initializes an instance of MessageInstanceWithReturnValue.
        /// </summary>
        /// <param name="executingId">Id of this executing.</param>
        /// <param name="messageName">Name of the message.</param>
        /// <param name="returnValue">Return value of this executing.</param>
        /// <param name="returnValueSourceSubscriberId">The source of the return value.</param>
        public MessageInstanceWithReturnValue(Guid executingId, string messageName, TReturn? returnValue, Guid? returnValueSourceSubscriberId) : base(executingId, messageName, returnValueSourceSubscriberId)
        {
            SubscriberReturnValue = returnValue;
        }

        /// <inheritdoc />
        public override Type? SubscriberReturnValueType => typeof(TReturn);

        /// <inheritdoc />
        public override object? SubscriberReturnValueGeneric => SubscriberReturnValue;
    }
}
