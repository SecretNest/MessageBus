Imports System.Threading
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SecretNest.MessageBus
Imports SecretNest.MessageBus.Options

<TestClass>
Public Class AsyncCall
    <TestMethod>
    Async Function TestMethod() As Task
        Using bus As New MessageBus
            Dim subscriber1000Ticket = bus.RegisterSubscriber(Of Integer, Integer)("ArgumentMatching", AddressOf Subscriber1000, New MessageBusSubscriberOptions(Of Integer, Integer)(sequence:=0, resultCheckingCallback:=Function(i) i > 0))
            Dim subscriber100Ticket = bus.RegisterSubscriber(Of Integer, Integer)("ArgumentMatching", AddressOf Subscriber100Async, New MessageBusSubscriberOptions(Of Integer, Integer)(sequence:=1, resultCheckingCallback:=Function(i) i > 0))

            Dim publisherTicket = bus.RegisterPublisher(Of Integer, Integer)("ArgumentMatching", New MessageBusPublisherOptions(Of Integer, Integer)(defaultReturnValue:=5))

            Dim result1000 = publisherTicket.Executor.Execute(1500)
            Dim result100 = publisherTicket.Executor.Execute(300)
            Dim result5 = publisherTicket.Executor.Execute(80)
            Dim result1000Async = Await publisherTicket.Executor.ExecuteAsync(1500)

            Dim e100Async As Exception = Nothing
            Using cancellationTokenSource = New CancellationTokenSource()
                cancellationTokenSource.Cancel()

                Try
                    Dim result100Async = Await publisherTicket.Executor.ExecuteAsync(300, cancellationTokenSource.Token)
                Catch ex As Exception
                    e100Async = ex
                End Try
            End Using
            Dim result5BAsync = Await publisherTicket.Executor.ExecuteAsync(80)

            bus.UnregisterPublisher(publisherTicket)
            bus.UnregisterSubscriber(subscriber1000Ticket)
            bus.UnregisterSubscriber(subscriber100Ticket)

            Assert.AreEqual(1000, result1000)
            Assert.AreEqual(100, result100)
            Assert.AreEqual(5, result5)
            Assert.AreEqual(1000, result1000Async)
            Assert.IsInstanceOfType(e100Async, GetType(OperationCanceledException))
            Assert.AreEqual(5, result5BAsync)

        End Using
    End Function

    Public Function Subscriber1000(argument As Integer) As Integer
        If argument > 1000 Then
            Return 1000
        Else
            Return 0
        End If
    End Function

    Public Async Function Subscriber100Async(argument As Integer, cancellationToken As CancellationToken) As Task(Of Integer)
        Await Task.Delay(100, cancellationToken)
        If argument > 100 Then
            Return 100
        Else
            Return 0
        End If
    End Function
End Class
