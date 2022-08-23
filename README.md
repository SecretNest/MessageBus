# MessageBus

Message Bus is a component provided from SecretNest.info, providing publishing subscription model support within one app.

## Doc

https://messagebus.docs.secretnest.info/

## Standards

netstandard 1.3

## Nuget Packages

[SecretNest.MessageBus](https://www.nuget.org/packages/SecretNest.MessageBus): Provides publishing subscription model support within one app. Imports this package when composing an app managing the instance of Message Bus. This package references SecretNest.MessageBus.Abstractions.

[SecretNest.MessageBus.Abstractions](https://www.nuget.org/packages/SecretNest.MessageBus.Abstractions): Abstractions for Message Bus from SecretNest.info. Imports this package when composing a component intended to be used to connect to Message Bus but do not hold the instance of the Message Bus itself. When the project contains the instance of Message Bus itself, imports SecretNest.MessageBus directly.
