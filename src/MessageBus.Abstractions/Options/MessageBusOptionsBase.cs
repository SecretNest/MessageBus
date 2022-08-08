using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus.Options
{
    /// <summary>
    /// Stores options that configure the operation of MessageBus. This is the parent class of <see cref="MessageBusOptionsBase{TParameter, TReturn}"/> This is an abstract class.
    /// </summary>
    public abstract class MessageBusOptionsBase
    {

    }

    /// <summary>
    /// Stores options that configure the operation of MessageBus. This is an abstract class.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    public abstract class MessageBusOptionsBase<TParameter> : MessageBusOptionsBase
    {

    }

    /// <summary>
    /// Stores options that configure the operation of MessageBus. This is an abstract class.
    /// </summary>
    /// <typeparam name="TParameter">The type of the parameter.</typeparam>
    /// <typeparam name="TReturn">The type of the return value.</typeparam>
    public abstract class MessageBusOptionsBase<TParameter, TReturn> : MessageBusOptionsBase<TParameter>
    {

    }
}
