using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.Behaviors;
using SecretNest.MessageBus.MessageNameMatching;

namespace SecretNest.MessageBus
{
    partial class MessageBusBase
    {
        /// <summary>
        /// Register a subscriber with parameter with the message name specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageName">The message name to be subscribed.</param>
        /// <param name="handler">The handler of the delegate with a parameter.</param>
        /// <param name="behaviors">The instances of subscriber behaviors. Default is <see langword="none"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, Subscriber<TParameter>> RegisterSubscriber<TParameter>(string messageName, Subscriber<TParameter> handler, SubscriberBehaviorCollection? behaviors = default);

        /// <summary>
        /// Register a subscriber with parameter using the message name matcher specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageNameMatcher">Use the matcher to traverse all message names to determine messages those need to be subscribed.</param>
        /// <param name="handler">The handler of the delegate with a parameter.</param>
        /// <param name="behaviors">The instances of subscriber behaviors. Default is <see langword="none"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, Subscriber<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher, Subscriber<TParameter> handler, SubscriberBehaviorCollection? behaviors = default);

        /// <summary>
        /// Register a subscriber with parameter with the message name specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageName">The message name to be subscribed.</param>
        /// <param name="handler">The handler of the delegate with a parameter.</param>
        /// <param name="behaviors">The instances of subscriber behaviors.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, Subscriber<TParameter>> RegisterSubscriber<TParameter>(string messageName, Subscriber<TParameter> handler, params SubscriberBehaviorBase[] behaviors);

        /// <summary>
        /// Register a subscriber with parameter using the message name matcher specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageNameMatcher">Use the matcher to traverse all message names to determine messages those need to be subscribed.</param>
        /// <param name="handler">The handler of the delegate with a parameter.</param>
        /// <param name="behaviors">The instances of subscriber behaviors.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, Subscriber<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher, Subscriber<TParameter> handler, params SubscriberBehaviorBase[] behaviors);

        /// <summary>
        /// Register a subscriber with parameter with the message name specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageName">The message name to be subscribed.</param>
        /// <param name="handler">The handler of the delegate with a parameter and a parameter of <see cref="MessageInstance"/>.</param>
        /// <param name="behaviors">The instances of subscriber behaviors. Default is <see langword="none"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, SubscriberWithMessageInstance<TParameter>> RegisterSubscriber<TParameter>(string messageName, SubscriberWithMessageInstance<TParameter> handler, SubscriberBehaviorCollection? behaviors = default);

        /// <summary>
        /// Register a subscriber with parameter using the message name matcher specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageNameMatcher">Use the matcher to traverse all message names to determine messages those need to be subscribed.</param>
        /// <param name="handler">The handler of the delegate with a parameter and a parameter of <see cref="MessageInstance"/>.</param>
        /// <param name="behaviors">The instances of subscriber behaviors. Default is <see langword="none"/>.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, SubscriberWithMessageInstance<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher, SubscriberWithMessageInstance<TParameter> handler, SubscriberBehaviorCollection? behaviors = default);

        /// <summary>
        /// Register a subscriber with parameter with the message name specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageName">The message name to be subscribed.</param>
        /// <param name="handler">The handler of the delegate with a parameter and a parameter of <see cref="MessageInstance"/>.</param>
        /// <param name="behaviors">The instances of subscriber behaviors.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, SubscriberWithMessageInstance<TParameter>> RegisterSubscriber<TParameter>(string messageName, SubscriberWithMessageInstance<TParameter> handler, params SubscriberBehaviorBase[] behaviors);

        /// <summary>
        /// Register a subscriber with parameter using the message name matcher specified.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="messageNameMatcher">Use the matcher to traverse all message names to determine messages those need to be subscribed.</param>
        /// <param name="handler">The handler of the delegate with a parameter and a parameter of <see cref="MessageInstance"/>.</param>
        /// <param name="behaviors">The instances of subscriber behaviors.</param>
        /// <returns>Subscriber ticket.</returns>
        public abstract SubscriberTicket<TParameter, SubscriberWithMessageInstance<TParameter>> RegisterSubscriber<TParameter>(MessageNameMatcherBase messageNameMatcher, SubscriberWithMessageInstance<TParameter> handler, params SubscriberBehaviorBase[] behaviors);
    }
}
