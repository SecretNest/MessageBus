Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports SecretNest.MessageBus
Imports SecretNest.MessageBus.MessageNameMatching
Imports SecretNest.MessageBus.Options

<TestClass>
Public Class NameMatching
    <TestMethod>
    Sub TestMethod()
        Using bus As New MessageBus
            Dim subscriberExact1Ticket = bus.RegisterSubscriber(Of String)("Hello", AddressOf subscriberExact1)
            Dim subscriberExact2Ticket = bus.RegisterSubscriber(Of String)("hello", AddressOf SubscriberExact2)
            Dim subscriberIgnoreCaseTicket = bus.RegisterSubscriber(Of String)(New MessageNameMatchingWithStringComparison("HELLO", StringComparison.OrdinalIgnoreCase), AddressOf SubscriberIgnoreCase)
            Dim subscriberAllTicket = bus.RegisterSubscriber(Of String)(New MessageNameMatchingAll, AddressOf SubscriberAll)
            ' ReSharper disable once StringLiteralTypo
            Dim subscriberRegEx1Ticket = bus.RegisterSubscriber(Of String)(New MessageNameMatchingWithRegularExpression("[Hh]ello"), AddressOf SubscriberRegEx1)
            Dim subscriberRegEx2Ticket = bus.RegisterSubscriber(Of String)(New MessageNameMatchingWithRegularExpression("hello"), AddressOf SubscriberRegEx2)

            Dim text = "Hello World"

            Dim publisherTicket = bus.RegisterPublisher(Of String)("Hello", New MessageBusPublisherOptions(Of String)(True))
            publisherTicket.Executor.Execute(text)

            bus.UnregisterPublisher(publisherTicket)
            bus.UnregisterSubscriber(subscriberExact1Ticket)
            bus.UnregisterSubscriber(subscriberExact2Ticket)
            bus.UnregisterSubscriber(subscriberIgnoreCaseTicket)
            bus.UnregisterSubscriber(subscriberAllTicket)
            bus.UnregisterSubscriber(subscriberRegEx1Ticket)
            bus.UnregisterSubscriber(subscriberRegEx2Ticket)

            Assert.AreEqual(text, _exact1)
            Assert.AreNotEqual(text, _exact2)
            Assert.AreEqual(text, _ignoreCase)
            Assert.AreEqual(text, _all)
            Assert.AreEqual(text, _regex1)
            Assert.AreNotEqual(text, _regex2)
        End Using
    End Sub

    Private _exact1, _exact2, _ignoreCase, _all, _regex1, _regex2 As String

    Public Sub SubscriberExact1(argument As String)
        _exact1 = argument
    End Sub

    Public Sub SubscriberExact2(argument As String)
        _exact2 = argument
    End Sub

    Public Sub SubscriberIgnoreCase(argument As String)
        _ignoreCase = argument
    End Sub

    Public Sub SubscriberAll(argument As String)
        _all = argument
    End Sub

    Public Sub SubscriberRegEx1(argument As String)
        _regex1 = argument
    End Sub

    Public Sub SubscriberRegEx2(argument As String)
        _regex2 = argument
    End Sub
End Class
