using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus
{
    internal class Sequencer
    {
        public string MessageName { get; }
        public ConcurrentDictionary<Guid, PublisherInfoBase> Publishers { get; }
        public ConcurrentDictionary<Guid, SubscriberInfoBase> Subscribers { get; }
        public List<SubscriberInfoBase> OrderedSubscribers { get; }

        public Sequencer(string messageName, IEnumerable<KeyValuePair<Guid, SubscriberInfoBase>> subscribers)
        {
            MessageName = messageName;
            Publishers = new ConcurrentDictionary<Guid, PublisherInfoBase>();
            Subscribers = new ConcurrentDictionary<Guid, SubscriberInfoBase>(subscribers);
            OrderedSubscribers = new(Subscribers.Values.OrderBy(i=>i.Sequence));
        }

        class SequencerEntry
        {
            private readonly bool _isAlwaysExecuteAll;
            private readonly Sequencer _sequencer;

            private AcceptedReturn? Execute(object? argument, MessageInstance? messageInstance)
            {
                return _sequencer.Execute(argument, messageInstance, _isAlwaysExecuteAll);
            }

            private async Task<AcceptedReturn?> ExecuteAsync(object? argument, MessageInstance? messageInstance,
                CancellationToken cancellationToken)
            {
                return await _sequencer.ExecuteAsync(argument, messageInstance, _isAlwaysExecuteAll, cancellationToken);
            }

            public SequencerEntry(PublisherInfoBase publisher, Sequencer sequencer)
            {
                _sequencer = sequencer;
                _isAlwaysExecuteAll = publisher.IsAlwaysExecuteAll;
                publisher.MessageExecutorSequencerSupport.OnAddedToSequencer(Execute, ExecuteAsync);
            }
        }

        public void AddPublisher(Guid key, PublisherInfoBase publisher)
        {
            if (Publishers.TryAdd(key, publisher))
            {
                _ = new SequencerEntry(publisher, this);
                publisher.MessageExecutorSequencerSupport.OnAddedToSequencer(GetMessageInstance);
            }
        }

        public bool TryRemovePublisher(Guid key, out PublisherInfoBase? publisher)
        {
            if (Publishers.TryRemove(key, out publisher))
            {
                publisher.MessageExecutorSequencerSupport.OnRemovedFromSequencer();
                return true;
            }
            else
            {
                publisher = null;
                return false;
            }
        }

        private AcceptedReturn? Execute(object? argument, MessageInstance? messageInstance, bool isAlwaysExecuteAll)
        {

        }

        private async Task<AcceptedReturn?> ExecuteAsync(object? argument, MessageInstance? messageInstance, bool isAlwaysExecuteAll, CancellationToken cancellationToken)
        {

        }

        private MessageInstance GetMessageInstance()
        {
            return new MessageInstance(Guid.NewGuid(), MessageName);
        }

        public void AddSubscriber(Guid subscriberId, SubscriberInfoBase subscriber)
        {
            if (Subscribers.TryAdd(subscriberId, subscriber))
            {
                lock (OrderedSubscribers)
                {
                    var mySequence = subscriber.Sequence;
                    var index = OrderedSubscribers.FindIndex(i =>
                        i.Sequence > mySequence);
                    if (index == -1)
                    {
                        OrderedSubscribers.Add(subscriber);
                    }
                    else
                    {
                        OrderedSubscribers.Insert(index, subscriber);
                    }
                }
            }
        }

        public void RemoveSubscriber(Guid subscriberId)
        {
            if (Subscribers.TryRemove(subscriberId, out var subscriber))
            {
                lock (OrderedSubscribers)
                {
                    OrderedSubscribers.Remove(subscriber);
                }
            }
        }
    }
}
