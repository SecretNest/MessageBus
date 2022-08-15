using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace Test
{
    [TestClass]
    public class DefaultReturn
    {
        [TestMethod]
        public void TestMethod()
        {
            using var bus = new MessageBus();

            var subscriberHaveReturn = bus.RegisterSubscriber<int, int>("Return", HaveReturn,
                new MessageBusSubscriberOptions<int, int>(sequence: 0));
            var subscriberNoReturnNoFinal = bus.RegisterSubscriber<int>(new MessageNameMatchingAll(), NoReturn,
                new MessageBusSubscriberOptions<int>(sequence: 1, isFinal: false));
            var subscriberNoReturn = bus.RegisterSubscriber<int>("NoReturnButFinal", NoReturn,
                new MessageBusSubscriberOptions<int>(sequence: 2));

            var publisher1 = bus.RegisterPublisher<int, int>("Return", new MessageBusPublisherOptions<int, int>(defaultReturnValue: 100));
            var publisher2 = bus.RegisterPublisher<int, int>("SomethingElse", new MessageBusPublisherOptions<int, int>(defaultReturnValue: 100));
            var publisher3 = bus.RegisterPublisher<int, int>("NoReturnButFinal", new MessageBusPublisherOptions<int, int>(defaultReturnValue: 100));

            _haveReturnExecuted = false;
            _noReturnExecuted = false;
            var result11 = publisher1.Executor.Execute(10);
            Assert.IsTrue(_haveReturnExecuted);
            Assert.IsFalse(_noReturnExecuted);
        
            _haveReturnExecuted = false;
            _noReturnExecuted = false;
            var result100 = publisher2.Executor.ExecuteAndGetMessageInstance(0, out var messageInstance1);
            Assert.IsFalse(_haveReturnExecuted);
            Assert.IsTrue(_noReturnExecuted);

            _haveReturnExecuted = false;
            _noReturnExecuted = false;
            var resultDefault = publisher3.Executor.ExecuteAndGetMessageInstance(0, out var messageInstance2);
            Assert.IsFalse(_haveReturnExecuted);
            Assert.IsTrue(_noReturnExecuted);

            bus.UnregisterSubscriber(subscriberHaveReturn);
            bus.UnregisterSubscriber(subscriberNoReturnNoFinal);
            bus.UnregisterSubscriber(subscriberNoReturn);
            bus.UnregisterPublisher(publisher1);
            bus.UnregisterPublisher(publisher2);
            bus.UnregisterPublisher(publisher3);

            Assert.AreEqual(11, result11);
            Assert.AreEqual(100, result100);
            Assert.AreEqual(0, resultDefault);

            Assert.IsFalse(messageInstance1.IsSubscriberReturnValueAccepted);
            Assert.IsFalse(messageInstance1 is MessageInstanceWithVoidReturnValue);

            Assert.IsTrue(messageInstance2.IsSubscriberReturnValueAccepted);
            Assert.IsTrue(messageInstance2 is MessageInstanceWithVoidReturnValue);
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
