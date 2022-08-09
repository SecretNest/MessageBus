﻿using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.MessageNameMatching;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    public partial class MessageBus : MessageBusBase
    {
        /// <summary>
        /// Gets or sets whether should remove the sequencer when the last related publisher is removed. Default value is <see langword="true"/>.
        /// </summary>
        public bool AutoShrink { get; set; } = true;

        /// <summary>
        /// Removes all sequencers those no publisher is attached.
        /// </summary>
        public void ShrinkSequencers()
        {
            var messageNames = _sequencers.Where(i => i.Value.Publishers.IsEmpty).Select(i => i.Key).ToArray();
            foreach (var messageName in messageNames)
            {
                _sequencers.Remove(messageName, out _);
            }
        }






        /// <inheritdoc />
        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}