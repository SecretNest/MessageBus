using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBus
    {
        public override SubscriberTicket<TParameter, TReturn, Subscriber<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(string messageName, Subscriber<TParameter, TReturn> handler,
            MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            throw new NotImplementedException();
        }

        public override SubscriberTicket<TParameter, TReturn, Subscriber<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(MessageNameMatcherBase messageNameMatcher, Subscriber<TParameter, TReturn> handler,
            MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            throw new NotImplementedException();
        }

        public override SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstance<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(string messageName, SubscriberWithMessageInstance<TParameter, TReturn> handler,
            MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            throw new NotImplementedException();
        }

        public override SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstance<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(MessageNameMatcherBase messageNameMatcher,
            SubscriberWithMessageInstance<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            throw new NotImplementedException();
        }
    }
}
