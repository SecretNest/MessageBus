using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus
{
    internal class MessageInstanceHelper
    {
        abstract class ValueWithTypeBase
        {
            public abstract object? ValueGeneric { get; }
            public abstract MessageInstanceWithReturnValueBase GetMessageInstanceWithValue(Guid executingId, string messageName, Guid? returnValueSourceSubscriberId);
        }

        class ValueWithType<T> : ValueWithTypeBase
        {
            public ValueWithType(T? value)
            {
                Value = value;
            }

            public T? Value { get; }
            public override object? ValueGeneric => Value;
            public override MessageInstanceWithReturnValueBase GetMessageInstanceWithValue(Guid executingId, string messageName, Guid? returnValueSourceSubscriberId)
            {
                return new MessageInstanceWithReturnValue<T>(executingId, messageName, Value, returnValueSourceSubscriberId);
            }
        }

        class ValueWithNoType : ValueWithTypeBase
        {
            public ValueWithNoType(object? value)
            {
                Value = value;
            }

            public object? Value { get; }
            public override object? ValueGeneric => Value;
            public override MessageInstanceWithReturnValueBase GetMessageInstanceWithValue(Guid executingId, string messageName, Guid? returnValueSourceSubscriberId)
            {
                return new MessageInstanceWithReturnValue(executingId, messageName, Value, returnValueSourceSubscriberId);
            }
        }

        class ValueNoValue : ValueWithTypeBase
        {
            public override object? ValueGeneric => null;
            public override MessageInstanceWithReturnValueBase GetMessageInstanceWithValue(Guid executingId, string messageName, Guid? returnValueSourceSubscriberId)
            {
                return new MessageInstanceWithVoidReturnValue(executingId, messageName, returnValueSourceSubscriberId);
            }
        }

        #region Subscribers
        private ValueWithTypeBase? _subscriberResult;
        public object? SubscriberResult => _subscriberResult?.ValueGeneric;
        public Guid? ReturnValueSourceSubscriberId { get; private set; }
        public bool IsSubscriberResultSet { get; private set; }

        public MessageInstanceHelper(string messageName)
        {
            MessageName = messageName;
        }

        public string MessageName { get; }

        private MessageInstance? _messageInstance;

        public MessageInstance GetMessageInstance()
        {
            if (_messageInstance == null)
            {
                if (_subscriberResult != null)
                {
                    _messageInstance = _subscriberResult.GetMessageInstanceWithValue(Guid.NewGuid(), MessageName, ReturnValueSourceSubscriberId);
                }
                else
                {
                    _messageInstance = new MessageInstance(Guid.NewGuid(), MessageName);
                }
                return _messageInstance;
            }
            else
            {
                return _messageInstance;
            }
        }

        public void SetSubscriberVoidResult(Guid? subscriberId)
        {
            SetSubscriberResultInternal(new ValueNoValue(), subscriberId);
        }

        public void SetSubscriberResult(object? value, Guid? subscriberId)
        {
            SetSubscriberResultInternal(new ValueWithNoType(value), subscriberId);
        }

        public void SetSubscriberResult<TSubscriberReturn>(TSubscriberReturn? value, Guid? subscriberId)
        {
            SetSubscriberResultInternal(new ValueWithType<TSubscriberReturn>(value), subscriberId);
        }

        private void SetSubscriberResultInternal(ValueWithTypeBase value, Guid? subscriberId)
        {
            _subscriberResult = value;
            ReturnValueSourceSubscriberId = subscriberId;
            IsSubscriberResultSet = true;

            if (_messageInstance != null)
            {
                //upgrade
                _messageInstance = _subscriberResult.GetMessageInstanceWithValue(Guid.NewGuid(), MessageName, subscriberId);
            }
        }
        #endregion
    }
}
