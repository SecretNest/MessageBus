﻿using SecretNest.MessageBus.Options;
using System;

namespace Test
{
    [TestClass]
    public class SubscriberForceExecuting
    {
        [TestMethod]
        public void TestMethod()
        {
            using var bus = new MessageBus();
            var subscriber1000Ticket = bus.RegisterSubscriber<int, int>("ArgumentMatching", Subscriber1000,
                new MessageBusSubscriberOptions<int, int>(sequence: 0, resultCheckingCallback: i => i > 0));
            var subscriber100Ticket = bus.RegisterSubscriber<int, int>("ArgumentMatching", Subscriber100,
                new MessageBusSubscriberOptions<int, int>(sequence: 2, resultCheckingCallback: i => i > 0));
            var subscriberForceTicket = bus.RegisterSubscriber<int>("ArgumentMatching", SubscriberForce,
                new MessageBusSubscriberOptions<int>(sequence: 1, isAlwaysExecution: true, isFinal: false));
       
            var publisherTicket = bus.RegisterPublisher<int, int>("ArgumentMatching", new MessageBusPublisherOptions<int, int>(defaultReturnValue: 5));

            _forceExecuted = false;
            var result1000 = publisherTicket.Executor.Execute(1500);
            Assert.IsTrue(_forceExecuted);
            _forceExecuted = false;
            var result100 = publisherTicket.Executor.Execute(300);
            Assert.IsTrue(_forceExecuted);
            _forceExecuted = false;
            var result5 = publisherTicket.Executor.Execute(80);
            Assert.IsTrue(_forceExecuted);

            bus.UnregisterPublisher(publisherTicket);
            bus.UnregisterSubscriber(subscriber1000Ticket);
            bus.UnregisterSubscriber(subscriber100Ticket);
            bus.UnregisterSubscriber(subscriberForceTicket);

            Assert.AreEqual(1000, result1000);
            Assert.AreEqual(100, result100);
            Assert.AreEqual(5, result5);
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

        private bool _forceExecuted;
        public void SubscriberForce(int argument)
        {
            _forceExecuted = true;
        }
    }
}
