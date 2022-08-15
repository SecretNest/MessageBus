using SecretNest.MessageBus.Options;
using System;

namespace Test
{
    [TestClass]
    public class DynamicAdding
    {
        [TestMethod]
        public void TestMethod()
        {
            using var bus = new MessageBus();
            var subscriber1000 = bus.RegisterSubscriber<int, int>("DynamicAdding", Subscriber1000,
                new MessageBusSubscriberOptions<int, int>(sequence: 0, resultCheckingCallback: i => i > 0));
            var subscriber100 = bus.RegisterSubscriber<int, int>("DynamicAdding", Subscriber10,
                new MessageBusSubscriberOptions<int, int>(sequence: 2, resultCheckingCallback: i => i > 0));
            
            var publisher = bus.RegisterPublisher<int, int>("DynamicAdding", new MessageBusPublisherOptions<int, int>(defaultReturnValue: 5));

            var result1000 = publisher.Executor.Execute(1500);
            var result10 = publisher.Executor.Execute(300);
            var result5 = publisher.Executor.Execute(8);

            //adding
            var subscriber10 = bus.RegisterSubscriber<int, int>("DynamicAdding", Subscriber100,
                new MessageBusSubscriberOptions<int, int>(sequence: 1, resultCheckingCallback: i => i > 0));
            var result100 = publisher.Executor.Execute(300);
            var result5B = publisher.Executor.Execute(8);

            //removing
            bus.UnregisterSubscriber(subscriber1000);
            var result100B = publisher.Executor.Execute(1500);

            bus.UnregisterPublisher(publisher);
            bus.UnregisterSubscriber(subscriber100);
            bus.UnregisterSubscriber(subscriber10);

            Assert.AreEqual(1000, result1000);
            Assert.AreEqual(10, result10);
            Assert.AreEqual(5, result5);
            Assert.AreEqual(100, result100);
            Assert.AreEqual(5, result5B);
            Assert.AreEqual(100, result100B);
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

        public int Subscriber10(int argument)
        {
            if (argument > 10)
                return 10;
            else
                return 0;
        }
    }
}
