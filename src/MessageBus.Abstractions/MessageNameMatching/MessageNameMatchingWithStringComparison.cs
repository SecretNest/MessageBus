using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus.MessageNameMatching
{
    /// <summary>
    /// Indicates the message name should be compared as string.
    /// </summary>
    public class MessageNameMatchingWithStringComparison : MessageNameMatcherBase
    {
        /// <summary>
        /// Initializes an instance of MatchingWithStringComparison.
        /// </summary>
        /// <param name="subscriberMessageName">The message name of the subscriber.</param>
        /// <param name="stringComparison">The culture, case, and sort rules to be used by <see cref="IsComplied"/>. Default value is StringComparison.OrdinalIgnoreCase.</param>
        public MessageNameMatchingWithStringComparison(string subscriberMessageName, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            SubscriberMessageName = subscriberMessageName;
            StringComparison = stringComparison;
        }

        /// <summary>
        /// Gets the message name of the subscriber.
        /// </summary>
        public override string SubscriberMessageNamePattern => SubscriberMessageName;

        /// <summary>
        /// Gets the message name of the subscriber.
        /// </summary>
        public string SubscriberMessageName { get; }

        /// <summary>
        /// Gets the culture, case, and sort rules to be used by <see cref="IsComplied"/>.
        /// </summary>
        public StringComparison StringComparison { get; }

        /// <inheritdoc />
        public override bool IsComplied(string publisherMessageName)
        {
            return publisherMessageName.Equals(SubscriberMessageNamePattern, StringComparison);
        }
    }
}
