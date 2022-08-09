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
        public override SubscriberTicket<TParameter, Subscriber<TParameter>> RegisterSubscriber<TParameter>(string messageName, Subscriber<TParameter> handler,
            MessageBusSubscriberOptions<TParameter>? options = default)
        {
            var subscriberInfo = new SubscriberInfo<TParameter>(messageName, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, Subscriber<TParameter>>(id, false, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }

        /// <inheritdoc />
        public override SubscriberTicket<TParameter, Subscriber<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher, Subscriber<TParameter> handler,
            MessageBusSubscriberOptions<TParameter>? options = default)
        {
            var subscriberInfo = new SubscriberInfo<TParameter>(messageNameMatcher, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, Subscriber<TParameter>>(id, false, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }

        /// <inheritdoc />
        public override SubscriberTicket<TParameter, SubscriberWithMessageInstance<TParameter>> RegisterSubscriber<TParameter>(string messageName, SubscriberWithMessageInstance<TParameter> handler,
            MessageBusSubscriberOptions<TParameter>? options = default)
        {
            var subscriberInfo = new SubscriberInfoWithMessageInstance<TParameter>(messageName, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, SubscriberWithMessageInstance<TParameter>>(id, false, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }

        /// <inheritdoc />
        public override SubscriberTicket<TParameter, SubscriberWithMessageInstance<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher,
            SubscriberWithMessageInstance<TParameter> handler, MessageBusSubscriberOptions<TParameter>? options = default)
        {
            var subscriberInfo = new SubscriberInfoWithMessageInstance<TParameter>(messageNameMatcher, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, SubscriberWithMessageInstance<TParameter>>(id, false, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }
    }
}
