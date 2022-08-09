using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus.Options
{

    /// <summary>
    /// Stores options that configure the publisher operation of MessageBus. This is an abstract class.
    /// </summary>
    public abstract class MessageBusPublisherOptionsBase
    {
        /// <summary>
        /// Gets whether all subscribers should be executed regardless of the result of the subscribers those have been executed by this instance.
        /// </summary>
        public bool IsAlwaysExecuteAll { get; }

        /// <summary>
        /// Initializes an instance of MessageBusPublisherOptionsBase.
        /// </summary>
        /// <param name="isAlwaysExecuteAll">Whether all subscribers should be executed regardless of the result of the subscribers those have been executed by this instance.</param>
        protected MessageBusPublisherOptionsBase(bool isAlwaysExecuteAll)
        {
            IsAlwaysExecuteAll = isAlwaysExecuteAll;
        }
    }

    /// <summary>
    /// Stores options that configure the publisher operation of MessageBus.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public class MessageBusPublisherOptions<TParameter> : MessageBusPublisherOptionsBase
    {
        /// <summary>
        /// Gets the callback for argument conversion.
        /// </summary>
        /// <remarks>If present, the callback is called to convert the argument the when publisher executing. Default value is <see langword="none"/>.</remarks>
        public Func<TParameter?, object?>? ArgumentConvertingCallback { get; }

        /// <summary>
        /// Initializes an instance of MessageBusPublisherOptions.
        /// </summary>
        /// <param name="isAlwaysExecuteAll">Whether all subscribers should be executed regardless of the result of the subscribers those have been executed by this instance.</param>
        /// <param name="argumentConvertingCallback">The callback for argument conversion.</param>
        public MessageBusPublisherOptions(
            bool isAlwaysExecuteAll = false, 
            Func<TParameter?, object?>? argumentConvertingCallback = null) 
            : base(isAlwaysExecuteAll)
        {
            ArgumentConvertingCallback = argumentConvertingCallback;
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
        public TReturn? DefaultReturnValue { get; }

        /// <summary>
        /// Gets the callback for return value conversion.
        /// </summary>
        /// <remarks>If present, the callback is called to convert the return value before publisher returning. The value provided by <see cref="DefaultReturnValue"/> is exempted from being converted by this callback. Default value is <see langword="none"/>.</remarks>
        public Func<object?, TReturn?>? ReturnValueConvertingCallback { get; }


        /// <summary>
        /// Initializes an instance of MessageBusPublisherOptions.
        /// </summary>
        /// <param name="defaultReturnValue">The default value for publisher returning.</param>
        /// <param name="isAlwaysExecuteAll">Whether all subscribers should be executed regardless of the result of the subscribers those have been executed by this instance.</param>
        /// <param name="argumentConvertingCallback">The callback for argument conversion.</param>
        /// <param name="returnValueConvertingCallback">The callback for return value conversion.</param>
        public MessageBusPublisherOptions(
            TReturn? defaultReturnValue = default,
            bool isAlwaysExecuteAll = false, 
            Func<TParameter?, object?>? argumentConvertingCallback = null,
            Func<object?, TReturn?>? returnValueConvertingCallback = null)
            : base(isAlwaysExecuteAll, argumentConvertingCallback)
        {
            DefaultReturnValue = defaultReturnValue;
            ReturnValueConvertingCallback = returnValueConvertingCallback;
        }
    }
}
