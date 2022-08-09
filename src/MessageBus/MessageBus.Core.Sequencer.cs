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

        private Guid AddPublisherToSequencer(string messageName, PublisherInfoBase publisher)
        {
            var sequencer = _sequencers.GetOrAdd(messageName, _ =>
            {
                var sequencer = new Sequencer(messageName, GetSubscribersFromPool(messageName));
                return sequencer;
            });

            var key = Guid.NewGuid();
            sequencer.AddPublisher(key, publisher);
            _publisherMessageNames[key] = messageName;
            return key;
        }

        private bool TryRemovePublisherFromSequencer(Guid key, out PublisherInfoBase? publisher)
        {
            if (!_publisherMessageNames.TryGetValue(key, out var messageName)
                || _sequencers.TryGetValue(messageName, out var sequencer))
            {
                publisher = default;
                return false;
            }

            var result = sequencer!.TryRemovePublisher(key, out publisher);
            if (AutoShrink)
            {
                if (sequencer.Publishers.IsEmpty)
                {
                    _sequencers.Remove(messageName, out _);
                }
            }

            return result;
        }

        private void AddSubscriberToSequencer(Guid subscriberId, SubscriberInfoBase subscriber)
        {
            foreach (var sequencer in _sequencers)
            {
                if (subscriber.MessageNameMatcher.IsComplied(sequencer.Key))
                {
                    sequencer.Value.AddSubscriber(subscriberId, subscriber);
                }
            }
        }

        private void RemoveSubscriberFromSequencer(Guid subscriberId)
        {
            foreach (var sequencer in _sequencers)
            {
                sequencer.Value.RemoveSubscriber(subscriberId);
            }
        }
    }
}
