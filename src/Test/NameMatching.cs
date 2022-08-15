using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace Test
{
    [TestClass]
    public class NameMatching
    {
        [TestMethod]
        public void TestMethod()
        {
            using var bus = new MessageBus();
            var subscriberExact1 = bus.RegisterSubscriber<string>("Hello", SubscriberExact1);
            var subscriberExact2 = bus.RegisterSubscriber<string>("hello", SubscriberExact2);
            var subscriberIgnoreCase = bus.RegisterSubscriber<string>(new MessageNameMatchingWithStringComparison("HELLO", StringComparison.OrdinalIgnoreCase), SubscriberIgnoreCase);
            var subscriberAll = bus.RegisterSubscriber<string>(new MessageNameMatchingAll(), SubscriberAll);
            // ReSharper disable once StringLiteralTypo
            var subscriberRegEx1 = bus.RegisterSubscriber<string>(new MessageNameMatchingWithRegularExpression("[Hh]ello"), SubscriberRegEx1);
            var subscriberRegEx2 = bus.RegisterSubscriber<string>(new MessageNameMatchingWithRegularExpression("hello"), SubscriberRegEx2);

            var text = "Hello World";

            var publisher = bus.RegisterPublisher<string>("Hello", new MessageBusPublisherOptions<string>(true));
            publisher.Executor.Execute(text);

            bus.UnregisterPublisher(publisher);
            bus.UnregisterSubscriber(subscriberExact1);
            bus.UnregisterSubscriber(subscriberExact2);
            bus.UnregisterSubscriber(subscriberIgnoreCase);
            bus.UnregisterSubscriber(subscriberAll);
            bus.UnregisterSubscriber(subscriberRegEx1);
            bus.UnregisterSubscriber(subscriberRegEx2);

            Assert.AreEqual(text, _exact1);
            Assert.AreNotEqual(text, _exact2);
            Assert.AreEqual(text, _ignoreCase);
            Assert.AreEqual(text, _all);
            Assert.AreEqual(text, _regex1);
            Assert.AreNotEqual(text, _regex2);
        }

        private string? _exact1, _exact2, _ignoreCase, _all, _regex1 , _regex2;

        public void SubscriberExact1(string? argument)
        {
            _exact1 = argument;
        }

        public void SubscriberExact2(string? argument)
        {
            _exact2 = argument;
        }

        public void SubscriberIgnoreCase(string? argument)
        {
            _ignoreCase = argument;
        }

        public void SubscriberAll(string? argument)
        {
            _all = argument;
        }

        public void SubscriberRegEx1(string? argument)
        {
            _regex1 = argument;
        }

        public void SubscriberRegEx2(string? argument)
        {
            _regex2 = argument;
        }
    }
}
