using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBus
    {
        private readonly ConcurrentDictionary<string, Sequencer> _sequencers = new ConcurrentDictionary<string, Sequencer>();

        private readonly ConcurrentDictionary<Guid, string> _publisherMessageNames = new ConcurrentDictionary<Guid, string>();

        private Guid AddPublisherToSequencer(PublisherBase publisher)
        {
            var messageName = publisher.MessageName;
            var sequencer = _sequencers.GetOrAdd(messageName, _ =>
            {
                var sequencer = new Sequencer(GetSubscribersFromPool(messageName));
                return sequencer;
            });

            var key = Guid.NewGuid();
            sequencer.AddPublisher(key, publisher);
            _publisherMessageNames[key] = messageName;
            return key;
        }

        private bool TryRemovePublisherFromSequencer(Guid key, out PublisherBase? publisher)
        {
            if (!_publisherMessageNames.TryGetValue(key, out var messageName)
                || _sequencers.TryGetValue(messageName, out var sequencer))
            {
                publisher = default;
                return false;
            }

            return sequencer!.TryRemovePublisher(key, out publisher);
        }

        private void AddSubscriberToSequencer(Guid key, SubscriberBase subscriber)
        {
            foreach (var sequencer in _sequencers)
            {
                if (subscriber.MessageNameMatcher.IsComplied(sequencer.Key))
                {
                    sequencer.Value.AddSubscriber(key, subscriber);
                }
            }
        }

        private void RemoveSubscriberFromSequencer(Guid key)
        {
            foreach (var sequencer in _sequencers)
            {
                sequencer.Value.RemoveSubscriber(key);
            }
        }

        private void ShrinkSequencers()
        {
            var messageNames = _sequencers.Where(i => i.Value.Publishers.IsEmpty).Select(i => i.Key).ToArray();
            foreach (var messageName in messageNames)
            {
                _sequencers.Remove(messageName, out _);
            }
        }
    }
}
