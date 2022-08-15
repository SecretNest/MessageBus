using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SecretNest.MessageBus
{
    internal interface IMessageExecutorSequencerSupport
    {
        void OnAddedToSequencer(Func<object?, MessageInstanceHelper> executeCallback,
            Func<object?, CancellationToken, Task<MessageInstanceHelper>> executeAsyncCallback);

        void OnRemovedFromSequencer();
    }

    internal class MessageExecutor<TParameter> : MessageExecutorBase<TParameter>, IMessageExecutorSequencerSupport, IDisposable
    {
        private Func<object?, MessageInstanceHelper> _executeCallback = null!;
        private Func<object?, CancellationToken, Task<MessageInstanceHelper>> _executeAsyncCallback = null!;
        private Func<TParameter?, object?>? _argumentConvertingCallback;

        void IMessageExecutorSequencerSupport.OnAddedToSequencer(Func<object?, MessageInstanceHelper> executeCallback, Func<object?, CancellationToken, Task<MessageInstanceHelper>> executeAsyncCallback)
        {
            _executeCallback = executeCallback;
            _executeAsyncCallback = executeAsyncCallback;
        }

        void IMessageExecutorSequencerSupport.OnRemovedFromSequencer()
        {
            _executeCallback = null!;
            _executeAsyncCallback = null!;
        }

        internal void ApplyPublisherOptions(Func<TParameter?, object?>? argumentConvertingCallback)
        {
            _argumentConvertingCallback = argumentConvertingCallback;
        }

        private MessageInstanceHelper ExecuteWrapped(TParameter? argument)
        {
            if (_argumentConvertingCallback != null)
            {
                return _executeCallback(_argumentConvertingCallback(argument));
            }
            else
            {
                return _executeCallback(argument);
            }
        }

        private async Task<MessageInstanceHelper> ExecuteWrappedAsync(TParameter? argument, CancellationToken cancellationToken)
        {
            if (_argumentConvertingCallback != null)
            {
                return (await _executeAsyncCallback(_argumentConvertingCallback(argument), cancellationToken));
            }
            else
            {
                return (await _executeAsyncCallback(argument, cancellationToken));
            }
        }

        public override void Execute(TParameter? argument)
        {
            ExecuteWrapped(argument);
        }

        public override async Task ExecuteAsync(TParameter? argument, CancellationToken? cancellationToken = default)
        {
            var cancellationTokenValue = cancellationToken ?? CancellationToken.None;
            cancellationTokenValue.ThrowIfCancellationRequested();
            await ExecuteWrappedAsync(argument, cancellationTokenValue);
        }

        public override void ExecuteAndGetMessageInstance(TParameter? argument, out MessageInstance messageInstance)
        {
            var helper = ExecuteWrapped(argument);
            messageInstance = helper.GetMessageInstance();
        }

        public override async Task<MessageInstance> ExecuteAndGetMessageInstanceAsync(TParameter? argument, CancellationToken? cancellationToken = default)
        {
            var cancellationTokenValue = cancellationToken ?? CancellationToken.None;
            cancellationTokenValue.ThrowIfCancellationRequested();
            var helper = await ExecuteWrappedAsync(argument, cancellationTokenValue).ConfigureAwait(false);
            cancellationTokenValue.ThrowIfCancellationRequested();
            return helper.GetMessageInstance();
        }

        public void Dispose()
        {
            _argumentConvertingCallback = null;
        }
    }

    internal class MessageExecutor<TParameter, TReturn> : MessageExecutorBase<TParameter, TReturn>, IMessageExecutorSequencerSupport, IDisposable
    {
        private Func<object?, MessageInstanceHelper> _executeCallback = null!;
        private Func<object?, CancellationToken, Task<MessageInstanceHelper>> _executeAsyncCallback = null!;
        private TReturn? _defaultReturnValue;
        private Func<TParameter?, object?>? _argumentConvertingCallback;
        private Func<object?, TReturn?>? _returnValueConvertingCallback;

        void IMessageExecutorSequencerSupport.OnAddedToSequencer(Func<object?, MessageInstanceHelper> executeCallback, Func<object?, CancellationToken, Task<MessageInstanceHelper>> executeAsyncCallback)
        {
            _executeCallback = executeCallback;
            _executeAsyncCallback = executeAsyncCallback;
        }

        void IMessageExecutorSequencerSupport.OnRemovedFromSequencer()
        {
            _executeCallback = null!;
            _executeAsyncCallback = null!;
        }
        
        internal void ApplyPublisherOptions(TReturn? defaultReturnValue, Func<TParameter?, object?>? argumentConvertingCallback, Func<object?, TReturn?>? returnValueConvertingCallback)
        {
            _defaultReturnValue = defaultReturnValue;
            _argumentConvertingCallback = argumentConvertingCallback;
            _returnValueConvertingCallback = returnValueConvertingCallback;
        }

        private MessageInstanceHelper ExecuteWrapped(TParameter? argument)
        {
            MessageInstanceHelper result;

            if (_argumentConvertingCallback != null)
            {
                result = _executeCallback(_argumentConvertingCallback(argument));
            }
            else
            {
                result = _executeCallback(argument);
            }

            return result;
        }

        private async Task<MessageInstanceHelper> ExecuteWrappedAsync(TParameter? argument, CancellationToken cancellationToken)
        {
            MessageInstanceHelper result;

            if (_argumentConvertingCallback != null)
            {
                result = await _executeAsyncCallback(_argumentConvertingCallback(argument), cancellationToken).ConfigureAwait(false);
            }
            else
            {
                result = await _executeAsyncCallback(argument, cancellationToken).ConfigureAwait(false);
            }

            return result;
        }

        private TReturn? GetFinalResult(MessageInstanceHelper helper)
        {
            if (helper.ReturnValueSourceSubscriberId == null) //default return required
            {
                return _defaultReturnValue;
            }
            else
            {
                var result = helper.SubscriberResult;
                if (_returnValueConvertingCallback != null)
                {
                    return _returnValueConvertingCallback(result);
                }
                else if (result == null)
                {
                    return default;
                }
                else
                {
                    return (TReturn?)result;
                }
            }
        }

        public override TReturn? Execute(TParameter? argument)
        {
            return GetFinalResult(ExecuteWrapped(argument));
        }

        public override async Task<TReturn?> ExecuteAsync(TParameter? argument, CancellationToken? cancellationToken = default)
        {
            var cancellationTokenValue = cancellationToken ?? CancellationToken.None;
            cancellationTokenValue.ThrowIfCancellationRequested();
            var helper = await ExecuteWrappedAsync(argument, cancellationTokenValue);
            cancellationTokenValue.ThrowIfCancellationRequested();
            return GetFinalResult(helper);
        }

        public override TReturn? ExecuteAndGetMessageInstance(TParameter? argument, out MessageInstance messageInstance)
        {
            var helper = ExecuteWrapped(argument);
            messageInstance = helper.GetMessageInstance();
            return GetFinalResult(helper);
        }

        public override async Task<MessageInstanceWithExecutorResult<TReturn>> ExecuteAndGetMessageInstanceAsync(TParameter? argument, CancellationToken? cancellationToken = default)
        {
            var cancellationTokenValue = cancellationToken ?? CancellationToken.None;
            cancellationTokenValue.ThrowIfCancellationRequested();
            var helper = await ExecuteWrappedAsync(argument, cancellationTokenValue).ConfigureAwait(false);
            cancellationTokenValue.ThrowIfCancellationRequested();
            return new MessageInstanceWithExecutorResult<TReturn>(helper.GetMessageInstance(), GetFinalResult(helper));
        }

        public void Dispose()
        {
            _defaultReturnValue = default;
            _argumentConvertingCallback = null;
            _returnValueConvertingCallback = null;
        }
    }
}
