using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus.MessageNameMatching
{
    /// <summary>
    /// Indicates the subscriber will match all message names.
    /// </summary>
    public class MatchingAll : MessageNameMatcherBase
    {
        /// <summary>
        /// Gets the message name or pattern of the subscriber. Empty string is returned always.
        /// </summary>
        public override string SubscriberMessageNamePattern { get; } = string.Empty;

        /// <summary>
        /// Checks whether the message name specified by publisher registering process is complied with the subscriber required.
        /// </summary>
        /// <param name="publisherMessageName">The message name specified by publisher registering process.</param>
        /// <returns><see langword="true"/>.</returns>
        public override bool IsComplied(string publisherMessageName)
        {
            return true;
        }
    }
}
