Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SecretNest.MessageBus
Imports SecretNest.MessageBus.MessageNameMatching
Imports SecretNest.MessageBus.Options

<TestClass>
Public Class DefaultReturn
    <TestMethod>
    Sub TestMethod()
        Using bus As New MessageBus
            Dim subscriberHaveReturnTicket = bus.RegisterSubscriber(Of Integer, Integer)("Return", AddressOf HaveReturn, New MessageBusSubscriberOptions(Of Integer, Integer)(sequence:=0))
            Dim subscriberNoReturnNoFinalTicket = bus.RegisterSubscriber(Of Integer)(New MessageNameMatchingAll(), AddressOf NoReturn, New MessageBusSubscriberOptions(Of Integer)(sequence:=1, isFinal:=False))
            Dim subscriberNoReturnTicket = bus.RegisterSubscriber(Of Integer)("NoReturnButFinal", AddressOf NoReturn, New MessageBusSubscriberOptions(Of Integer)(sequence:=2))

            Dim publisher1Ticket = bus.RegisterPublisher(Of Integer, Integer)("Return", New MessageBusPublisherOptions(Of Integer, Integer)(defaultReturnValue:=100))
            Dim publisher2Ticket = bus.RegisterPublisher(Of Integer, Integer)("SomethingElse", New MessageBusPublisherOptions(Of Integer, Integer)(defaultReturnValue:=100))
            Dim publisher3Ticket = bus.RegisterPublisher(Of Integer, Integer)("NoReturnButFinal", New MessageBusPublisherOptions(Of Integer, Integer)(defaultReturnValue:=100))

            _haveReturnExecuted = False
            _noReturnExecuted = False
            Dim result11 = publisher1Ticket.Executor.Execute(10)
            Assert.IsTrue(_haveReturnExecuted)
            Assert.IsFalse(_noReturnExecuted)

            _haveReturnExecuted = False
            _noReturnExecuted = False
            Dim messageInstance1 As MessageInstance = Nothing
            Dim result100 = publisher2Ticket.Executor.ExecuteAndGetMessageInstance(0, messageInstance1)
            Assert.IsFalse(_haveReturnExecuted)
            Assert.IsTrue(_noReturnExecuted)

            _haveReturnExecuted = False
            _noReturnExecuted = False
            Dim messageInstance2 As MessageInstance = Nothing
            Dim resultDefault = publisher3Ticket.Executor.ExecuteAndGetMessageInstance(0, messageInstance2)
            Assert.IsFalse(_haveReturnExecuted)
            Assert.IsTrue(_noReturnExecuted)

            bus.UnregisterSubscriber(subscriberHaveReturnTicket)
            bus.UnregisterSubscriber(subscriberNoReturnNoFinalTicket)
            bus.UnregisterSubscriber(subscriberNoReturnTicket)
            bus.UnregisterPublisher(publisher1Ticket)
            bus.UnregisterPublisher(publisher2Ticket)
            bus.UnregisterPublisher(publisher3Ticket)

            Assert.AreEqual(11, result11)
            Assert.AreEqual(100, result100)
            Assert.AreEqual(0, resultDefault)

            Assert.IsFalse(messageInstance1.IsSubscriberReturnValueAccepted)
            Assert.IsFalse(messageInstance1.GetType() Is GetType(MessageInstanceWithVoidReturnValue))

            Assert.IsTrue(messageInstance2.IsSubscriberReturnValueAccepted)
            Assert.IsTrue(messageInstance2.GetType() Is GetType(MessageInstanceWithVoidReturnValue))
        End Using
    End Sub

    Private _haveReturnExecuted, _noReturnExecuted As Boolean
    Public Function HaveReturn(something As Integer) As Integer
        _haveReturnExecuted = True
        Return something + 1
    End Function

    Public Sub NoReturn(something As Integer)
        _noReturnExecuted = True
    End Sub

End Class
