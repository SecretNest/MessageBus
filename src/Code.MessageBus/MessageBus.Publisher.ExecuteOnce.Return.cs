using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBus
    {
        /// <inheritdoc />
        public override TReturn? ExecuteOnceWithReturn<TParameter, TReturn>(string messageName, TParameter? argument,
            MessageBusPublisherOptions<TParameter, TReturn>? options = default) where TReturn : default where TParameter : default
        {
            var helper = ExecuteOnceInternal(messageName, argument, options?.ArgumentConvertingCallback, options?.IsAlwaysExecuteAll);
            return ExecuteOnceInternalGetFinalResult(helper, options == null ? default : options.DefaultReturnValue,
                options?.ReturnValueConvertingCallback);
        }

        /// <inheritdoc />
        public override async Task<TReturn?> ExecuteOnceWithReturnAsync<TParameter, TReturn>(string messageName, TParameter? argument,
            MessageBusPublisherOptions<TParameter, TReturn>? options = default,
            CancellationToken? cancellationToken = default) where TReturn : default where TParameter : default
        {
            var cancellationTokenValue = cancellationToken ?? CancellationToken.None;
            cancellationTokenValue.ThrowIfCancellationRequested();
            var helper = await ExecuteOnceInternalAsync(messageName, argument, options?.ArgumentConvertingCallback,
                options?.IsAlwaysExecuteAll, cancellationTokenValue).ConfigureAwait(false);
            cancellationTokenValue.ThrowIfCancellationRequested();
            return ExecuteOnceInternalGetFinalResult(helper, options == null ? default : options.DefaultReturnValue,
                options?.ReturnValueConvertingCallback);
        }

        /// <inheritdoc />
        public override TReturn? ExecuteOnceAndGetMessageInstanceWithReturn<TParameter, TReturn>(string messageName, TParameter? argument,
            out MessageInstance messageInstance, MessageBusPublisherOptions<TParameter, TReturn>? options = default) where TReturn : default where TParameter : default
        {
            var helper = ExecuteOnceInternal(messageName, argument, options?.ArgumentConvertingCallback, options?.IsAlwaysExecuteAll);
            messageInstance = helper.GetMessageInstance();
            return ExecuteOnceInternalGetFinalResult(helper, options == null ? default : options.DefaultReturnValue,
                options?.ReturnValueConvertingCallback);
        }

        /// <inheritdoc />
        public override async Task<MessageInstanceWithExecutorResult<TReturn>> ExecuteOnceAndGetMessageInstanceWithReturnAsync<TParameter, TReturn>(string messageName, TParameter? argument,
            MessageBusPublisherOptions<TParameter, TReturn>? options = default,
            CancellationToken? cancellationToken = default) where TParameter : default
        {
            var cancellationTokenValue = cancellationToken ?? CancellationToken.None;
            cancellationTokenValue.ThrowIfCancellationRequested();
            var helper = await ExecuteOnceInternalAsync(messageName, argument, options?.ArgumentConvertingCallback,
                options?.IsAlwaysExecuteAll, cancellationTokenValue).ConfigureAwait(false);
            cancellationTokenValue.ThrowIfCancellationRequested();
            return new MessageInstanceWithExecutorResult<TReturn>(helper.GetMessageInstance(), 
                ExecuteOnceInternalGetFinalResult(helper, options == null ? default : options.DefaultReturnValue,
                options?.ReturnValueConvertingCallback));
        }
    }
}
