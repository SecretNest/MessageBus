using SecretNest.MessageBus.Behaviors;

namespace SecretNest.MessageBus
{
    /// <summary>
    /// Represents an subscriber. This is an abstract class.
    /// </summary>
    public abstract class SubscriberTicketBase
    {
        /// <summary>
        /// Gets the id of this publisher.
        /// </summary>
        public abstract Guid Id { get; }

        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        public abstract Type ParameterType { get; }

        /// <summary>
        /// Gets the type of the return value.
        /// </summary>
        public abstract Type ReturnValueType { get; }

        /// <summary>
        /// Gets the collection of the behaviors provided with subscriber registration.
        /// </summary>
        public abstract SubscriberBehaviorCollection Behaviors { get; }

        /// <summary>
        /// Gets whether the delegate is async version.
        /// </summary>
        public abstract bool IsAsync { get; }
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
        public abstract TDelegate Handler { get; }
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
        /// <param name="behaviors">The collection of the behaviors provided with subscriber registration.</param>
        /// <param name="handler">The instance of the delegate.</param>
        public SubscriberTicket(Guid id, bool isAsync, SubscriberBehaviorCollection behaviors, TDelegate handler)
        {
            Id = id;
            IsAsync = isAsync;
            Behaviors = behaviors;
            Handler = handler;
            ParameterType = typeof(TParameter);
            ReturnValueType = typeof(void);
        }

        /// <inheritdoc />
        public override TDelegate Handler { get; }
        /// <inheritdoc />
        public override Guid Id { get; }
        /// <inheritdoc />
        public override Type ParameterType { get; }
        /// <inheritdoc />
        public override Type ReturnValueType { get; }
        /// <inheritdoc />
        public override bool IsAsync { get; }
        /// <inheritdoc />
        public override SubscriberBehaviorCollection Behaviors { get; }
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
        /// <param name="behaviors">The collection of the behaviors provided with subscriber registration.</param>
        /// <param name="handler">The instance of the delegate.</param>
        public SubscriberTicket(Guid id, bool isAsync, SubscriberBehaviorCollection behaviors, TDelegate handler)
        {
            Id = id;
            IsAsync = isAsync;
            Behaviors = behaviors;
            Handler = handler;
            ParameterType = typeof(TParameter);
            ReturnValueType = typeof(TReturn);
        }

        /// <inheritdoc />
        public override TDelegate Handler { get; }
        /// <inheritdoc />
        public override Guid Id { get; }
        /// <inheritdoc />
        public override Type ParameterType { get; }
        /// <inheritdoc />
        public override Type ReturnValueType { get; }
        /// <inheritdoc />
        public override bool IsAsync { get; }
        /// <inheritdoc />
        public override SubscriberBehaviorCollection Behaviors { get; }
    }
}
