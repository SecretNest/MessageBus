using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus.Options
{
    /// <summary>
    /// Stores options that configure the subscriber operation of MessageBus.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public class MessageBusSubscriberOptions<TParameter> : MessageBusOptionsBase<TParameter>
    {
        /// <summary>
        /// Gets the sequence number of this subscriber when executing.
        /// </summary>
        public int Sequence { get; }

        /// <summary>
        /// Gets whether the subscriber should be executed regardless of the result of the subscribers those have been executed by this instance.
        /// </summary>
        public bool IsAlwaysExecution { get; }

        /// <summary>
        /// Gets whether message should be returned instead of executing subsequent subscribers.
        /// </summary>
        /// <remarks>Default value is <see langword="true"/>.</remarks>
        public virtual bool IsFinal { get; }

        /// <summary>
        /// Gets the callback for parameter conversion.
        /// </summary>
        /// <remarks>If present, the callback is called to convert the parameter the before subscriber executing. Default value is <see langword="none"/>.</remarks>
        public Func<object, TParameter>? ParameterConvertingCallback { get; }

        /// <summary>
        /// Initializes an instance of MessageBusSubscriberOptions.
        /// </summary>
        /// <param name="sequence">The sequence number of this subscriber when executing. Default value is 0.</param>
        /// <param name="isAlwaysExecution">Whether the subscriber should be executed regardless of the result of the subscribers those have been executed by this instance. Default value is <see langword="false"/>.</param>
        /// <param name="isFinal">Whether message should be returned instead of executing subsequent subscribers. Default value is <see langword="true"/>.</param>
        /// <param name="parameterConvertingCallback">The callback for parameter conversion.</param>
        public MessageBusSubscriberOptions(
            int sequence = 0,
            bool isAlwaysExecution = false,
            bool isFinal = true,
            Func<object, TParameter>? parameterConvertingCallback = null)
        {
            Sequence = sequence;
            IsAlwaysExecution = isAlwaysExecution;
            IsFinal = isFinal;
            ParameterConvertingCallback = parameterConvertingCallback;
        }
    }

    /// <summary>
    /// Stores options that configure the subscriber operation of MessageBus.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    public class MessageBusSubscriberOptions<TParameter, TReturn> : MessageBusSubscriberOptions<TParameter>
    {
        /// <summary>
        /// Gets the callback for checking result.
        /// </summary>
        /// <remarks>If present, the callback is called to check whether message should be returned instead of executing subsequent subscribers. Default value is <see langword="none"/> which returns <see langword="true"/> for all return values.</remarks>
        public Func<TReturn, bool>? ResultCheckingCallback { get; }

        /// <summary>
        /// Gets the callback for return value conversion.
        /// </summary>
        /// <remarks>If present, the callback is called to convert the return value before returning to publisher. Default value is <see langword="none"/>.</remarks>
        public Func<TReturn, object>? ReturnValueConvertingCallback { get; }

        /// <summary>
        /// Initializes an instance of MessageBusSubscriberOptions.
        /// </summary>
        /// <param name="sequence">The sequence number of this subscriber when executing. Default value is 0.</param>
        /// <param name="isAlwaysExecution">Whether the subscriber should be executed regardless of the result of the subscribers those have been executed by this instance. Default value is <see langword="false"/>.</param>
        /// <param name="resultCheckingCallback">The callback for checking result.</param>
        /// <param name="parameterConvertingCallback">The callback for parameter conversion.</param>
        /// <param name="returnValueConvertingCallback">The callback for return value conversion.</param>
        public MessageBusSubscriberOptions(
            int sequence = 0,
            bool isAlwaysExecution = false,
            Func<TReturn, bool>? resultCheckingCallback = null,
            Func<object, TParameter>? parameterConvertingCallback = null,
            Func<TReturn, object>? returnValueConvertingCallback = null)
            : base(sequence, isAlwaysExecution, resultCheckingCallback == null, parameterConvertingCallback)
        {
            ResultCheckingCallback = resultCheckingCallback;
            ReturnValueConvertingCallback = returnValueConvertingCallback;
        }
    }
}
