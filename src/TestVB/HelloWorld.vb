Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SecretNest.MessageBus

<TestClass>
Public Class HelloWorld
    <TestMethod>
    Sub TestMethod()
        Using bus As New MessageBus
            Dim subscriberTicket = bus.RegisterSubscriber(Of String)("Hello", AddressOf SubscriberMethod)
            Dim text = "Hello World"

            Dim publisherTicket = bus.RegisterPublisher(Of String)("Hello")
            publisherTicket.Executor.Execute(text)

            bus.UnregisterPublisher(publisherTicket)
            bus.UnregisterSubscriber(subscriberTicket)

            Assert.AreEqual(text, _received)
        End Using
    End Sub

    Private _received As String
    Public Sub SubscriberMethod(argument As String)
        _received = argument
    End Sub
End Class
