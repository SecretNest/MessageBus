using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus
{
    internal interface MessageExecutorSequencerSupport
    {
        void OnAddedToSequencer(Func<object?, MessageInstance?, AcceptedReturn?> executeCallback,
            Func<object?, MessageInstance?, CancellationToken, Task<AcceptedReturn?>> executeAsyncCallback,
            Func<MessageInstance> getMessageInstanceCallback);

        void OnRemovedFromSequencer();
    }

    internal class AcceptedReturn
    {
        public object? Value { get; }

        public AcceptedReturn(object? value)
        {
            Value = value;
        }
    }

    internal class MessageExecutor<TParameter> : MessageExecutorBase<TParameter>, MessageExecutorSequencerSupport, IDisposable
    {
        private Func<object?, MessageInstance?, AcceptedReturn?> _executeCallback = null!;
        private Func<object?, MessageInstance?, CancellationToken, Task<AcceptedReturn?>> _executeAsyncCallback = null!;
        private Func<MessageInstance> _getMessageInstanceCallback = null!;
        private Func<TParameter?, object?>? _argumentConvertingCallback;

        void MessageExecutorSequencerSupport.OnAddedToSequencer(Func<object?, MessageInstance?, AcceptedReturn?> executeCallback, Func<object?, MessageInstance?, CancellationToken, Task<AcceptedReturn?>> executeAsyncCallback, Func<MessageInstance> getMessageInstanceCallback)
        {
            _executeCallback = executeCallback;
            _executeAsyncCallback = executeAsyncCallback;
            _getMessageInstanceCallback = getMessageInstanceCallback;
        }

        void MessageExecutorSequencerSupport.OnRemovedFromSequencer()
        {
            _executeCallback = null!;
            _executeAsyncCallback = null!;
            _getMessageInstanceCallback = null!;
        }

        internal void ApplyPublisherOptions(Func<TParameter?, object?>? argumentConvertingCallback)
        {
            _argumentConvertingCallback = argumentConvertingCallback;
        }

        private void Execute(TParameter? argument, MessageInstance? messageInstance)
        {
            if (_argumentConvertingCallback != null)
            {
                _executeCallback(_argumentConvertingCallback(argument), messageInstance);
            }
            else if (argument == null)
            {
                _executeCallback(null, messageInstance);
            }
            else
            {
                _executeCallback(__refvalue(__makeref(argument), object), messageInstance);
            }
        }

        private Task ExecuteAsync(TParameter? argument, MessageInstance? messageInstance, CancellationToken cancellationToken)
        {
            if (_argumentConvertingCallback != null)
            {
                return _executeAsyncCallback(_argumentConvertingCallback(argument), messageInstance, cancellationToken);
            }
            else if (argument == null)
            {
                return _executeAsyncCallback(null, messageInstance, cancellationToken);
            }
            else
            {
                return _executeAsyncCallback(__refvalue(__makeref(argument), object), messageInstance, cancellationToken);
            }
        }

        public override void Execute(TParameter? argument)
        {
            Execute(argument, null);
        }

        public override Task ExecuteAsync(TParameter? argument, CancellationToken cancellationToken = default)
        {
            return ExecuteAsync(argument, null, cancellationToken);
        }

        public override void ExecuteAndGetMessageInstance(TParameter? argument, out MessageInstance messageInstance)
        {
            messageInstance = _getMessageInstanceCallback();
            Execute(argument, messageInstance);
        }

        public override async Task<MessageInstance> ExecuteAndGetMessageInstanceAsync(TParameter? argument, CancellationToken cancellationToken = default)
        {
            var messageInstance = _getMessageInstanceCallback();
            await ExecuteAsync(argument, messageInstance, cancellationToken).ConfigureAwait(false);
            return messageInstance;
        }

        public void Dispose()
        {
            _argumentConvertingCallback = null;
        }
    }

    internal class MessageExecutor<TParameter, TReturn> : MessageExecutorBase<TParameter, TReturn>, MessageExecutorSequencerSupport, IDisposable
    {
        private Func<object?, MessageInstance?, AcceptedReturn?> _executeCallback = null!;
        private Func<object?, MessageInstance?, CancellationToken, Task<AcceptedReturn?>> _executeAsyncCallback = null!;
        private Func<MessageInstance> _getMessageInstanceCallback = null!;
        private TReturn? _defaultReturnValue;
        private Func<TParameter?, object?>? _argumentConvertingCallback;
        private Func<object?, TReturn?>? _returnValueConvertingCallback;

        void MessageExecutorSequencerSupport.OnAddedToSequencer(Func<object?, MessageInstance?, AcceptedReturn?> executeCallback, Func<object?, MessageInstance?, CancellationToken, Task<AcceptedReturn?>> executeAsyncCallback, Func<MessageInstance> getMessageInstanceCallback)
        {
            _executeCallback = executeCallback;
            _executeAsyncCallback = executeAsyncCallback;
            _getMessageInstanceCallback = getMessageInstanceCallback;
        }

        void MessageExecutorSequencerSupport.OnRemovedFromSequencer()
        {
            _executeCallback = null!;
            _executeAsyncCallback = null!;
            _getMessageInstanceCallback = null!;
        }
        
        internal void ApplyPublisherOptions(TReturn? defaultReturnValue, Func<TParameter?, object?>? argumentConvertingCallback, Func<object?, TReturn?>? returnValueConvertingCallback)
        {
            _defaultReturnValue = defaultReturnValue;
            _argumentConvertingCallback = argumentConvertingCallback;
            _returnValueConvertingCallback = returnValueConvertingCallback;
        }

        private TReturn? Execute(TParameter? argument, MessageInstance? messageInstance)
        {
            AcceptedReturn? result;

            if (_argumentConvertingCallback != null)
            {
                result = _executeCallback(_argumentConvertingCallback(argument), messageInstance);
            }
            else if (argument == null)
            {
                result = _executeCallback(null, messageInstance);
            }
            else
            {
                result = _executeCallback(__refvalue(__makeref(argument), object), messageInstance);
            }

            if (result == null)
            {
                return _defaultReturnValue;
            }

            if (_returnValueConvertingCallback != null)
            {
                return _returnValueConvertingCallback(result.Value);
            }
            else
            {
                var value = result.Value;
                if (value == null)
                    return default;
                return __refvalue(__makeref(value), TReturn);
            }
        }

        private async Task<TReturn?> ExecuteAsync(TParameter? argument, MessageInstance? messageInstance, CancellationToken cancellationToken)
        {
            AcceptedReturn? result;

            if (_argumentConvertingCallback != null)
            {
                result = await _executeAsyncCallback(_argumentConvertingCallback(argument), messageInstance, cancellationToken).ConfigureAwait(false);
            }
            else if (argument == null)
            {
                result = await _executeAsyncCallback(null, messageInstance, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                result = await _executeAsyncCallback(__refvalue(__makeref(argument), object), messageInstance, cancellationToken).ConfigureAwait(false);
            }

            if (result == null)
            {
                return _defaultReturnValue;
            }

            if (_returnValueConvertingCallback != null)
            {
                return _returnValueConvertingCallback(result.Value);
            }
            else
            {
                var value = result.Value;
                if (value == null)
                    return default;
                return __refvalue(__makeref(value), TReturn);
            }
        }

        public override TReturn? Execute(TParameter? argument)
        {
            return Execute(argument, null);
        }

        public override Task<TReturn?> ExecuteAsync(TParameter? argument, CancellationToken cancellationToken = default)
        {
            return ExecuteAsync(argument, null, cancellationToken);
        }

        public override TReturn? ExecuteAndGetMessageInstance(TParameter? argument, out MessageInstance messageInstance)
        {
            messageInstance = _getMessageInstanceCallback();
            return Execute(argument, messageInstance);
        }

        public override async Task<MessageInstanceWithReturnValue<TReturn>> ExecuteAndGetMessageInstanceAsync(TParameter? argument, CancellationToken cancellationToken = default)
        {
            var messageInstance = _getMessageInstanceCallback();
            var result = await ExecuteAsync(argument, messageInstance, cancellationToken).ConfigureAwait(false);
            return new MessageInstanceWithReturnValue<TReturn>(messageInstance, result);
        }

        public void Dispose()
        {
            _defaultReturnValue = default;
            _argumentConvertingCallback = null;
            _returnValueConvertingCallback = null;
        }
    }
}
