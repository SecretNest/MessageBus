using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus.Options
{

    /// <summary>
    /// Stores options that configure the publisher operation of MessageBus. This is an abstract class.
    /// </summary>
    public class MessageBusPublisherOptionsBase
    {

    }

    /// <summary>
    /// Stores options that configure the publisher operation of MessageBus.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public class MessageBusPublisherOptions<TParameter> : MessageBusPublisherOptionsBase
    {
        /// <summary>
        /// Gets the callback for parameter conversion.
        /// </summary>
        /// <remarks>If present, the callback is called to convert the parameter the when publisher executing. Default value is <see langword="none"/>.</remarks>
        public Func<TParameter, object>? ParameterConvertingCallback { get; }

        /// <summary>
        /// Initializes an instance of MessageBusPublisherOptions.
        /// </summary>
        /// <param name="parameterConvertingCallback">The callback for parameter conversion.</param>
        public MessageBusPublisherOptions(
            Func<TParameter, object>? parameterConvertingCallback = null)
        {
            ParameterConvertingCallback = parameterConvertingCallback;
        }
    }

    /// <summary>
    /// Stores options that configure the publisher operation of MessageBus.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    public class MessageBusPublisherOptions<TParameter, TReturn> : MessageBusPublisherOptions<TParameter>
    {
        /// <summary>
        /// Gets the default value for publisher returning.
        /// </summary>
        /// <remarks>The value is returned when no subscriber executed with accepted value returns.</remarks>
        public TReturn DefaultReturn { get; }

        /// <summary>
        /// Gets the callback for return value conversion.
        /// </summary>
        /// <remarks>If present, the callback is called to convert the return value before publisher returning. The value provided by <see cref="DefaultReturn"/> is exempted from being converted by this callback. Default value is <see langword="none"/>.</remarks>
        public Func<object, TReturn>? ReturnValueConvertingCallback { get; }


        /// <summary>
        /// Initializes an instance of MessageBusPublisherOptions.
        /// </summary>
        /// <param name="defaultReturn">The default value for publisher returning.</param>
        /// <param name="parameterConvertingCallback">The callback for parameter conversion.</param>
        /// <param name="returnValueConvertingCallback">The callback for return value conversion.</param>
        public MessageBusPublisherOptions(
            TReturn defaultReturn = default!,
            Func<TParameter, object>? parameterConvertingCallback = null,
            Func<object, TReturn>? returnValueConvertingCallback = null)
            : base(parameterConvertingCallback)
        {
            DefaultReturn = defaultReturn;
            ReturnValueConvertingCallback = returnValueConvertingCallback;
        }
    }
}
