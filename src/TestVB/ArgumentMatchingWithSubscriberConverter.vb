Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SecretNest.MessageBus
Imports SecretNest.MessageBus.Options

<TestClass>
Public Class ArgumentMatchingWithSubscriberConverter
    <TestMethod>
    Public Sub TestMethod()
        Using bus As New MessageBus
            Dim subscriber1000Ticket = bus.RegisterSubscriber(Of String, String)("ArgumentMatching", AddressOf Subscriber1000, New MessageBusSubscriberOptions(Of String, String)(sequence:=0, resultCheckingCallback:=Function(i) i IsNot Nothing, argumentConvertingCallback:=Function(i) i?.ToString(), returnValueConvertingCallback:=Function(s) Integer.Parse(s)))
            Dim subscriber100Ticket = bus.RegisterSubscriber(Of Integer, Integer)("ArgumentMatching", AddressOf Subscriber100, New MessageBusSubscriberOptions(Of Integer, Integer)(sequence:=1, resultCheckingCallback:=Function(i) i > 0))

            Dim publisherTicket = bus.RegisterPublisher(Of Integer, Integer)("ArgumentMatching", New MessageBusPublisherOptions(Of Integer, Integer)(defaultReturnValue:=5))

            Dim result1000 = publisherTicket.Executor.Execute(1000)
            Dim result100 = publisherTicket.Executor.Execute(300)
            Dim result5 = publisherTicket.Executor.Execute(80)

            bus.UnregisterPublisher(publisherTicket)
            bus.UnregisterSubscriber(subscriber1000Ticket)
            bus.UnregisterSubscriber(subscriber100Ticket)

            Assert.AreEqual(1000, result1000)
            Assert.AreEqual(100, result100)
            Assert.AreEqual(5, result5)
        End Using
    End Sub

    Public Shared Function Subscriber1000(argument As String) As String
        If argument = "1000" Then
            Return "1000"
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function Subscriber100(argument As Integer) As Integer
        If argument > 100 Then
            Return 100
        Else
            Return 0
        End If
    End Function
End Class
