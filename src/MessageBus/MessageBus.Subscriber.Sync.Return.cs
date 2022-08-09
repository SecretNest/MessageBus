using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBus
    {
        /// <inheritdoc />
        public override SubscriberTicket<TParameter, TReturn, Subscriber<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(string messageName, Subscriber<TParameter, TReturn> handler,
            MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            var subscriberInfo = new SubscriberInfo<TParameter, TReturn>(messageName, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, TReturn, Subscriber<TParameter, TReturn>>(id, false, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }

        /// <inheritdoc />
        public override SubscriberTicket<TParameter, TReturn, Subscriber<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(MessageNameMatcherBase messageNameMatcher, Subscriber<TParameter, TReturn> handler,
            MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            var subscriberInfo = new SubscriberInfo<TParameter, TReturn>(messageNameMatcher, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, TReturn, Subscriber<TParameter, TReturn>>(id, false, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }

        /// <inheritdoc />
        public override SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstance<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(string messageName, SubscriberWithMessageInstance<TParameter, TReturn> handler,
            MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            var subscriberInfo = new SubscriberInfoWithMessageInstance<TParameter, TReturn>(messageName, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstance<TParameter, TReturn>>(id, false, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }

        /// <inheritdoc />
        public override SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstance<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(MessageNameMatcherBase messageNameMatcher,
            SubscriberWithMessageInstance<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            var subscriberInfo = new SubscriberInfoWithMessageInstance<TParameter, TReturn>(messageNameMatcher, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstance<TParameter, TReturn>>(id, false, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }
    }
}
