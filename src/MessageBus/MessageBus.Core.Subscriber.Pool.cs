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
        private ConcurrentDictionary<Guid, SubscriberInfoBase> _subscribersMatchAll = new ConcurrentDictionary<Guid, SubscriberInfoBase>();

        private ConcurrentDictionary<string, HashSet<Guid>> _subscribersNames = new ConcurrentDictionary<string, HashSet<Guid>>();
        private ConcurrentDictionary<string, HashSet<Guid>> _subscribersNamesIgnoreCase = new ConcurrentDictionary<string, HashSet<Guid>>(StringComparer.OrdinalIgnoreCase);
        private ConcurrentDictionary<Guid, SubscriberInfoBase> _subscribersMatchName = new ConcurrentDictionary<Guid, SubscriberInfoBase>();

        private ConcurrentDictionary<Guid, SubscriberInfoBase> _subscribersMatchOther = new ConcurrentDictionary<Guid, SubscriberInfoBase>();

        private Guid AddSubscriberToPool(SubscriberInfoBase subscriber)
        {
            var subscriberId = Guid.NewGuid();
            Add();
            subscriber.SubscriberId = subscriberId;
            return subscriberId;

            void Add()
            {
                var messageNameMatcherType = subscriber.MessageNameMatcher.GetType();
                if (messageNameMatcherType == typeof(MessageNameMatchingAll))
                {
                    _subscribersMatchAll.TryAdd(subscriberId, subscriber);
                    return;
                }
                else if (messageNameMatcherType == typeof(MessageNameMatchingWithStringComparison))
                {
                    _subscribersMatchName.TryAdd(subscriberId, subscriber);
                    var matcher = subscriber.MessageNameMatcher as MessageNameMatchingWithStringComparison;
                    if (matcher!.StringComparison == StringComparison.Ordinal)
                    {
                        var hashSet = _subscribersNames.GetOrAdd(matcher.SubscriberMessageName,
                            _ => new HashSet<Guid>());
                        lock (hashSet)
                        {
                            hashSet.Add(subscriberId);
                            return;
                        }
                    }
                    else if (matcher.StringComparison == StringComparison.OrdinalIgnoreCase)
                    {
                        var hashSet = _subscribersNamesIgnoreCase.GetOrAdd(matcher.SubscriberMessageName,
                            _ => new HashSet<Guid>());
                        lock (hashSet)
                        {
                            hashSet.Add(subscriberId);
                            return;
                        }
                    }
                }
                else
                {
                    _subscribersMatchOther.TryAdd(subscriberId, subscriber);
                    return;
                }
            }
        }

        private bool TryRemoveSubscriberFromPool(Guid subscriberId, out SubscriberInfoBase subscriber)
        {
            if (_subscribersMatchAll.TryRemove(subscriberId, out subscriber))
            {
                return true;
            }
            else if (_subscribersMatchName.TryRemove(subscriberId, out subscriber))
            {
                var matcher = subscriber.MessageNameMatcher as MessageNameMatchingWithStringComparison;
                if (matcher!.StringComparison == StringComparison.Ordinal)
                {
                    if (_subscribersNames.TryGetValue(matcher.SubscriberMessageName, out var hashSet))
                    {
                        lock (hashSet)
                        {
                            hashSet.Remove(subscriberId);
                        }
                    }
                    else if (_subscribersNamesIgnoreCase.TryGetValue(matcher.SubscriberMessageName, out hashSet))
                    {
                        lock (hashSet)
                        {
                            hashSet.Remove(subscriberId);
                        }
                    }
                }
                return true;
            }
            else
            {
                return _subscribersMatchOther.TryRemove(subscriberId, out subscriber);
            }
        }

        private IEnumerable<KeyValuePair<Guid, SubscriberInfoBase>> GetSubscribersFromPool(string messageName)
        {
            foreach (var item in _subscribersMatchAll)
            {
                yield return item;
            }

            if (_subscribersNames.TryGetValue(messageName, out var hashSet))
            {
                foreach (var id in hashSet)
                {
                    if (_subscribersMatchName.TryGetValue(id, out var subscriber))
                    {
                        yield return new KeyValuePair<Guid, SubscriberInfoBase>(id, subscriber);
                    }
                }
            }

            if (_subscribersNamesIgnoreCase.TryGetValue(messageName, out hashSet))
            {
                foreach (var id in hashSet)
                {
                    if (_subscribersMatchName.TryGetValue(id, out var subscriber))
                    {
                        yield return new KeyValuePair<Guid, SubscriberInfoBase>(id, subscriber);
                    }
                }
            }

            foreach (var item in _subscribersMatchOther)
            {
                if (item.Value.MessageNameMatcher.IsComplied(messageName))
                    yield return item;
            }
        }

        private void OnShutdownSubscriberPool()
        {
            _subscribersMatchAll = null!;
            _subscribersNames = null!;
            _subscribersNamesIgnoreCase = null!;
            _subscribersMatchName = null!;
            _subscribersMatchOther = null!;
        }
    }
}
