using SecretNest.MessageBus.Options;
using System;

namespace Test
{
    [TestClass]
    public class ConditionMatching
    {
        [TestMethod]
        public void TestMethod()
        {
            using var bus = new MessageBus();
            var subscriber1000Ticket = bus.RegisterSubscriber<int, int>("ConditionMatching", Subscriber1000,
                new MessageBusSubscriberOptions<int, int>(sequence: 0, conditionCheckingCallback: i => i is int and > 1000));
            var subscriber100Ticket = bus.RegisterSubscriber<int, int>("ConditionMatching", Subscriber100,
                new MessageBusSubscriberOptions<int, int>(sequence: 1, conditionCheckingCallback: i => i is int and > 100));

            var publisherTicket = bus.RegisterPublisher<int, int>("ConditionMatching", new MessageBusPublisherOptions<int, int>(defaultReturnValue: 5));

            var result1000 = publisherTicket.Executor.Execute(1500);
            var result100 = publisherTicket.Executor.Execute(300);
            var result5 = publisherTicket.Executor.Execute(80);

            bus.UnregisterPublisher(publisherTicket);
            bus.UnregisterSubscriber(subscriber1000Ticket);
            bus.UnregisterSubscriber(subscriber100Ticket);

            Assert.AreEqual(1000, result1000);
            Assert.AreEqual(100, result100);
            Assert.AreEqual(5, result5);
        }

        public int Subscriber1000(int argument)
        {
            return 1000;
        }

        public int Subscriber100(int argument)
        {
            return 100;
        }
    }
}
