using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                return await _sequencer.ExecuteAsync(argument, _isAlwaysExecuteAll, cancellationToken).ConfigureAwait(false);
            }

            public SequencerEntry(PublisherInfoBase publisher, Sequencer sequencer)
            {
                _sequencer = sequencer;
                _isAlwaysExecuteAll = publisher.IsAlwaysExecuteAll;
                publisher.MessageExecutorSequencerSupport.OnAddedToSequencer(Execute, ExecuteAsync);
            }
        }

        public void OnShutdown()
        {
            foreach (var publisher in Publishers.Values)
            {
                publisher.MessageExecutorSequencerSupport.OnRemovedFromSequencer();
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

        public static MessageInstanceHelper ExecuteOnce(string messageName, 
            SubscriberInfoBase[] subscribers, 
            object? argument, bool isAlwaysExecuteAll)
        {
            var subscribersCount = subscribers.Length;

            var currentSubscriberIndex = 0;

            var messageInstanceHelper = new MessageInstanceHelper(messageName);

            //get the first result
            while (currentSubscriberIndex < subscribersCount)
            {
                var subscriber = subscribers[currentSubscriberIndex];
                if (subscriber.ConditionCheckingCallback != null)
                {
                    if (!subscriber.ConditionCheckingCallback(argument))
                    {
                        currentSubscriberIndex++;
                        continue;
                    }
                }
                subscriber.Execute(argument, messageInstanceHelper);
                currentSubscriberIndex++;
                if (messageInstanceHelper.IsSubscriberResultSet)
                {
                    break;
                }
            }

            //others
            while (currentSubscriberIndex < subscribersCount)
            {
                var subscriber = subscribers[currentSubscriberIndex];
                if (subscriber.ConditionCheckingCallback != null)
                {
                    if (!subscriber.ConditionCheckingCallback(argument))
                    {
                        currentSubscriberIndex++;
                        continue;
                    }
                }
                if (isAlwaysExecuteAll || subscriber.IsAlwaysExecution)
                {
                    subscriber.ExecuteForce(argument, messageInstanceHelper);
                }
                currentSubscriberIndex++;
            }

            return messageInstanceHelper;
        }

        public static async Task<MessageInstanceHelper> ExecuteOnceAsync(string messageName,
            SubscriberInfoBase[] subscribers,
            object? argument, bool isAlwaysExecuteAll,
            CancellationToken cancellationToken)
        {
            var subscribersCount = subscribers.Length;

            var currentSubscriberIndex = 0;

            var messageInstanceHelper = new MessageInstanceHelper(messageName);

            //get the first result
            while (currentSubscriberIndex < subscribersCount)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var subscriber = subscribers[currentSubscriberIndex];
                if (subscriber.ConditionCheckingCallback != null)
                {
                    if (!subscriber.ConditionCheckingCallback(argument))
                    {
                        currentSubscriberIndex++;
                        continue;
                    }
                }
                await subscriber.ExecuteAsync(argument, messageInstanceHelper, cancellationToken).ConfigureAwait(false);
                currentSubscriberIndex++;
                if (messageInstanceHelper.IsSubscriberResultSet)
                {
                    break;
                }
            }

            //others
            while (currentSubscriberIndex < subscribersCount)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var subscriber = subscribers[currentSubscriberIndex];
                if (subscriber.ConditionCheckingCallback != null)
                {
                    if (!subscriber.ConditionCheckingCallback(argument))
                    {
                        currentSubscriberIndex++;
                        continue;
                    }
                }
                if (isAlwaysExecuteAll || subscriber.IsAlwaysExecution)
                {
                    await subscriber.ExecuteForceAsync(argument, messageInstanceHelper, cancellationToken).ConfigureAwait(false);
                }
                currentSubscriberIndex++;
            }

            return messageInstanceHelper;

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

            return ExecuteOnce(MessageName, subscribers, argument, isAlwaysExecuteAll);
        }

        private async Task<MessageInstanceHelper> ExecuteAsync(object? argument, bool isAlwaysExecuteAll, CancellationToken cancellationToken)
        {
            var subscribers = GetOrderedSubscribers();

            return await ExecuteOnceAsync(MessageName, subscribers, argument, isAlwaysExecuteAll, cancellationToken).ConfigureAwait(false);
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
