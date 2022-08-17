using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBus
    {
        /// <inheritdoc />
        public override PublisherTicket<TParameter, TReturn> RegisterPublisher<TParameter, TReturn>(string messageName, MessageBusPublisherOptions<TParameter, TReturn>? options = default)
        {
            var publisherInfo = new PublisherInfo<TParameter, TReturn>(options);
            var id = AddPublisherToSequencer(messageName, publisherInfo);

            var ticket = new PublisherTicket<TParameter, TReturn>(id, options, publisherInfo.MessageExecutor);
            return ticket;
        }

        /// <inheritdoc />
        public override PublisherTicket<TParameter> RegisterPublisher<TParameter>(string messageName, MessageBusPublisherOptions<TParameter>? options = default)
        {
            var publisherInfo = new PublisherInfo<TParameter>(options);
            var id = AddPublisherToSequencer(messageName, publisherInfo);

            var ticket = new PublisherTicket<TParameter>(id, options, publisherInfo.MessageExecutor);
            return ticket;
        }

        /// <inheritdoc />
        // ReSharper disable once IdentifierTypo
        public override bool UnregisterPublisher(PublisherTicketBase publisherTicket)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            return publisherTicket != null && UnregisterPublisher(publisherTicket.Id);
        }

        /// <inheritdoc />
        // ReSharper disable once IdentifierTypo
        public override bool UnregisterPublisher(Guid publisherId)
        {
            if (TryRemovePublisherFromSequencer(publisherId, out var publisherInfo))
            {
                ((IDisposable)publisherInfo!.MessageExecutorGeneric).Dispose();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
