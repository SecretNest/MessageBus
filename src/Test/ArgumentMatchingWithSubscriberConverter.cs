using SecretNest.MessageBus.Options;
using System;

namespace Test
{
    [TestClass]
    public class ArgumentMatchingWithSubscriberConverter
    {
        [TestMethod]
        public void TestMethod()
        {
            using var bus = new MessageBus();
            var subscriber1000Ticket = bus.RegisterSubscriber<string, string>("ArgumentMatching", Subscriber1000,
                new MessageBusSubscriberOptions<string, string>(
                    sequence: 0,
                    resultCheckingCallback: i => i != null,
                    argumentConvertingCallback: i => i?.ToString(),
                    returnValueConvertingCallback: s => int.Parse(s!)));
            var subscriber100Ticket = bus.RegisterSubscriber<int, int>("ArgumentMatching", Subscriber100,
                new MessageBusSubscriberOptions<int, int>(
                    sequence: 1,
                    resultCheckingCallback: i => i > 0));
            
            var publisherTicket = bus.RegisterPublisher<int, int>("ArgumentMatching", new MessageBusPublisherOptions<int, int>(defaultReturnValue: 5));

            var result1000 = publisherTicket.Executor.Execute(1000);
            var result100 = publisherTicket.Executor.Execute(300);
            var result5 = publisherTicket.Executor.Execute(80);

            bus.UnregisterPublisher(publisherTicket);
            bus.UnregisterSubscriber(subscriber1000Ticket);
            bus.UnregisterSubscriber(subscriber100Ticket);

            Assert.AreEqual(1000, result1000);
            Assert.AreEqual(100, result100);
            Assert.AreEqual(5, result5);
        }

        public string? Subscriber1000(string? argument)
        {
            if (argument == "1000")
                return "1000";
            else
                return null;
        }

        public int Subscriber100(int argument)
        {
            if (argument > 100)
                return 100;
            else
                return 0;
        }
    }
}
