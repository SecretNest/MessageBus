using SecretNest.MessageBus.Behaviors;

namespace SecretNest.MessageBus
{
    /// <summary>
    /// Represents an publisher. This is an abstract class.
    /// </summary>
    public abstract class PublisherTicketBase
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
        /// Gets the collection of the behaviors provided with publisher registration.
        /// </summary>
        public abstract PublisherBehaviorCollection Behaviors { get; }
    }

    /// <summary>
    /// Represents an publisher with the type of parameter and return code specified.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TReturn">The type of the return code.</typeparam>
    public class PublisherTicket<TParameter, TReturn> : PublisherTicketBase
    {
        /// <summary>
        /// Initializes an instance of PublisherTicket.
        /// </summary>
        /// <param name="id">The id of this publisher.</param>
        /// <param name="behaviors">The collection of the behaviors provided with publisher registration.</param>
        /// <param name="executor">The instance of <see cref="ExecutorBase{TParameter, TReturn}"/>.</param>
        public PublisherTicket(Guid id, PublisherBehaviorCollection behaviors, ExecutorBase<TParameter, TReturn> executor)
        {
            Id = id;
            Behaviors = behaviors;
            Executor = executor;
            ParameterType = typeof(TParameter);
            ReturnValueType = typeof(TReturn);
        }

        /// <summary>
        /// Gets the instance of <see cref="ExecutorBase{TParameter, TReturn}"/>.
        /// </summary>
        public ExecutorBase<TParameter, TReturn> Executor { get; }

        /// <inheritdoc />
        public override Guid Id { get; }
        /// <inheritdoc />
        public override Type ParameterType { get; }
        /// <inheritdoc />
        public override Type ReturnValueType { get; }
        /// <inheritdoc />
        public override PublisherBehaviorCollection Behaviors { get; }
    }

    /// <summary>
    /// Represents an publisher with the type of parameter specified, without return code.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public class PublisherTicket<TParameter> : PublisherTicketBase
    {
        /// <summary>
        /// Initializes an instance of PublisherTicket.
        /// </summary>
        /// <param name="id">The id of this publisher.</param>
        /// <param name="behaviors">The collection of the behaviors provided with publisher registration.</param>
        /// <param name="executor">The instance of <see cref="ExecutorBase{TParameter}"/>.</param>
        public PublisherTicket(Guid id, PublisherBehaviorCollection behaviors, ExecutorBase<TParameter> executor)
        {
            Id = id;
            Behaviors = behaviors;
            Executor = executor;
            ParameterType = typeof(TParameter);
            ReturnValueType = typeof(void);
        }

        /// <summary>
        /// Gets the instance of <see cref="ExecutorBase{TParameter}"/>.
        /// </summary>
        public ExecutorBase<TParameter> Executor { get; }

        /// <inheritdoc />
        public override Guid Id { get; }
        /// <inheritdoc />
        public override Type ParameterType { get; }
        /// <inheritdoc />
        public override Type ReturnValueType { get; }
        /// <inheritdoc />
        public override PublisherBehaviorCollection Behaviors { get; }

    }
}
