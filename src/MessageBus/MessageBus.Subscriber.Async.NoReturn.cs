using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBus
    {
        public override SubscriberTicket<TParameter, SubscriberAsync<TParameter>> RegisterSubscriber<TParameter>(string messageName, SubscriberAsync<TParameter> handler,
            MessageBusSubscriberOptions<TParameter>? options = default)
        {
            throw new NotImplementedException();
        }

        public override SubscriberTicket<TParameter, SubscriberAsync<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher, SubscriberAsync<TParameter> handler,
            MessageBusSubscriberOptions<TParameter>? options = default)
        {
            throw new NotImplementedException();
        }

        public override SubscriberTicket<TParameter, SubscriberWithMessageInstanceAsync<TParameter>> RegisterSubscriber<TParameter>(string messageName, SubscriberWithMessageInstanceAsync<TParameter> handler,
            MessageBusSubscriberOptions<TParameter>? options = default)
        {
            throw new NotImplementedException();
        }

        public override SubscriberTicket<TParameter, SubscriberWithMessageInstanceAsync<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher,
            SubscriberWithMessageInstanceAsync<TParameter> handler, MessageBusSubscriberOptions<TParameter>? options = default)
        {
            throw new NotImplementedException();
        }
    }
}
