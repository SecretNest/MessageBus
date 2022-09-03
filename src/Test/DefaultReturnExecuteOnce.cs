using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace Test
{
    [TestClass]
    public class DefaultReturnExecuteOnce
    {
        [TestMethod]
        public void TestMethod()
        {
            using var bus = new MessageBus();

            var subscriberHaveReturnTicket = bus.RegisterSubscriber<int, int>("Return", HaveReturn,
                new MessageBusSubscriberOptions<int, int>(sequence: 0));
            var subscriberNoReturnNoFinalTicket = bus.RegisterSubscriber<int>(new MessageNameMatchingAll(), NoReturn,
                new MessageBusSubscriberOptions<int>(sequence: 1, isFinal: false));
            var subscriberNoReturnTicket = bus.RegisterSubscriber<int>("NoReturnButFinal", NoReturn,
                new MessageBusSubscriberOptions<int>(sequence: 2));

            var publisher1Ticket = bus.RegisterPublisher<int, int>("Return", new MessageBusPublisherOptions<int, int>(defaultReturnValue: 100));
            var publisher2Ticket = bus.RegisterPublisher<int, int>("SomethingElse", new MessageBusPublisherOptions<int, int>(defaultReturnValue: 100));
            var publisher3Ticket = bus.RegisterPublisher<int, int>("NoReturnButFinal", new MessageBusPublisherOptions<int, int>(defaultReturnValue: 100));

            _haveReturnExecuted = false;
            _noReturnExecuted = false;
            var result11 = publisher1Ticket.Executor.Execute(10);
            Assert.IsTrue(_haveReturnExecuted);
            Assert.IsFalse(_noReturnExecuted);
        
            _haveReturnExecuted = false;
            _noReturnExecuted = false;
            var result100 = publisher2Ticket.Executor.ExecuteAndGetMessageInstance(0, out var messageInstance1);
            Assert.IsFalse(_haveReturnExecuted);
            Assert.IsTrue(_noReturnExecuted);

            _haveReturnExecuted = false;
            _noReturnExecuted = false;
            var result100B = bus.ExecuteOnceAndGetMessageInstanceWithReturn("SomethingElse", 0, out var messageInstance1B, new MessageBusPublisherOptions<int, int>(defaultReturnValue: 100));
            Assert.IsFalse(_haveReturnExecuted);
            Assert.IsTrue(_noReturnExecuted);

            _haveReturnExecuted = false;
            _noReturnExecuted = false;
            var resultDefault = publisher3Ticket.Executor.ExecuteAndGetMessageInstance(0, out var messageInstance2);
            Assert.IsFalse(_haveReturnExecuted);
            Assert.IsTrue(_noReturnExecuted);

            _haveReturnExecuted = false;
            _noReturnExecuted = false;
            var resultDefaultB = bus.ExecuteOnceAndGetMessageInstanceWithReturn("NoReturnButFinal", 0, out var messageInstance2B, new MessageBusPublisherOptions<int, int>(defaultReturnValue: 100));
            Assert.IsFalse(_haveReturnExecuted);
            Assert.IsTrue(_noReturnExecuted);

            bus.UnregisterSubscriber(subscriberHaveReturnTicket);
            bus.UnregisterSubscriber(subscriberNoReturnNoFinalTicket);
            bus.UnregisterSubscriber(subscriberNoReturnTicket);
            bus.UnregisterPublisher(publisher1Ticket);
            bus.UnregisterPublisher(publisher2Ticket);
            bus.UnregisterPublisher(publisher3Ticket);

            Assert.AreEqual(11, result11);
            Assert.AreEqual(100, result100);
            Assert.AreEqual(100, result100B);
            Assert.AreEqual(0, resultDefaultB);
            Assert.AreEqual(0, resultDefault);

            Assert.IsFalse(messageInstance1.IsSubscriberReturnValueAccepted);
            Assert.IsFalse(messageInstance1 is MessageInstanceWithVoidReturnValue);
            Assert.IsFalse(messageInstance1B.IsSubscriberReturnValueAccepted);
            Assert.IsFalse(messageInstance1B is MessageInstanceWithVoidReturnValue);

            Assert.IsTrue(messageInstance2.IsSubscriberReturnValueAccepted);
            Assert.IsTrue(messageInstance2 is MessageInstanceWithVoidReturnValue);
            Assert.IsTrue(messageInstance2B.IsSubscriberReturnValueAccepted);
            Assert.IsTrue(messageInstance2B is MessageInstanceWithVoidReturnValue);
        }

        private bool _haveReturnExecuted, _noReturnExecuted;
        public int HaveReturn(int something)
        {
            _haveReturnExecuted = true;
            return something + 1;
        }

        public void NoReturn(int something)
        {
            _noReturnExecuted = true;
        }
    }
}
