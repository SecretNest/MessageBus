using SecretNest.MessageBus.Options;
using System;

namespace Test
{
    [TestClass]
    public class ArgumentMatchingWithPublisherConverterExecuteOnce
    {
        [TestMethod]
        public void TestMethod()
        {
            using var bus = new MessageBus();
            var subscriber1000Ticket = bus.RegisterSubscriber<int, int>("ArgumentMatching", Subscriber1000,
                new MessageBusSubscriberOptions<int, int>(sequence: 0, resultCheckingCallback: i => i > 0));
            var subscriber100Ticket = bus.RegisterSubscriber<int, int>("ArgumentMatching", Subscriber100,
                new MessageBusSubscriberOptions<int, int>(sequence: 1, resultCheckingCallback: i => i > 0));

            var publisherTicket = bus.RegisterPublisher<string, string>("ArgumentMatching",
                new MessageBusPublisherOptions<string, string>(
                    defaultReturnValue: "5", 
                    argumentConvertingCallback: s => int.Parse(s!),
                    returnValueConvertingCallback: i => i?.ToString()));

            var result1000 = publisherTicket.Executor.Execute("1500");
            var result100 = publisherTicket.Executor.Execute("300");
            var result5 = publisherTicket.Executor.Execute("80");
            var result5B = bus.ExecuteOnceWithReturn("ArgumentMatching", "80", new MessageBusPublisherOptions<string, string>(
                defaultReturnValue: "5",
                argumentConvertingCallback: s => int.Parse(s!),
                returnValueConvertingCallback: i => i?.ToString()));

            bus.UnregisterPublisher(publisherTicket);
            bus.UnregisterSubscriber(subscriber1000Ticket);
            bus.UnregisterSubscriber(subscriber100Ticket);

            Assert.AreEqual("1000", result1000);
            Assert.AreEqual("100", result100);
            Assert.AreEqual("5", result5);
            Assert.AreEqual("5", result5B);
        }

        public int Subscriber1000(int argument)
        {
            if (argument > 1000)
                return 1000;
            else
                return 0;
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
