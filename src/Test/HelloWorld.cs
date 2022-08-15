namespace Test
{
    [TestClass]
    public class HelloWorld
    {
        [TestMethod]
        public void TestMethod()
        {
            using var bus = new MessageBus();
            var subscriber = bus.RegisterSubscriber<string>("Hello", SubscriberMethod);
            var text = "Hello World";

            var publisher = bus.RegisterPublisher<string>("Hello");
            publisher.Executor.Execute(text);

            bus.UnregisterPublisher(publisher);
            bus.UnregisterSubscriber(subscriber);

            Assert.AreEqual(text, _received);
        }

        private string? _received;
        public void SubscriberMethod(string? argument)
        {
            _received = argument;
        }
    }
}