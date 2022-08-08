using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.MessageNameMatching;

namespace SecretNest.MessageBus
{
    internal abstract class SubscriberBase
    {

        public MessageNameMatcherBase MessageNameMatcher { get; }
    }
}
