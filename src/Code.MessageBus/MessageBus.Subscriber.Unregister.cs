using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus
{
    partial class MessageBus
    {
        /// <inheritdoc />
        // ReSharper disable once IdentifierTypo
        public override bool UnregisterSubscriber(SubscriberTicketBase subscriberTicket)
        {
            return UnregisterSubscriber(subscriberTicket.Id);
        }

        /// <inheritdoc />
        // ReSharper disable once IdentifierTypo
        public override bool UnregisterSubscriber(Guid subscriberId)
        {
            if (TryRemoveSubscriberFromPool(subscriberId, out var subscriber))
            {
                RemoveSubscriberFromSequencer(subscriberId);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
