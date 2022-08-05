using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SecretNest.MessageBus.MessageNameMatching
{
    /// <summary>
    /// Indicates the message name should be compared by regular expression.
    /// </summary>
    public class MatchingWithRegularExpression : MessageNameMatcherBase
    {
        private Regex _regex;

        /// <summary>
        /// Initializes an instance of MatchingWithRegularExpression.
        /// </summary>
        /// <param name="regEx">The regular expression to be used for matching the message name specified by publisher registering process.</param>
        public MatchingWithRegularExpression(string regEx)
        {
            RegularExpression = regEx;
            _regex = new Regex(regEx, RegexOptions.Compiled);
        }

        /// <summary>
        /// Gets the message name pattern of the subscriber.
        /// </summary>
        public override string SubscriberMessageNamePattern => RegularExpression;

        /// <summary>
        /// Gets the message name pattern of the subscriber.
        /// </summary>
        public string RegularExpression { get; }

        /// <inheritdoc />
        public override bool IsComplied(string publisherMessageName)
        {
            return _regex.IsMatch(publisherMessageName);
        }
    }
}
