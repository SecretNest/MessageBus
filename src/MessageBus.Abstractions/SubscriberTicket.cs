using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    /// <summary>
    /// Represents an subscriber. This is an abstract class.
    /// </summary>
    public abstract class SubscriberTicketBase
    {
        /// <summary>
        /// Gets the id of this subscriber.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        public abstract Type ParameterType { get; }

        /// <summary>
        /// Gets the type of the return value.
        /// </summary>
        public abstract Type ReturnValueType { get; }

        /// <summary>
        /// Gets the generic instance of subscriber options provided with subscriber registration.
        /// </summary>
        public abstract MessageBusSubscriberOptionsBase OptionsGeneric { get; }

        /// <summary>
        /// Gets whether the delegate is async version.
        /// </summary>
        public bool IsAsync { get; }

        /// <summary>
        /// Gets the instance of message name matcher.
        /// </summary>
        public MessageNameMatcherBase MessageNameMatcher { get; }

        /// <summary>
        /// Initializes an instance of SubscriberTicketBase.
        /// </summary>
        /// <param name="id">The id of this subscriber.</param>
        /// <param name="isAsync">Whether the delegate is async version.</param>
        protected SubscriberTicketBase(Guid id, bool isAsync)
        {
            Id = id;
            IsAsync = isAsync;
        }
    }

    /// <summary>
    /// Represents an subscriber, containing the delegate. This is an abstract class.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    public abstract class SubscriberTicketBase<TDelegate> : SubscriberTicketBase
    {
        /// <summary>
        /// Gets the instance of the delegate.
        /// </summary>
        public TDelegate Handler { get; }

        /// <summary>
        /// Initializes an instance of SubscriberTicketBase.
        /// </summary>
        /// <param name="id">The id of this subscriber.</param>
        /// <param name="isAsync">Whether the delegate is async version.</param>
        /// <param name="handler">The instance of the delegate.</param>
        protected SubscriberTicketBase(Guid id, bool isAsync, TDelegate handler) : base(id, isAsync)
        {
            Handler = handler;
        }
    }
    
    /// <summary>
    /// Represents an subscriber with the type of parameter and delegate specified.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    public class SubscriberTicket<TParameter, TDelegate> : SubscriberTicketBase<TDelegate>
    {
        /// <summary>
        /// Initializes an instance of SubscriberTicket.
        /// </summary>
        /// <param name="id">The id of this subscriber.</param>
        /// <param name="isAsync">Whether the delegate is async version.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="none"/>.</param>
        /// <param name="handler">The instance of the delegate.</param>
        public SubscriberTicket(Guid id, bool isAsync, MessageBusSubscriberOptions<TParameter> options, TDelegate handler)
        : base(id, isAsync, handler)
        {
            Options = options;
            ParameterType = typeof(TParameter);
            ReturnValueType = typeof(void);
        }

        /// <inheritdoc />
        public override Type ParameterType { get; }
        /// <inheritdoc />
        public override Type ReturnValueType { get; }
        /// <inheritdoc />
        public override MessageBusSubscriberOptionsBase OptionsGeneric => Options;

        public MessageBusSubscriberOptions<TParameter> Options { get; }
    }

    /// <summary>
    /// Represents an subscriber with the type of parameter, return code, and delegate specified.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TReturn">The type of the return code.</typeparam>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    public class SubscriberTicket<TParameter, TReturn, TDelegate> : SubscriberTicketBase<TDelegate>
    {
        /// <summary>
        /// Initializes an instance of SubscriberTicket.
        /// </summary>
        /// <param name="id">The id of this subscriber.</param>
        /// <param name="isAsync">Whether the delegate is async version.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="none"/>.</param>
        /// <param name="handler">The instance of the delegate.</param>
        public SubscriberTicket(Guid id, bool isAsync, MessageBusSubscriberOptions<TParameter, TReturn> options, TDelegate handler)
            : base(id, isAsync, handler)
        {
            Options = options;
            ParameterType = typeof(TParameter);
            ReturnValueType = typeof(TReturn);
        }

        /// <inheritdoc />
        public override Type ParameterType { get; }
        /// <inheritdoc />
        public override Type ReturnValueType { get; }
        /// <inheritdoc />
        public override MessageBusSubscriberOptionsBase OptionsGeneric => Options;

        public MessageBusSubscriberOptions<TParameter, TReturn> Options { get; }
    }
}
