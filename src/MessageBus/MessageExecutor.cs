using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus
{
    internal interface MessageExecutorSequencerSupport
    {
        void OnAddedToSequencer(Func<object?, MessageInstanceHelper> executeCallback,
            Func<object?, CancellationToken, Task<MessageInstanceHelper>> executeAsyncCallback);

        void OnRemovedFromSequencer();
    }

    internal class MessageExecutor<TParameter> : MessageExecutorBase<TParameter>, MessageExecutorSequencerSupport, IDisposable
    {
        private Func<object?, MessageInstanceHelper> _executeCallback = null!;
        private Func<object?, CancellationToken, Task<MessageInstanceHelper>> _executeAsyncCallback = null!;
        private Func<TParameter?, object?>? _argumentConvertingCallback;

        void MessageExecutorSequencerSupport.OnAddedToSequencer(Func<object?, MessageInstanceHelper> executeCallback, Func<object?, CancellationToken, Task<MessageInstanceHelper>> executeAsyncCallback)
        {
            _executeCallback = executeCallback;
            _executeAsyncCallback = executeAsyncCallback;
        }

        void MessageExecutorSequencerSupport.OnRemovedFromSequencer()
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
            else if (argument == null)
            {
                return _executeCallback(null);
            }
            else
            {
                return _executeCallback(__refvalue(__makeref(argument), object));
            }
        }

        private async Task<MessageInstanceHelper> ExecuteWrappedAsync(TParameter? argument, CancellationToken? cancellationToken)
        {
            if (_argumentConvertingCallback != null)
            {
                return (await _executeAsyncCallback(_argumentConvertingCallback(argument), cancellationToken ??= CancellationToken.None));
            }
            else if (argument == null)
            {
                return (await _executeAsyncCallback(null, cancellationToken ??= CancellationToken.None));
            }
            else
            {
                return (await _executeAsyncCallback(__refvalue(__makeref(argument), object), cancellationToken ??= CancellationToken.None));
            }
        }

        public override void Execute(TParameter? argument)
        {
            ExecuteWrapped(argument);
        }

        public override async Task ExecuteAsync(TParameter? argument, CancellationToken? cancellationToken = default)
        {
            await ExecuteWrappedAsync(argument, cancellationToken);
        }

        public override void ExecuteAndGetMessageInstance(TParameter? argument, out MessageInstanceWithVoidReturnValue messageInstance)
        {
            var helper = ExecuteWrapped(argument);
            messageInstance = helper.GetMessageInstanceWithVoidReturnValue();
        }

        public override async Task<MessageInstanceWithVoidReturnValue> ExecuteAndGetMessageInstanceAsync(TParameter? argument, CancellationToken? cancellationToken = default)
        {
            var helper = await ExecuteWrappedAsync(argument, cancellationToken).ConfigureAwait(false);
            return helper.GetMessageInstanceWithVoidReturnValue();
        }

        public void Dispose()
        {
            _argumentConvertingCallback = null;
        }
    }

    internal class MessageExecutor<TParameter, TReturn> : MessageExecutorBase<TParameter, TReturn>, MessageExecutorSequencerSupport, IDisposable
    {
        private Func<object?, MessageInstanceHelper> _executeCallback = null!;
        private Func<object?, CancellationToken, Task<MessageInstanceHelper>> _executeAsyncCallback = null!;
        private TReturn? _defaultReturnValue;
        private Func<TParameter?, object?>? _argumentConvertingCallback;
        private Func<object?, TReturn?>? _returnValueConvertingCallback;

        void MessageExecutorSequencerSupport.OnAddedToSequencer(Func<object?, MessageInstanceHelper> executeCallback, Func<object?, CancellationToken, Task<MessageInstanceHelper>> executeAsyncCallback)
        {
            _executeCallback = executeCallback;
            _executeAsyncCallback = executeAsyncCallback;
        }

        void MessageExecutorSequencerSupport.OnRemovedFromSequencer()
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
            else if (argument == null)
            {
                result = _executeCallback(null);
            }
            else
            {
                result = _executeCallback(__refvalue(__makeref(argument), object));
            }

            if (result.ReturnValueSourceSubscriberId == null) //default return required
            {
                result.SetFinalResult(_defaultReturnValue);
            }
            else if (_returnValueConvertingCallback != null)
            {
                result.SetFinalResult(_returnValueConvertingCallback(result.SubscriberResult));
            }
            else
            {
                result.AcceptFinalResult<TReturn>();
            }

            return result;
        }

        private async Task<MessageInstanceHelper> ExecuteWrappedAsync(TParameter? argument, CancellationToken? cancellationToken)
        {
            MessageInstanceHelper result;

            if (_argumentConvertingCallback != null)
            {
                result = await _executeAsyncCallback(_argumentConvertingCallback(argument), cancellationToken ??= CancellationToken.None).ConfigureAwait(false);
            }
            else if (argument == null)
            {
                result = await _executeAsyncCallback(null, cancellationToken ??= CancellationToken.None).ConfigureAwait(false);
            }
            else
            {
                result = await _executeAsyncCallback(__refvalue(__makeref(argument), object), cancellationToken ??= CancellationToken.None).ConfigureAwait(false);
            }

            if (result.ReturnValueSourceSubscriberId == null) //default return required
            {
                result.SetFinalResult(_defaultReturnValue);
            }
            else if (_returnValueConvertingCallback != null)
            {
                result.SetFinalResult(_returnValueConvertingCallback(result.SubscriberResult));
            }
            else
            {
                result.AcceptFinalResult<TReturn>();
            }

            return result;
        }

        public override TReturn? Execute(TParameter? argument)
        {
            return ExecuteWrapped(argument).GetFinalResult<TReturn>();
        }

        public override async Task<TReturn?> ExecuteAsync(TParameter? argument, CancellationToken? cancellationToken = default)
        {
            return (await ExecuteWrappedAsync(argument, cancellationToken)).GetFinalResult<TReturn>();
        }

        public override TReturn? ExecuteAndGetMessageInstance(TParameter? argument, out MessageInstanceWithReturnValue<TReturn> messageInstance)
        {
            var helper = ExecuteWrapped(argument);
            messageInstance = helper.GetMessageInstanceWithReturnValue<TReturn>();
            return helper.GetFinalResult<TReturn>();
        }

        public override async Task<MessageInstanceWithReturnValue<TReturn>> ExecuteAndGetMessageInstanceAsync(TParameter? argument, CancellationToken? cancellationToken = default)
        {
            var helper = await ExecuteWrappedAsync(argument, cancellationToken).ConfigureAwait(false);
            return helper.GetMessageInstanceWithReturnValue<TReturn>();
        }

        public void Dispose()
        {
            _defaultReturnValue = default;
            _argumentConvertingCallback = null;
            _returnValueConvertingCallback = null;
        }
    }
}
