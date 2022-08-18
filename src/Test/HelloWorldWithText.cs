namespace Test
{
    [TestClass]
    public class HelloWorldWithText
    {
        [TestMethod]
        public void TestMethod()
        {
            using var bus = new MessageBus();
            var subscriberTicket = bus.RegisterSubscriber<string, int>("Hello", SubscriberMethod);
            var text = "Hello World";
            var result = 100;

            var publisherTicket = bus.RegisterPublisher<string, int>("Hello"); 
            var returnValue = publisherTicket.Executor.Execute(text);

            bus.UnregisterPublisher(publisherTicket);
            bus.UnregisterSubscriber(subscriberTicket);

            Assert.AreEqual(text, _received);
            Assert.AreEqual(result, returnValue);
        }

        private string? _received;
        public int SubscriberMethod(string? argument)
        {
            _received = argument;
            return 100;
        }
    }
}