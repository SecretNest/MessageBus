namespace SecretNest.MessageBus.Behaviors
{
    interface ITypeConverterBehavior<in TFrom, out TTo>
    {
        /// <summary>
        /// Converts data from <typeparamref name="TFrom"/> to <typeparamref name="TTo"/>.
        /// </summary>
        /// <param name="from">Source data.</param>
        /// <returns>Converted data.</returns>
        TTo Convert(TFrom from);

        /// <summary>
        /// Gets the usage of the converter.
        /// </summary>
        ConverterUsage ConverterUsage { get; }
    }

    /// <summary>
    /// Converts data from <typeparamref name="TFrom"/> to <typeparamref name="TTo"/> for publisher. This is an abstract class.
    /// </summary>
    /// <typeparam name="TFrom">The type of the supported data source.</typeparam>
    /// <typeparam name="TTo">The type of the converted data.</typeparam>
    public abstract class PublisherConverterBehaviorBase<TFrom, TTo> : PublisherBehaviorBase,
        ITypeConverterBehavior<TFrom, TTo>
    {
        /// <inheritdoc />
        public abstract TTo Convert(TFrom from);
        /// <inheritdoc />
        public abstract ConverterUsage ConverterUsage { get; }
    }

    /// <summary>
    /// Converts data from <typeparamref name="TFrom"/> to <typeparamref name="TTo"/> for subscriber. This is an abstract class.
    /// </summary>
    /// <typeparam name="TFrom">The type of the supported data source.</typeparam>
    /// <typeparam name="TTo">The type of the converted data.</typeparam>
    public abstract class SubscriberConverterBehaviorBase<TFrom, TTo> : SubscriberBehaviorBase,
        ITypeConverterBehavior<TFrom, TTo>
    {
        /// <inheritdoc />
        public abstract TTo Convert(TFrom from);
        /// <inheritdoc />
        public abstract ConverterUsage ConverterUsage { get; }
    }

    /// <summary>
    /// Usage of the converter.
    /// </summary>
    [Flags]
    public enum ConverterUsage
    {
        /// <summary>
        /// Indicates the converter cannot be used.
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates the converter is designed to convert argument.
        /// </summary>
        Argument = 1,
        /// <summary>
        /// Indicates the converter is designed to convert return value.
        /// </summary>
        ReturnValue = 2,
        /// <summary>
        /// Indicates the converter is designed to convert both argument and return value.
        /// </summary>
        ArgumentAndReturnValue = 3
    }
}
