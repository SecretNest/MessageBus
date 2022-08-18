Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SecretNest.MessageBus
Imports SecretNest.MessageBus.Options

<TestClass>
Public Class DynamicAdding
    <TestMethod>
    Sub TestMethod()
        Using bus As New MessageBus
            Dim subscriber1000Ticket = bus.RegisterSubscriber(Of Integer, Integer)("DynamicAdding", AddressOf Subscriber1000, New MessageBusSubscriberOptions(Of Integer, Integer)(sequence:=0, resultCheckingCallback:=Function(i) i > 0))
            Dim subscriber10Ticket = bus.RegisterSubscriber(Of Integer, Integer)("DynamicAdding", AddressOf Subscriber10, New MessageBusSubscriberOptions(Of Integer, Integer)(sequence:=2, resultCheckingCallback:=Function(i) i > 0))

            Dim publisherTicket = bus.RegisterPublisher(Of Integer, Integer)("DynamicAdding", New MessageBusPublisherOptions(Of Integer, Integer)(defaultReturnValue:=5))
            Dim result1000 = publisherTicket.Executor.Execute(1500)
            Dim result10 = publisherTicket.Executor.Execute(300)
            Dim result5 = publisherTicket.Executor.Execute(8)

            'adding
            Dim subscriber100Ticket = bus.RegisterSubscriber(Of Integer, Integer)("DynamicAdding", AddressOf Subscriber100, New MessageBusSubscriberOptions(Of Integer, Integer)(sequence:=1, resultCheckingCallback:=Function(i) i > 0))
            Dim result100 = publisherTicket.Executor.Execute(300)
            Dim result5B = publisherTicket.Executor.Execute(8)

            'removing
            bus.UnregisterSubscriber(subscriber1000Ticket)
            Dim result100B = publisherTicket.Executor.Execute(1500)

            bus.UnregisterPublisher(publisherTicket)
            bus.UnregisterSubscriber(subscriber100Ticket)
            bus.UnregisterSubscriber(subscriber10Ticket)

            Assert.AreEqual(1000, result1000)
            Assert.AreEqual(10, result10)
            Assert.AreEqual(5, result5)
            Assert.AreEqual(100, result100)
            Assert.AreEqual(5, result5B)
            Assert.AreEqual(100, result100B)
        End Using
    End Sub

    Public Shared Function Subscriber1000(argument As Integer) As Integer
        If argument > 1000 Then
            Return 1000
        Else
            Return 0
        End If
    End Function

    Public Shared Function Subscriber100(argument As Integer) As Integer
        If argument > 100 Then
            Return 100
        Else
            Return 0
        End If
    End Function

    Public Shared Function Subscriber10(argument As Integer) As Integer
        If argument > 10 Then
            Return 10
        Else
            Return 0
        End If
    End Function
End Class
