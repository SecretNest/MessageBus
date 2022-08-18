Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SecretNest.MessageBus

<TestClass>
Public Class HelloWorldWithText
    <TestMethod>
    Sub TestMethod()
        Using bus As New MessageBus
            Dim subscriberTicket = bus.RegisterSubscriber(Of String, Integer)("Hello", AddressOf SubscriberMethod)
            Dim text = "Hello World"
            Dim result = 100

            Dim publisherTicket = bus.RegisterPublisher(Of String, Integer)("Hello")
            Dim returnValue = publisherTicket.Executor.Execute(text)

            bus.UnregisterPublisher(publisherTicket)
            bus.UnregisterSubscriber(subscriberTicket)

            Assert.AreEqual(text, _received)
            Assert.AreEqual(result, returnValue)
        End Using
    End Sub

    Private _received As String
    Public Function SubscriberMethod(argument As String) As Integer
        _received = argument
        Return 100
    End Function
End Class
