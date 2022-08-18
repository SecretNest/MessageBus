namespace Test
{
    [TestClass]
    public class HelloWorld
    {
        [TestMethod]
        public void TestMethod()
        {
            using var bus = new MessageBus();
            var subscriberTicket = bus.RegisterSubscriber<string>("Hello", SubscriberMethod);
            var text = "Hello World";

            var publisherTicket = bus.RegisterPublisher<string>("Hello");
            publisherTicket.Executor.Execute(text);

            bus.UnregisterPublisher(publisherTicket);
            bus.UnregisterSubscriber(subscriberTicket);

            Assert.AreEqual(text, _received);
        }

        private string? _received;
        public void SubscriberMethod(string? argument)
        {
            _received = argument;
        }
    }
}