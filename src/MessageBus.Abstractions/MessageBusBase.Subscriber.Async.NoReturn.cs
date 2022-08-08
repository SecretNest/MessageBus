using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBusBase
    {
        /// <summary>
        /// Register a subscriber with parameter and return value with the message name specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageName">The message name to be subscribed.</param>
        /// <param name="handler">The handler of the async delegate with a parameter and a parameter of <see cref="CancellationToken"/>.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="none"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, SubscriberAsync<TParameter>> RegisterSubscriber<TParameter>(string messageName, SubscriberAsync<TParameter> handler, MessageBusSubscriberOptions<TParameter>? options = default);

        /// <summary>
        /// Register a subscriber with parameter and return value using the message name matcher specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageNameMatcher">Use the matcher to traverse all message names to determine messages those need to be subscribed.</param>
        /// <param name="handler">The handler of the async delegate with a parameter and a parameter of <see cref="CancellationToken"/>.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="none"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, SubscriberAsync<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher, SubscriberAsync<TParameter> handler, MessageBusSubscriberOptions<TParameter>? options = default);

        /// <summary>
        /// Register a subscriber with parameter and return value with the message name specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageName">The message name to be subscribed.</param>
        /// <param name="handler">The handler of the async delegate with a parameter, a parameter of <see cref="MessageInstance"/> and a parameter of <see cref="CancellationToken"/>.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="none"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, SubscriberWithMessageInstanceAsync<TParameter>> RegisterSubscriber<TParameter>(string messageName, SubscriberWithMessageInstanceAsync<TParameter> handler, MessageBusSubscriberOptions<TParameter>? options = default);

        /// <summary>
        /// Register a subscriber with parameter and return value using the message name matcher specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageNameMatcher">Use the matcher to traverse all message names to determine messages those need to be subscribed.</param>
        /// <param name="handler">The handler of the async delegate with a parameter, a parameter of <see cref="MessageInstance"/> and a parameter of <see cref="CancellationToken"/>.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="none"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, SubscriberWithMessageInstanceAsync<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher, SubscriberWithMessageInstanceAsync<TParameter> handler, MessageBusSubscriberOptions<TParameter>? options = default);
    }
}
