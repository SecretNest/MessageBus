using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBus
    {
        public override SubscriberTicket<TParameter, Subscriber<TParameter>> RegisterSubscriber<TParameter>(string messageName, Subscriber<TParameter> handler,
            MessageBusSubscriberOptions<TParameter>? options = default)
        {
            throw new NotImplementedException();
        }

        public override SubscriberTicket<TParameter, Subscriber<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher, Subscriber<TParameter> handler,
            MessageBusSubscriberOptions<TParameter>? options = default)
        {
            throw new NotImplementedException();
        }

        public override SubscriberTicket<TParameter, SubscriberWithMessageInstance<TParameter>> RegisterSubscriber<TParameter>(string messageName, SubscriberWithMessageInstance<TParameter> handler,
            MessageBusSubscriberOptions<TParameter>? options = default)
        {
            throw new NotImplementedException();
        }

        public override SubscriberTicket<TParameter, SubscriberWithMessageInstance<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher,
            SubscriberWithMessageInstance<TParameter> handler, MessageBusSubscriberOptions<TParameter>? options = default)
        {
            throw new NotImplementedException();
        }
    }
}
