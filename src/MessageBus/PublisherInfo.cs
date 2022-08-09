using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    internal abstract class PublisherInfoBase
    {
        public abstract MessageExecutorSequencerSupport MessageExecutorSequencerSupport { get; }
        public abstract bool IsAlwaysExecutionAll { get; }
        public abstract MessageExecutorBase MessageExecutorGeneric { get; }
    }

    internal class PublisherInfo<TParameter> : PublisherInfoBase
    {
        public PublisherInfo(MessageBusPublisherOptions<TParameter>? options)
        {
            var executor = new MessageExecutor<TParameter>();

            if (options != null)
            {
                IsAlwaysExecutionAll = options.IsAlwaysExecutionAll;
                executor.ApplyPublisherOptions(options.ArgumentConvertingCallback);
            }
            else
            {
                IsAlwaysExecutionAll = false;
            }

            MessageExecutor = executor;
        }

        public override MessageExecutorSequencerSupport MessageExecutorSequencerSupport => MessageExecutor;
        public override bool IsAlwaysExecutionAll { get; }
        public override MessageExecutorBase MessageExecutorGeneric => MessageExecutor;
        public MessageExecutor<TParameter> MessageExecutor { get; }
    }

    internal class PublisherInfo<TParameter, TReturn> : PublisherInfoBase
    {
        public PublisherInfo(MessageBusPublisherOptions<TParameter, TReturn>? options)
        {
            var executor = new MessageExecutor<TParameter, TReturn>();

            if (options != null)
            {
                IsAlwaysExecutionAll = options.IsAlwaysExecutionAll;
                executor.ApplyPublisherOptions(options.DefaultReturnValue, options.ArgumentConvertingCallback, options.ReturnValueConvertingCallback);
            }
            else
            {
                IsAlwaysExecutionAll = false;
            }

            MessageExecutor = executor;
        }

        public override MessageExecutorSequencerSupport MessageExecutorSequencerSupport => MessageExecutor;
        public override bool IsAlwaysExecutionAll { get; }
        public override MessageExecutorBase MessageExecutorGeneric => MessageExecutor;
        public MessageExecutor<TParameter, TReturn> MessageExecutor { get; }
    }
}
