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
        public override SubscriberTicket<TParameter, TReturn, SubscriberAsync<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(string messageName, SubscriberAsync<TParameter, TReturn> handler,
            MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            var subscriberInfo = new SubscriberAsyncInfo<TParameter, TReturn>(messageName, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, TReturn, SubscriberAsync<TParameter, TReturn>>(id, false, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }

        /// <inheritdoc />
        public override SubscriberTicket<TParameter, TReturn, SubscriberAsync<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(MessageNameMatcherBase messageNameMatcher,
            SubscriberAsync<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            var subscriberInfo = new SubscriberAsyncInfo<TParameter, TReturn>(messageNameMatcher, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, TReturn, SubscriberAsync<TParameter, TReturn>>(id, false, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }

        /// <inheritdoc />
        public override SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstanceAsync<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(string messageName, SubscriberWithMessageInstanceAsync<TParameter, TReturn> handler,
            MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            var subscriberInfo = new SubscriberAsyncInfoWithMessageInstance<TParameter, TReturn>(messageName, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstanceAsync<TParameter, TReturn>>(id, false, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }

        /// <inheritdoc />
        public override SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstanceAsync<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(MessageNameMatcherBase messageNameMatcher,
            SubscriberWithMessageInstanceAsync<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default)
        {
            var subscriberInfo = new SubscriberAsyncInfoWithMessageInstance<TParameter, TReturn>(messageNameMatcher, options, handler);
            var id = AddSubscriberToPool(subscriberInfo);
            AddSubscriberToSequencer(id, subscriberInfo);

            var ticket = new SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstanceAsync<TParameter, TReturn>>(id, false, options, handler, subscriberInfo.MessageNameMatcher);
            return ticket;
        }
    }
}
