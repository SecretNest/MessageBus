Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SecretNest.MessageBus
Imports SecretNest.MessageBus.Options

<TestClass>
Public Class SubscriberForceExecuting
    <TestMethod>
    Sub TestMethod()
        Using bus As New MessageBus
            Dim subscriber1000Ticket = bus.RegisterSubscriber(Of Integer, Integer)("ArgumentMatching", AddressOf Subscriber1000, New MessageBusSubscriberOptions(Of Integer, Integer)(sequence:=0, resultCheckingCallback:=Function(i) i > 0))
            Dim subscriber100Ticket = bus.RegisterSubscriber(Of Integer, Integer)("ArgumentMatching", AddressOf Subscriber100, New MessageBusSubscriberOptions(Of Integer, Integer)(sequence:=2, resultCheckingCallback:=Function(i) i > 0))
            Dim subscriberForceTicket = bus.RegisterSubscriber(Of Integer)("ArgumentMatching", AddressOf SubscriberForce, New MessageBusSubscriberOptions(Of Integer)(sequence:=1, isAlwaysExecution:=True, isFinal:=False))

            Dim publisherTicket = bus.RegisterPublisher(Of Integer, Integer)("ArgumentMatching", New MessageBusPublisherOptions(Of Integer, Integer)(defaultReturnValue:=5))

            _forceExecuted = False
            Dim result1000 = publisherTicket.Executor.Execute(1500)
            Assert.IsTrue(_forceExecuted)
            _forceExecuted = False
            Dim result100 = publisherTicket.Executor.Execute(300)
            Assert.IsTrue(_forceExecuted)
            _forceExecuted = False
            Dim result5 = publisherTicket.Executor.Execute(80)
            Assert.IsTrue(_forceExecuted)

            bus.UnregisterPublisher(publisherTicket)
            bus.UnregisterSubscriber(subscriber1000Ticket)
            bus.UnregisterSubscriber(subscriber100Ticket)
            bus.UnregisterSubscriber(subscriberForceTicket)

            Assert.AreEqual(1000, result1000)
            Assert.AreEqual(100, result100)
            Assert.AreEqual(5, result5)
        End Using
    End Sub

    Public Function Subscriber1000(argument As Integer) As Integer
        If argument > 1000 Then
            Return 1000
        Else
            Return 0
        End If
    End Function

    Public Function Subscriber100(argument As Integer) As Integer
        If argument > 100 Then
            Return 100
        Else
            Return 0
        End If
    End Function

    Private _forceExecuted As Boolean

    Public Sub SubscriberForce(argument As Integer)
        _forceExecuted = True
    End Sub
End Class
