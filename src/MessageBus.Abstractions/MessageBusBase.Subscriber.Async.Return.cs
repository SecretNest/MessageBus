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
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        /// <param name="messageName">The message name to be subscribed.</param>
        /// <param name="handler">The handler of the async delegate with a parameter, a parameter of <see cref="CancellationToken"/> and return.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="none"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, TReturn, SubscriberAsync<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(string messageName, SubscriberAsync<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default);

        /// <summary>
        /// Register a subscriber with parameter and return value using the message name matcher specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        /// <param name="messageNameMatcher">Use the matcher to traverse all message names to determine messages those need to be subscribed.</param>
        /// <param name="handler">The handler of the async delegate with a parameter, a parameter of <see cref="CancellationToken"/> and return.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="none"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, TReturn, SubscriberAsync<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(MessageNameMatcherBase messageNameMatcher, SubscriberAsync<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default);

        /// <summary>
        /// Register a subscriber with parameter and return value with the message name specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        /// <param name="messageName">The message name to be subscribed.</param>
        /// <param name="handler">The handler of the async delegate with a parameter, a parameter of <see cref="MessageInstance"/>, a parameter of <see cref="CancellationToken"/> and return.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="none"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstanceAsync<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(string messageName, SubscriberWithMessageInstanceAsync<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default);

        /// <summary>
        /// Register a subscriber with parameter and return value using the message name matcher specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        /// <param name="messageNameMatcher">Use the matcher to traverse all message names to determine messages those need to be subscribed.</param>
        /// <param name="handler">The handler of the async delegate with a parameter, a parameter of <see cref="MessageInstance"/>, a parameter of <see cref="CancellationToken"/> and return.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="none"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstanceAsync<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(MessageNameMatcherBase messageNameMatcher, SubscriberWithMessageInstanceAsync<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default);

    }
}
