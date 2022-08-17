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
        public override SubscriberTicket<TParameter, SubscriberAsync<TParameter>> RegisterSubscriber<TParameter>(string messageName, SubscriberAsync<TParameter> handler,
            MessageBusSubscriberOptions<TParameter>? options = default)
        {
            var subscriberInfo = new SubscriberAsyncInfo<TParameter>(messageName, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, SubscriberAsync<TParameter>>(id, true, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }

        /// <inheritdoc />
        public override SubscriberTicket<TParameter, SubscriberAsync<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher, SubscriberAsync<TParameter> handler,
            MessageBusSubscriberOptions<TParameter>? options = default)
        {
            var subscriberInfo = new SubscriberAsyncInfo<TParameter>(messageNameMatcher, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, SubscriberAsync<TParameter>>(id, true, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }

        /// <inheritdoc />
        public override SubscriberTicket<TParameter, SubscriberWithMessageInstanceAsync<TParameter>> RegisterSubscriber<TParameter>(string messageName, SubscriberWithMessageInstanceAsync<TParameter> handler,
            MessageBusSubscriberOptions<TParameter>? options = default)
        {
            var subscriberInfo = new SubscriberAsyncInfoWithMessageInstance<TParameter>(messageName, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, SubscriberWithMessageInstanceAsync<TParameter>>(id, true, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }

        /// <inheritdoc />
        public override SubscriberTicket<TParameter, SubscriberWithMessageInstanceAsync<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher,
            SubscriberWithMessageInstanceAsync<TParameter> handler, MessageBusSubscriberOptions<TParameter>? options = default)
        {
            var subscriberInfo = new SubscriberAsyncInfoWithMessageInstance<TParameter>(messageNameMatcher, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, SubscriberWithMessageInstanceAsync<TParameter>>(id, true, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }
    }
}
