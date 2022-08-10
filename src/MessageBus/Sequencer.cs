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

            private MessageInstanceHelper Execute(object? argument)
            {
                return _sequencer.Execute(argument, _isAlwaysExecuteAll);
            }

            private async Task<MessageInstanceHelper> ExecuteAsync(object? argument, CancellationToken cancellationToken)
            {
                return await _sequencer.ExecuteAsync(argument, _isAlwaysExecuteAll, cancellationToken);
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

        private SubscriberInfoBase[] GetOrderedSubscribers()
        {
            lock (OrderedSubscribers)
            {
                return OrderedSubscribers.ToArray();
            }
        }

        private MessageInstanceHelper Execute(object? argument, bool isAlwaysExecuteAll)
        {
            var subscribers = GetOrderedSubscribers();

            var subscribersCount = subscribers.Length;

            var currentSubscriberIndex = 0;

            var messageInstanceHelper = new MessageInstanceHelper(MessageName);

            //get the first result
            for (; currentSubscriberIndex < subscribersCount; )
            {
                subscribers[currentSubscriberIndex].Execute(argument, messageInstanceHelper);
                currentSubscriberIndex++;
                if (messageInstanceHelper.IsSubscriberResultSet)
                {
                    break;
                }
            }

            //others
            for (; currentSubscriberIndex < subscribersCount; currentSubscriberIndex++)
            {
                var subscriber = subscribers[currentSubscriberIndex];
                if (isAlwaysExecuteAll || subscriber.IsAlwaysExecution)
                {
                    subscriber.ExecuteForce(argument, messageInstanceHelper);
                }
            }

            return messageInstanceHelper;
        }

        private async Task<MessageInstanceHelper> ExecuteAsync(object? argument, bool isAlwaysExecuteAll, CancellationToken cancellationToken)
        {

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
