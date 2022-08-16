using System;
using SecretNest.MessageBus.Options;

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
        /// Gets the generic instance of publisher options provided with publisher registration.
        /// </summary>
        public abstract MessageBusPublisherOptionsBase? OptionsGeneric { get; }

        /// <summary>
        /// Initializes an instance of PublisherTicketBase.
        /// </summary>
        /// <param name="id">The id of this publisher.</param>
        protected PublisherTicketBase(Guid id)
        {
            Id = id;
        }
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
        /// <param name="options">The instance of publisher options. Default is <see langword="null"/>.</param>
        /// <param name="executor">The instance of <see cref="MessageExecutorBase{TParameter}"/>.</param>
        public PublisherTicket(Guid id, MessageBusPublisherOptions<TParameter>? options,
            MessageExecutorBase<TParameter> executor)
            : base(id)
        {
            Options = options;
            Executor = executor;
            ParameterType = typeof(TParameter);
            ReturnValueType = typeof(void);
        }

        /// <summary>
        /// Gets the instance of <see cref="MessageExecutorBase{TParameter}"/>.
        /// </summary>
        public MessageExecutorBase<TParameter> Executor { get; }

        /// <inheritdoc />
        public override Type ParameterType { get; }
        /// <inheritdoc />
        public override Type ReturnValueType { get; }
        /// <inheritdoc />
        public override MessageBusPublisherOptionsBase? OptionsGeneric => Options;

        /// <summary>
        /// Gets the instance of publisher options provided with publisher registration.
        /// </summary>
        public MessageBusPublisherOptions<TParameter>? Options { get; }
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
        /// <param name="options">The instance of publisher options. Default is <see langword="null"/>.</param>
        /// <param name="executor">The instance of <see cref="MessageExecutorBase{TParameter, TReturn}"/>.</param>
        public PublisherTicket(Guid id, MessageBusPublisherOptions<TParameter, TReturn>? options,
            MessageExecutorBase<TParameter, TReturn> executor)
            : base(id)
        {
            Options = options;
            Executor = executor;
            ParameterType = typeof(TParameter);
            ReturnValueType = typeof(TReturn);
        }

        /// <summary>
        /// Gets the instance of <see cref="MessageExecutorBase{TParameter, TReturn}"/>.
        /// </summary>
        public MessageExecutorBase<TParameter, TReturn> Executor { get; }

        /// <inheritdoc />
        public override Type ParameterType { get; }
        /// <inheritdoc />
        public override Type ReturnValueType { get; }
        /// <inheritdoc />
        public override MessageBusPublisherOptionsBase? OptionsGeneric => Options;

        /// <summary>
        /// Gets the instance of publisher options provided with publisher registration.
        /// </summary>
        public MessageBusPublisherOptions<TParameter, TReturn>? Options { get; }
    }

}
