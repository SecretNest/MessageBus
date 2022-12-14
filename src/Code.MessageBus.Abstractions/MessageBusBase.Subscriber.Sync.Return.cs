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
        /// <param name="handler">The handler of the delegate with a parameter and return.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="null"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, TReturn, Subscriber<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(string messageName, Subscriber<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default);

        /// <summary>
        /// Register a subscriber with parameter and return value using the message name matcher specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        /// <param name="messageNameMatcher">Use the matcher to traverse all message names to determine messages those need to be subscribed.</param>
        /// <param name="handler">The handler of the delegate with a parameter and return.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="null"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, TReturn, Subscriber<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(MessageNameMatcherBase messageNameMatcher, Subscriber<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default);

        /// <summary>
        /// Register a subscriber with parameter and return value with the message name specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        /// <param name="messageName">The message name to be subscribed.</param>
        /// <param name="handler">The handler of the delegate with a parameter, a parameter of <see cref="MessageInstance"/> and return.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="null"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstance<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(string messageName, SubscriberWithMessageInstance<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default);

        /// <summary>
        /// Register a subscriber with parameter and return value using the message name matcher specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        /// <param name="messageNameMatcher">Use the matcher to traverse all message names to determine messages those need to be subscribed.</param>
        /// <param name="handler">The handler of the delegate with a parameter, a parameter of <see cref="MessageInstance"/> and return.</param>
        /// <param name="options">The instance of subscriber options. Default is <see langword="null"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, TReturn, SubscriberWithMessageInstance<TParameter, TReturn>> RegisterSubscriber<TParameter, TReturn>(MessageNameMatcherBase messageNameMatcher, SubscriberWithMessageInstance<TParameter, TReturn> handler, MessageBusSubscriberOptions<TParameter, TReturn>? options = default);
    }
}
