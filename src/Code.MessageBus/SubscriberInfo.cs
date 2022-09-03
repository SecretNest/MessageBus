using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    internal abstract class SubscriberInfoBase
    {
        public Guid SubscriberId { get; set; } //set by AddSubscriberToPool

        public abstract MessageNameMatcherBase MessageNameMatcher { get; }

        public abstract int Sequence { get; }
        public abstract bool IsAlwaysExecution { get; }

        public abstract Func<object?, bool>? ConditionCheckingCallback { get; }

        public abstract void Execute(object? argument, MessageInstanceHelper messageInstanceHelper);
        public abstract Task ExecuteAsync(object? argument, MessageInstanceHelper messageInstanceHelper, CancellationToken cancellationToken);

        public abstract void ExecuteForce(object? argument, MessageInstanceHelper messageInstanceHelper);
        public abstract Task ExecuteForceAsync(object? argument, MessageInstanceHelper messageInstanceHelper, CancellationToken cancellationToken);
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
                ConditionCheckingCallback = options.ConditionCheckingCallback;
            }
            else
            {
                _isFinal = true;
            }
        }

        public override MessageNameMatcherBase MessageNameMatcher { get; }
        public override int Sequence { get; }
        public override bool IsAlwaysExecution { get; }
        private readonly bool _isFinal;
        public override Func<object?, bool>? ConditionCheckingCallback { get; }

        private readonly Func<object?, TParameter?>? _argumentConvertingCallback;

        public abstract void ExecuteInternal(TParameter? argument, MessageInstanceHelper messageInstanceHelper);
        public abstract Task ExecuteInternalAsync(TParameter? argument, MessageInstanceHelper messageInstanceHelper, CancellationToken cancellationToken);

        public override void Execute(object? argument, MessageInstanceHelper messageInstanceHelper)
        {
            ExecuteForce(argument, messageInstanceHelper);

            if (_isFinal)
            {
                messageInstanceHelper.SetSubscriberVoidResult(SubscriberId);
            }
        }

        public override async Task ExecuteAsync(object? argument, MessageInstanceHelper messageInstanceHelper, CancellationToken cancellationToken)
        {
            await ExecuteForceAsync(argument, messageInstanceHelper, cancellationToken).ConfigureAwait(false);

            if (_isFinal)
            {
                messageInstanceHelper.SetSubscriberVoidResult(SubscriberId);
            }
        }

        public override void ExecuteForce(object? argument, MessageInstanceHelper messageInstanceHelper)
        {
            if (_argumentConvertingCallback != null)
            {
                ExecuteInternal(_argumentConvertingCallback(argument), messageInstanceHelper);
            }
            //else if (argument == null)
            //{
            //    ExecuteInternal(default, messageInstanceHelper);
            //}
            else
            {
                ExecuteInternal((TParameter?)argument, messageInstanceHelper);
            }
        }

        public override async Task ExecuteForceAsync(object? argument, MessageInstanceHelper messageInstanceHelper, CancellationToken cancellationToken)
        {
            if (_argumentConvertingCallback != null)
            {
                await ExecuteInternalAsync(_argumentConvertingCallback(argument), messageInstanceHelper, cancellationToken).ConfigureAwait(false);
            }
            //else if (argument == null)
            //{
            //    await ExecuteInternalAsync(default, messageInstanceHelper, cancellationToken);
            //}
            else
            {
                await ExecuteInternalAsync((TParameter?)argument, messageInstanceHelper, cancellationToken).ConfigureAwait(false);
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
                ConditionCheckingCallback = options.ConditionCheckingCallback;
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
        public override Func<object?, bool>? ConditionCheckingCallback { get; }
        private readonly Func<object?, TParameter?>? _argumentConvertingCallback;
        private readonly Func<TReturn?, object?>? _returnValueConvertingCallback;
        private readonly Func<TReturn?, bool>? _resultCheckingCallback;

        public abstract TReturn? ExecuteInternal(TParameter? argument, MessageInstanceHelper messageInstanceHelper);
        public abstract Task<TReturn?> ExecuteInternalAsync(TParameter? argument, MessageInstanceHelper messageInstanceHelper, CancellationToken cancellationToken);

        public override void Execute(object? argument, MessageInstanceHelper messageInstanceHelper)
        {
            TReturn? result;

            if (_argumentConvertingCallback != null)
            {
                result = ExecuteInternal(_argumentConvertingCallback(argument), messageInstanceHelper);
            }
            //else if (argument == null)
            //{
            //    result = ExecuteInternal(default, messageInstanceHelper);
            //}
            else
            {
                result = ExecuteInternal((TParameter?)argument, messageInstanceHelper);
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
                    messageInstanceHelper.SetSubscriberResult(_returnValueConvertingCallback(result), SubscriberId);
                }
                //else if (result == null)
                //{
                //    messageInstanceHelper.SetSubscriberResult(null, SubscriberId);
                //}
                else
                {
                    messageInstanceHelper.SetSubscriberResult(result, SubscriberId);
                }
            }
        }

        public override async Task ExecuteAsync(object? argument, MessageInstanceHelper messageInstanceHelper, CancellationToken cancellationToken)
        {
            TReturn? result;

            if (_argumentConvertingCallback != null)
            {
                result = await ExecuteInternalAsync(_argumentConvertingCallback(argument), messageInstanceHelper, cancellationToken).ConfigureAwait(false);
            }
            //else if (argument == null)
            //{
            //    result = await ExecuteInternalAsync(default, messageInstanceHelper, cancellationToken);
            //}
            else
            {
                result = await ExecuteInternalAsync((TParameter?)argument, messageInstanceHelper, cancellationToken).ConfigureAwait(false);
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
                    messageInstanceHelper.SetSubscriberResult(_returnValueConvertingCallback(result), SubscriberId);
                }
                //else if (result == null)
                //{
                //    messageInstanceHelper.SetSubscriberResult(null, SubscriberId);
                //}
                else
                {
                    messageInstanceHelper.SetSubscriberResult(result, SubscriberId);
                }
            }
        }

        public override void ExecuteForce(object? argument, MessageInstanceHelper messageInstanceHelper)
        {
            if (_argumentConvertingCallback != null)
            {
                ExecuteInternal(_argumentConvertingCallback(argument), messageInstanceHelper);
            }
            //else if (argument == null)
            //{
            //    ExecuteInternal(default, messageInstanceHelper);
            //}
            else
            {
                ExecuteInternal((TParameter?)argument, messageInstanceHelper);
            }
        }

        public override async Task ExecuteForceAsync(object? argument, MessageInstanceHelper messageInstanceHelper, CancellationToken cancellationToken)
        {
            if (_argumentConvertingCallback != null)
            {
                await ExecuteInternalAsync(_argumentConvertingCallback(argument), messageInstanceHelper, cancellationToken).ConfigureAwait(false);
            }
            //else if (argument == null)
            //{
            //    await ExecuteInternalAsync(default, messageInstanceHelper, cancellationToken);
            //}
            else
            {
                await ExecuteInternalAsync((TParameter?)argument, messageInstanceHelper, cancellationToken).ConfigureAwait(false);
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

        public override void ExecuteInternal(TParameter? argument, MessageInstanceHelper messageInstanceHelper)
        {
            _callback(argument);
        }

        public override Task ExecuteInternalAsync(TParameter? argument, MessageInstanceHelper messageInstanceHelper,
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

        public override TReturn? ExecuteInternal(TParameter? argument, MessageInstanceHelper messageInstanceHelper)
        {
            return _callback(argument);
        }

        public override Task<TReturn?> ExecuteInternalAsync(TParameter? argument, MessageInstanceHelper messageInstanceHelper,
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

        public override void ExecuteInternal(TParameter? argument, MessageInstanceHelper messageInstanceHelper)
        {
            _callback(argument, messageInstanceHelper.GetMessageInstance());
        }

        public override Task ExecuteInternalAsync(TParameter? argument, MessageInstanceHelper messageInstanceHelper,
            CancellationToken cancellationToken)
        {
            _callback(argument, messageInstanceHelper.GetMessageInstance());
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

        public override TReturn? ExecuteInternal(TParameter? argument, MessageInstanceHelper messageInstanceHelper)
        {
            return _callback(argument, messageInstanceHelper.GetMessageInstance());
        }

        public override Task<TReturn?> ExecuteInternalAsync(TParameter? argument,
            MessageInstanceHelper messageInstanceHelper, CancellationToken cancellationToken)
        {
            var result = _callback(argument, messageInstanceHelper.GetMessageInstance());
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

        public override void ExecuteInternal(TParameter? argument, MessageInstanceHelper messageInstanceHelper)
        {
            _callback(argument, CancellationToken.None).Wait();
        }

        public override async Task ExecuteInternalAsync(TParameter? argument, MessageInstanceHelper messageInstanceHelper,
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

        public override TReturn? ExecuteInternal(TParameter? argument, MessageInstanceHelper messageInstanceHelper)
        {
            return _callback(argument, CancellationToken.None).Result;
        }

        public override async Task<TReturn?> ExecuteInternalAsync(TParameter? argument, MessageInstanceHelper messageInstanceHelper,
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

        public override void ExecuteInternal(TParameter? argument, MessageInstanceHelper messageInstanceHelper)
        {
            _callback(argument, messageInstanceHelper.GetMessageInstance(), CancellationToken.None).Wait();
        }

        public override async Task ExecuteInternalAsync(TParameter? argument, MessageInstanceHelper messageInstanceHelper,
            CancellationToken cancellationToken)
        {
            await _callback(argument, messageInstanceHelper.GetMessageInstance(), cancellationToken).ConfigureAwait(false);
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

        public override TReturn? ExecuteInternal(TParameter? argument, MessageInstanceHelper messageInstanceHelper)
        {
            return _callback(argument, messageInstanceHelper.GetMessageInstance(), CancellationToken.None).Result;
        }

        public override async Task<TReturn?> ExecuteInternalAsync(TParameter? argument,
            MessageInstanceHelper messageInstanceHelper, CancellationToken cancellationToken)
        {
            return await _callback(argument, messageInstanceHelper.GetMessageInstance(), cancellationToken).ConfigureAwait(false);
        }
    }
}
