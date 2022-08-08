using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus
{
    partial class MessageBus
    {
        // ReSharper disable once IdentifierTypo
        public override bool UnregisterSubscriber(SubscriberTicketBase subscriberTicket)
        {
            throw new NotImplementedException();
        }

        // ReSharper disable once IdentifierTypo
        public override bool UnregisterSubscriber(Guid subscriberId)
        {
            throw new NotImplementedException();
        }
    }
}
