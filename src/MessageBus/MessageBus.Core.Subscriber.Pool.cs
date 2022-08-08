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
        private readonly ConcurrentDictionary<Guid, SubscriberBase> _subscribersMatchAll = new ConcurrentDictionary<Guid, SubscriberBase>();

        private readonly ConcurrentDictionary<string, HashSet<Guid>> _subscribersNames = new ConcurrentDictionary<string, HashSet<Guid>>();
        private readonly ConcurrentDictionary<string, HashSet<Guid>> _subscribersNamesIgnoreCase = new ConcurrentDictionary<string, HashSet<Guid>>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<Guid, SubscriberBase> _subscribersMatchName = new ConcurrentDictionary<Guid, SubscriberBase>();

        private readonly ConcurrentDictionary<Guid, SubscriberBase> _subscribersMatchOther = new ConcurrentDictionary<Guid, SubscriberBase>();

        private Guid AddSubscriberToPool(SubscriberBase subscriber)
        {
            var key = Guid.NewGuid();
            Add();
            return key;

            void Add()
            {
                var messageNameMatcherType = subscriber.MessageNameMatcher.GetType();
                if (messageNameMatcherType == typeof(MessageNameMatchingAll))
                {
                    _subscribersMatchAll.TryAdd(key, subscriber);
                    return;
                }
                else if (messageNameMatcherType == typeof(MessageNameMatchingWithStringComparison))
                {
                    _subscribersMatchName.TryAdd(key, subscriber);
                    var matcher = subscriber.MessageNameMatcher as MessageNameMatchingWithStringComparison;
                    if (matcher!.StringComparison == StringComparison.Ordinal)
                    {
                        var hashSet = _subscribersNames.GetOrAdd(matcher.SubscriberMessageName,
                            _ => new HashSet<Guid>());
                        lock (hashSet)
                        {
                            hashSet.Add(key);
                            return;
                        }
                    }
                    else if (matcher.StringComparison == StringComparison.OrdinalIgnoreCase)
                    {
                        var hashSet = _subscribersNamesIgnoreCase.GetOrAdd(matcher.SubscriberMessageName,
                            _ => new HashSet<Guid>());
                        lock (hashSet)
                        {
                            hashSet.Add(key);
                            return;
                        }
                    }
                }
                else
                {
                    _subscribersMatchOther.TryAdd(key, subscriber);
                    return;
                }
            }
        }

        private bool TryRemoveSubscriberFromPool(Guid key, out SubscriberBase subscriber)
        {
            if (_subscribersMatchAll.TryRemove(key, out subscriber))
            {
                return true;
            }
            else if (_subscribersMatchName.TryRemove(key, out subscriber))
            {
                var matcher = subscriber.MessageNameMatcher as MessageNameMatchingWithStringComparison;
                if (matcher!.StringComparison == StringComparison.Ordinal)
                {
                    if (_subscribersNames.TryGetValue(matcher.SubscriberMessageName, out var hashSet))
                    {
                        lock (hashSet)
                        {
                            hashSet.Remove(key);
                        }
                    }
                    else if (_subscribersNamesIgnoreCase.TryGetValue(matcher.SubscriberMessageName, out hashSet))
                    {
                        lock (hashSet)
                        {
                            hashSet.Remove(key);
                        }
                    }
                }
                return true;
            }
            else
            {
                return _subscribersMatchOther.TryRemove(key, out subscriber);
            }
        }

        private IEnumerable<KeyValuePair<Guid, SubscriberBase>> GetSubscribersFromPool(string messageName)
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
                        yield return new KeyValuePair<Guid, SubscriberBase>(id, subscriber);
                    }
                }
            }

            if (_subscribersNamesIgnoreCase.TryGetValue(messageName, out hashSet))
            {
                foreach (var id in hashSet)
                {
                    if (_subscribersMatchName.TryGetValue(id, out var subscriber))
                    {
                        yield return new KeyValuePair<Guid, SubscriberBase>(id, subscriber);
                    }
                }
            }

            foreach (var item in _subscribersMatchOther)
            {
                if (item.Value.MessageNameMatcher.IsComplied(messageName))
                    yield return item;
            }
        }
    }
}
