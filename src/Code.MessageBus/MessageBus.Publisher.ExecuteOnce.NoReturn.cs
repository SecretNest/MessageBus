using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBus
    {
        /// <inheritdoc />
        public override void ExecuteOnce<TParameter>(string messageName, TParameter? argument, 
            MessageBusPublisherOptions<TParameter>? options = default) where TParameter : default
        {
            ExecuteOnceInternal(messageName, argument, options?.ArgumentConvertingCallback, options?.IsAlwaysExecuteAll);
        }

        /// <inheritdoc />
        public override async Task ExecuteOnceAsync<TParameter>(string messageName, TParameter? argument, 
            MessageBusPublisherOptions<TParameter>? options = default,
            CancellationToken? cancellationToken = default) where TParameter : default
        {
            var cancellationTokenValue = cancellationToken ?? CancellationToken.None;
            cancellationTokenValue.ThrowIfCancellationRequested();
            await ExecuteOnceInternalAsync(messageName, argument, options?.ArgumentConvertingCallback,
                options?.IsAlwaysExecuteAll, cancellationTokenValue).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override void ExecuteOnceAndGetMessageInstance<TParameter>(string messageName, TParameter? argument,
            out MessageInstance messageInstance, MessageBusPublisherOptions<TParameter>? options = default) where TParameter : default
        {
            var helper = ExecuteOnceInternal(messageName, argument, options?.ArgumentConvertingCallback, options?.IsAlwaysExecuteAll);
            messageInstance = helper.GetMessageInstance();
        }

        /// <inheritdoc />
        public override async Task<MessageInstance> ExecuteOnceAndGetMessageInstanceAsync<TParameter>(string messageName, TParameter? argument,
            MessageBusPublisherOptions<TParameter>? options = default, CancellationToken? cancellationToken = default) where TParameter : default
        {
            var cancellationTokenValue = cancellationToken ?? CancellationToken.None;
            cancellationTokenValue.ThrowIfCancellationRequested();
            var helper = await ExecuteOnceInternalAsync(messageName, argument, options?.ArgumentConvertingCallback,
                options?.IsAlwaysExecuteAll, cancellationTokenValue).ConfigureAwait(false);
            cancellationTokenValue.ThrowIfCancellationRequested();
            return helper.GetMessageInstance();
        }
    }
}
