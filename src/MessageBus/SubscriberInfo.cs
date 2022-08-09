using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    internal abstract class SubscriberInfoBase
    {
        public abstract MessageNameMatcherBase MessageNameMatcher { get; }

        public abstract int Sequence { get; }
        public abstract bool IsAlwaysExecution { get; }

        public abstract AcceptedReturn? Execute(object? argument, Lazy<MessageInstance> boxedMessageInstance);
        public abstract Task<AcceptedReturn?> ExecuteAsync(object? argument, Lazy<MessageInstance> boxedMessageInstance, CancellationToken cancellationToken);
    }

    internal abstract class SubscriberInfoBase<TParameter> : SubscriberInfoBase
    {
        protected SubscriberInfoBase(string messageName, MessageBusSubscriberOptions<TParameter>? options) : this(new MessageNameMatchingWithStringComparison(messageName, StringComparison.Ordinal), options){}
        protected SubscriberInfoBase(MessageNameMatcherBase messageNameMatcher, MessageBusSubscriberOptions<TParameter>? options)
        {
            MessageNameMatcher = messageNameMatcher;
            if (options != null)
            {
                Sequence = options.Sequence;
                IsAlwaysExecution = options.IsAlwaysExecution;
                _isFinal = options.IsFinal;
                _argumentConvertingCallback = options.ArgumentConvertingCallback;
            }
            else
            {
                _isFinal = true;
            }
        }

        public override MessageNameMatcherBase MessageNameMatcher { get; }
        public override int Sequence { get; }
        public override bool IsAlwaysExecution { get; }
        private bool _isFinal { get; }
        private readonly Func<object?, TParameter?>? _argumentConvertingCallback;

        public abstract void ExecuteInternal(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance);
        public abstract Task ExecuteInternalAsync(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance, CancellationToken cancellationToken);

        public override AcceptedReturn? Execute(object? argument, Lazy<MessageInstance> boxedMessageInstance)
        {
            if (_argumentConvertingCallback != null)
            {
                ExecuteInternal(_argumentConvertingCallback(argument), boxedMessageInstance);
            }
            else if (argument == null)
            {
                ExecuteInternal(default, boxedMessageInstance);
            }
            else
            {
                ExecuteInternal(__refvalue(__makeref(argument), TParameter), boxedMessageInstance);
            }

            if (_isFinal)
            {
                return new AcceptedReturn(null);
            }
            else
            {
                return null;
            }
        }

        public override async Task<AcceptedReturn?> ExecuteAsync(object? argument, Lazy<MessageInstance> boxedMessageInstance, CancellationToken cancellationToken)
        {
            if (_argumentConvertingCallback != null)
            {
                await ExecuteInternalAsync(_argumentConvertingCallback(argument), boxedMessageInstance, cancellationToken);
            }
            else if (argument == null)
            {
                await ExecuteInternalAsync(default, boxedMessageInstance, cancellationToken);
            }
            else
            {
                await ExecuteInternalAsync(__refvalue(__makeref(argument), TParameter), boxedMessageInstance, cancellationToken);
            }

            if (_isFinal)
            {
                return new AcceptedReturn(null);
            }
            else
            {
                return null;
            }
        }
    }

    internal abstract class SubscriberInfoBase<TParameter, TReturn> : SubscriberInfoBase
    {
        protected SubscriberInfoBase(string messageName, MessageBusSubscriberOptions<TParameter, TReturn>? options) : this(new MessageNameMatchingWithStringComparison(messageName, StringComparison.Ordinal), options) { }
        protected SubscriberInfoBase(MessageNameMatcherBase messageNameMatcher, MessageBusSubscriberOptions<TParameter, TReturn>? options)
        {
            MessageNameMatcher = messageNameMatcher;
            if (options != null)
            {
                Sequence = options.Sequence;
                IsAlwaysExecution = options.IsAlwaysExecution;
                _isFinal = options.IsFinal;
                _resultCheckingCallback = options.ResultCheckingCallback;
                _argumentConvertingCallback = options.ArgumentConvertingCallback;
                _returnValueConvertingCallback = options.ReturnValueConvertingCallback;
            }
            else
            {
                _isFinal = true;
            }
        }

        public override MessageNameMatcherBase MessageNameMatcher { get; }
        public override int Sequence { get; }
        public override bool IsAlwaysExecution { get; }
        private bool _isFinal { get; }
        private readonly Func<object?, TParameter?>? _argumentConvertingCallback;
        private readonly Func<TReturn?, object?>? _returnValueConvertingCallback;
        private readonly Func<TReturn?, bool>? _resultCheckingCallback;

        public abstract TReturn? ExecuteInternal(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance);
        public abstract Task<TReturn?> ExecuteInternalAsync(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance, CancellationToken cancellationToken);

        public override AcceptedReturn? Execute(object? argument, Lazy<MessageInstance> boxedMessageInstance)
        {
            TReturn? result;

            if (_argumentConvertingCallback != null)
            {
                result = ExecuteInternal(_argumentConvertingCallback(argument), boxedMessageInstance);
            }
            else if (argument == null)
            {
                result = ExecuteInternal(default, boxedMessageInstance);
            }
            else
            {
                result = ExecuteInternal(__refvalue(__makeref(argument), TParameter), boxedMessageInstance);
            }

            bool shouldReturn;
            if (_resultCheckingCallback != null) //cannot be _isFinal
            {
                shouldReturn = _resultCheckingCallback(result);
            }
            else
            {
                shouldReturn = _isFinal;
            }

            if (shouldReturn)
            {
                if (_returnValueConvertingCallback != null)
                {
                    return new AcceptedReturn(_returnValueConvertingCallback(result));
                }
                else if (result == null)
                {
                    return new AcceptedReturn(null);
                }
                else
                {
                    return new AcceptedReturn(__refvalue(__makeref(result), TReturn));
                }
            }
            else
            {
                return null;
            }
        }

        public override async Task<AcceptedReturn?> ExecuteAsync(object? argument, Lazy<MessageInstance> boxedMessageInstance, CancellationToken cancellationToken)
        {
            TReturn? result;

            if (_argumentConvertingCallback != null)
            {
                result = await ExecuteInternalAsync(_argumentConvertingCallback(argument), boxedMessageInstance, cancellationToken);
            }
            else if (argument == null)
            {
                result = await ExecuteInternalAsync(default, boxedMessageInstance, cancellationToken);
            }
            else
            {
                result = await ExecuteInternalAsync(__refvalue(__makeref(argument), TParameter), boxedMessageInstance, cancellationToken);
            }

            bool shouldReturn;
            if (_resultCheckingCallback != null) //cannot be _isFinal
            {
                shouldReturn = _resultCheckingCallback(result);
            }
            else
            {
                shouldReturn = _isFinal;
            }

            if (shouldReturn)
            {
                if (_returnValueConvertingCallback != null)
                {
                    return new AcceptedReturn(_returnValueConvertingCallback(result));
                }
                else if (result == null)
                {
                    return new AcceptedReturn(null);
                }
                else
                {
                    return new AcceptedReturn(__refvalue(__makeref(result), TReturn));
                }
            }
            else
            {
                return null;
            }
        }
    }

    internal class SubscriberInfo<TParameter> : SubscriberInfoBase<TParameter>
    {
        public SubscriberInfo(string messageName, MessageBusSubscriberOptions<TParameter>? options, Subscriber<TParameter> callback) : base(messageName, options)
        {
            _callback = callback;
        }

        public SubscriberInfo(MessageNameMatcherBase messageNameMatcher, MessageBusSubscriberOptions<TParameter>? options, Subscriber<TParameter> callback) : base(messageNameMatcher, options)
        {
            _callback = callback;
        }

        private readonly Subscriber<TParameter> _callback;

        public override void ExecuteInternal(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance)
        {
            _callback(argument);
        }

        public override Task ExecuteInternalAsync(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance,
            CancellationToken cancellationToken)
        {
            _callback(argument);
            return Task.CompletedTask;
        }
    }

    internal class SubscriberInfo<TParameter, TReturn> : SubscriberInfoBase<TParameter, TReturn>
    {
        public SubscriberInfo(string messageName, MessageBusSubscriberOptions<TParameter, TReturn>? options, Subscriber<TParameter, TReturn> callback) : base(messageName, options)
        {
            _callback = callback;
        }

        public SubscriberInfo(MessageNameMatcherBase messageNameMatcher, MessageBusSubscriberOptions<TParameter, TReturn>? options, Subscriber<TParameter, TReturn> callback) : base(messageNameMatcher, options)
        {
            _callback = callback;
        }

        private readonly Subscriber<TParameter, TReturn> _callback;

       public override TReturn? ExecuteInternal(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance)
        {
            return _callback(argument);
        }

        public override Task<TReturn?> ExecuteInternalAsync(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance,
            CancellationToken cancellationToken)
        {
            var result = _callback(argument);
            return Task.FromResult(result);
        }
    }

    internal class SubscriberInfoWithMessageInstance<TParameter> : SubscriberInfoBase<TParameter>
    {
        public SubscriberInfoWithMessageInstance(string messageName, MessageBusSubscriberOptions<TParameter>? options, SubscriberWithMessageInstance<TParameter> callback) : base(messageName, options)
        {
            _callback = callback;
        }

        public SubscriberInfoWithMessageInstance(MessageNameMatcherBase messageNameMatcher, MessageBusSubscriberOptions<TParameter>? options, SubscriberWithMessageInstance<TParameter> callback) : base(messageNameMatcher, options)
        {
            _callback = callback;
        }

        private readonly SubscriberWithMessageInstance<TParameter> _callback;

        public override void ExecuteInternal(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance)
        {
            _callback(argument, boxedMessageInstance.Value);
        }

        public override Task ExecuteInternalAsync(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance,
            CancellationToken cancellationToken)
        {
            _callback(argument, boxedMessageInstance.Value);
            return Task.CompletedTask;
        }
    }

    internal class SubscriberInfoWithMessageInstance<TParameter, TReturn> : SubscriberInfoBase<TParameter, TReturn>
    {
        public SubscriberInfoWithMessageInstance(string messageName, MessageBusSubscriberOptions<TParameter, TReturn>? options, SubscriberWithMessageInstance<TParameter, TReturn> callback) : base(messageName, options)
        {
            _callback = callback;
        }

        public SubscriberInfoWithMessageInstance(MessageNameMatcherBase messageNameMatcher, MessageBusSubscriberOptions<TParameter, TReturn>? options, SubscriberWithMessageInstance<TParameter, TReturn> callback) : base(messageNameMatcher, options)
        {
            _callback = callback;
        }

        private readonly SubscriberWithMessageInstance<TParameter, TReturn> _callback;

        public override TReturn? ExecuteInternal(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance)
        {
            return _callback(argument, boxedMessageInstance.Value);
        }

        public override Task<TReturn?> ExecuteInternalAsync(TParameter? argument,
            Lazy<MessageInstance> boxedMessageInstance, CancellationToken cancellationToken)
        {
            var result = _callback(argument, boxedMessageInstance.Value);
            return Task.FromResult(result);
        }
    }

    internal class SubscriberAsyncInfo<TParameter> : SubscriberInfoBase<TParameter>
    {
        public SubscriberAsyncInfo(string messageName, MessageBusSubscriberOptions<TParameter>? options, SubscriberAsync<TParameter> callback) : base(messageName, options)
        {
            _callback = callback;
        }

        public SubscriberAsyncInfo(MessageNameMatcherBase messageNameMatcher, MessageBusSubscriberOptions<TParameter>? options, SubscriberAsync<TParameter> callback) : base(messageNameMatcher, options)
        {
            _callback = callback;
        }

        private readonly SubscriberAsync<TParameter> _callback;

        public override void ExecuteInternal(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance)
        {
            _callback(argument, CancellationToken.None).Wait();
        }

        public override async Task ExecuteInternalAsync(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance,
            CancellationToken cancellationToken)
        {
            await _callback(argument, cancellationToken).ConfigureAwait(false);
        }
    }

    internal class SubscriberAsyncInfo<TParameter, TReturn> : SubscriberInfoBase<TParameter, TReturn>
    {
        public SubscriberAsyncInfo(string messageName, MessageBusSubscriberOptions<TParameter, TReturn>? options, SubscriberAsync<TParameter, TReturn> callback) : base(messageName, options)
        {
            _callback = callback;
        }

        public SubscriberAsyncInfo(MessageNameMatcherBase messageNameMatcher, MessageBusSubscriberOptions<TParameter, TReturn>? options, SubscriberAsync<TParameter, TReturn> callback) : base(messageNameMatcher, options)
        {
            _callback = callback;
        }

        private readonly SubscriberAsync<TParameter, TReturn> _callback;

        public override TReturn? ExecuteInternal(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance)
        {
            return _callback(argument, CancellationToken.None).Result;
        }

        public override async Task<TReturn?> ExecuteInternalAsync(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance,
            CancellationToken cancellationToken)
        {
            return await _callback(argument, cancellationToken).ConfigureAwait(false);
        }
    }

    internal class SubscriberAsyncInfoWithMessageInstance<TParameter> : SubscriberInfoBase<TParameter>
    {
        public SubscriberAsyncInfoWithMessageInstance(string messageName, MessageBusSubscriberOptions<TParameter>? options, SubscriberWithMessageInstanceAsync<TParameter> callback) : base(messageName, options)
        {
            _callback = callback;
        }

        public SubscriberAsyncInfoWithMessageInstance(MessageNameMatcherBase messageNameMatcher, MessageBusSubscriberOptions<TParameter>? options, SubscriberWithMessageInstanceAsync<TParameter> callback) : base(messageNameMatcher, options)
        {
            _callback = callback;
        }

        private readonly SubscriberWithMessageInstanceAsync<TParameter> _callback;

        public override void ExecuteInternal(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance)
        {
            _callback(argument, boxedMessageInstance.Value, CancellationToken.None).Wait();
        }

        public override async Task ExecuteInternalAsync(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance,
            CancellationToken cancellationToken)
        {
            await _callback(argument, boxedMessageInstance.Value, cancellationToken).ConfigureAwait(false);
        }
    }

    internal class SubscriberAsyncInfoWithMessageInstance<TParameter, TReturn> : SubscriberInfoBase<TParameter, TReturn>
    {
        public SubscriberAsyncInfoWithMessageInstance(string messageName, MessageBusSubscriberOptions<TParameter, TReturn>? options, SubscriberWithMessageInstanceAsync<TParameter, TReturn> callback) : base(messageName, options)
        {
            _callback = callback;
        }

        public SubscriberAsyncInfoWithMessageInstance(MessageNameMatcherBase messageNameMatcher, MessageBusSubscriberOptions<TParameter, TReturn>? options, SubscriberWithMessageInstanceAsync<TParameter, TReturn> callback) : base(messageNameMatcher, options)
        {
            _callback = callback;
        }

        private readonly SubscriberWithMessageInstanceAsync<TParameter, TReturn> _callback;

        public override TReturn? ExecuteInternal(TParameter? argument, Lazy<MessageInstance> boxedMessageInstance)
        {
            return _callback(argument, boxedMessageInstance.Value, CancellationToken.None).Result;
        }

        public override async Task<TReturn?> ExecuteInternalAsync(TParameter? argument,
            Lazy<MessageInstance> boxedMessageInstance, CancellationToken cancellationToken)
        {
            return await _callback(argument, boxedMessageInstance.Value, cancellationToken).ConfigureAwait(false);
        }
    }
}
