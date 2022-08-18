using SecretNest.MessageBus.Options;

namespace Test
{
    [TestClass]
    public class AsyncCall
    {
        [TestMethod]
        public async Task TestMethodAsync()
        {
            using var bus = new MessageBus();
            var subscriber1000Ticket = bus.RegisterSubscriber<int, int>("ArgumentMatching", Subscriber1000,
                new MessageBusSubscriberOptions<int, int>(sequence: 0, resultCheckingCallback: i => i > 0));
            var subscriber100Ticket = bus.RegisterSubscriber<int, int>("ArgumentMatching", Subscriber100Async,
                new MessageBusSubscriberOptions<int, int>(sequence: 1, resultCheckingCallback: i => i > 0));
            
            var publisherTicket = bus.RegisterPublisher<int, int>("ArgumentMatching", new MessageBusPublisherOptions<int, int>(defaultReturnValue: 5));

            var result1000 = publisherTicket.Executor.Execute(1500);
            var result100 = publisherTicket.Executor.Execute(300);
            var result5 = publisherTicket.Executor.Execute(80);
            var result1000Async = await publisherTicket.Executor.ExecuteAsync(1500);

            using var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();
            Exception e100Async = null!;
            try
            {
                var result100Async = await publisherTicket.Executor.ExecuteAsync(300, cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                e100Async = e;
            }

            var result5BAsync = await publisherTicket.Executor.ExecuteAsync(80);

            bus.UnregisterPublisher(publisherTicket);
            bus.UnregisterSubscriber(subscriber1000Ticket);
            bus.UnregisterSubscriber(subscriber100Ticket);

            Assert.AreEqual(1000, result1000);
            Assert.AreEqual(100, result100);
            Assert.AreEqual(5, result5);
            Assert.AreEqual(1000, result1000Async);
            Assert.IsInstanceOfType(e100Async, typeof(OperationCanceledException));
            Assert.AreEqual(5, result5BAsync);
        }

        public int Subscriber1000(int argument)
        {
            if (argument > 1000)
                return 1000;
            else
                return 0;
        }

        public async Task<int> Subscriber100Async(int argument, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
            if (argument > 100)
                return 100;
            else
                return 0;
        }
    }
}
