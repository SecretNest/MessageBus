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
        private bool _disposedValue;

        /// <summary>
        /// Disposes the instance.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed state.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    DisposeManagedState();
                }

                DisposeUnmanagedState();
                _disposedValue = true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes managed state.
        /// </summary>
        protected abstract void DisposeManagedState();

        /// <summary>
        /// Disposes unmanaged state.
        /// </summary>
        private protected virtual void DisposeUnmanagedState()
        {
        }
    }
}
