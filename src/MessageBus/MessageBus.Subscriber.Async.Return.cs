using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBus
    {
        public override SubscriberTicket<TParameter, TReturn, SubscriberAsync<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(string messageName, SubscriberAsync<TParameter, TReturn> handler,
            MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            throw new NotImplementedException();
        }

        public override SubscriberTicket<TParameter, TReturn, SubscriberAsync<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(MessageNameMatcherBase messageNameMatcher,
            SubscriberAsync<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            throw new NotImplementedException();
        }

        public override SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstanceAsync<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(string messageName, SubscriberWithMessageInstanceAsync<TParameter, TReturn> handler,
            MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            throw new NotImplementedException();
        }

        public override SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstanceAsync<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(MessageNameMatcherBase messageNameMatcher,
            SubscriberWithMessageInstanceAsync<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            throw new NotImplementedException();
        }
    }
}
