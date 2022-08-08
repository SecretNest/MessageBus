using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBus
    {
        public override PublisherTicket<TParameter, TReturn> RegisterPublisher<TParameter, TReturn>(string messageName,
            MessageBusPublisherOptions<TParameter, TReturn>? options = default)
        {
            throw new NotImplementedException();
        }

        public override PublisherTicket<TParameter> RegisterVoidPublisher<TParameter>(string messageName, MessageBusPublisherOptions<TParameter>? options = default)
        {
            throw new NotImplementedException();
        }

        // ReSharper disable once IdentifierTypo
        public override bool UnregisterPublisher(PublisherTicketBase publisherTicket)
        {
            throw new NotImplementedException();
        }

        // ReSharper disable once IdentifierTypo
        public override bool UnregisterPublisher(Guid publisherId)
        {
            throw new NotImplementedException();
        }
    }
}
