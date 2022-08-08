using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus
{
    internal class Sequencer
    {
        public ConcurrentDictionary<Guid, PublisherBase> Publishers { get; }

        public Sequencer(IEnumerable<KeyValuePair<Guid, SubscriberBase>> subscribers)
        {
            Publishers = new ConcurrentDictionary<Guid, PublisherBase>();

        }

        public void AddPublisher(Guid key, PublisherBase publisher)
        {

        }

        public bool TryRemovePublisher(Guid key, out PublisherBase? publisher)
        {

        }

        public void AddSubscriber(Guid key, SubscriberBase subscriber)
        {

        }

        public void RemoveSubscriber(Guid key)
        {

        }
    }
}
