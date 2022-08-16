using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus
{
    /// <summary>
    /// Defines the methods of MessageBus. This is an abstract class.
    /// </summary>
    public abstract partial class MessageBusBase : IDisposable
    {
        /// <inheritdoc />
        public abstract void Dispose();
    }
}
