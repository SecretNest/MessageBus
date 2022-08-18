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
            var subscriberExact1Ticket = bus.RegisterSubscriber<string>("Hello", SubscriberExact1);
            var subscriberExact2Ticket = bus.RegisterSubscriber<string>("hello", SubscriberExact2);
            var subscriberIgnoreCaseTicket = bus.RegisterSubscriber<string>(new MessageNameMatchingWithStringComparison("HELLO", StringComparison.OrdinalIgnoreCase), SubscriberIgnoreCase);
            var subscriberAllTicket = bus.RegisterSubscriber<string>(new MessageNameMatchingAll(), SubscriberAll);
            // ReSharper disable once StringLiteralTypo
            var subscriberRegEx1Ticket = bus.RegisterSubscriber<string>(new MessageNameMatchingWithRegularExpression("[Hh]ello"), SubscriberRegEx1);
            var subscriberRegEx2Ticket = bus.RegisterSubscriber<string>(new MessageNameMatchingWithRegularExpression("hello"), SubscriberRegEx2);

            var text = "Hello World";

            var publisherTicket = bus.RegisterPublisher<string>("Hello", new MessageBusPublisherOptions<string>(true));
            publisherTicket.Executor.Execute(text);

            bus.UnregisterPublisher(publisherTicket);
            bus.UnregisterSubscriber(subscriberExact1Ticket);
            bus.UnregisterSubscriber(subscriberExact2Ticket);
            bus.UnregisterSubscriber(subscriberIgnoreCaseTicket);
            bus.UnregisterSubscriber(subscriberAllTicket);
            bus.UnregisterSubscriber(subscriberRegEx1Ticket);
            bus.UnregisterSubscriber(subscriberRegEx2Ticket);

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
