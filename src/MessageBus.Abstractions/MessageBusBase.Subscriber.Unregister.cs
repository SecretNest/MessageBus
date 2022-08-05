using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus
{
    partial class MessageBusBase
    {
        // ReSharper disable once CommentTypo
        /// <summary>
        /// Unregister a subscriber.
        /// </summary>
        /// <param name="subscriberTicket">The ticket of the subscriber to be unregistered.</param>
        // ReSharper disable once IdentifierTypo
        public abstract void UnregisterSubscriber(SubscriberTicketBase subscriberTicket);

        // ReSharper disable once CommentTypo
        /// <summary>
        /// Unregister a subscriber.
        /// </summary>
        /// <param name="subscriberId">The id of the subscriber to be unregistered.</param>
        // ReSharper disable once IdentifierTypo
        public abstract void UnregisterSubscriber(Guid subscriberId);
    }
}
