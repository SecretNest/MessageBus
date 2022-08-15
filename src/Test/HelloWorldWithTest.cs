namespace Test
{
    [TestClass]
    public class HelloWorldWithTest
    {
        [TestMethod]
        public void TestMethod()
        {
            using var bus = new MessageBus();
            var subscriber = bus.RegisterSubscriber<string, int>("Hello", SubscriberMethod);
            var text = "Hello World";
            var result = 100;

            var publisher = bus.RegisterPublisher<string, int>("Hello"); 
            var returnValue = publisher.Executor.Execute(text);

            bus.UnregisterPublisher(publisher);
            bus.UnregisterSubscriber(subscriber);

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