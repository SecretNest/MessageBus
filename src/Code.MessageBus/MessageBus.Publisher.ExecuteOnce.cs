using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SecretNest.MessageBus.Options;

namespace SecretNest.MessageBus
{
    partial class MessageBus
    {
        MessageInstanceHelper ExecuteOnceInternal<TParameter>(string messageName, TParameter argument,
            Func<TParameter?, object?>? argumentConvertingCallback, bool? isAlwaysExecuteAll)
        {
            var subscribers = GetSubscribersFromPool(messageName)
                .Select(i => i.Value)
                .OrderBy(i => i.Sequence)
                .ToArray();
            MessageInstanceHelper helper;
            if (argumentConvertingCallback != null)
            {
                var converted = argumentConvertingCallback(argument);
                helper = Sequencer.ExecuteOnce(messageName, subscribers, converted, isAlwaysExecuteAll == true);
            }
            else
            {
                helper = Sequencer.ExecuteOnce(messageName, subscribers, argument, isAlwaysExecuteAll == true);
            }

            return helper;
        }

        async Task<MessageInstanceHelper> ExecuteOnceInternalAsync<TParameter>(string messageName, TParameter argument,
            Func<TParameter?, object?>? argumentConvertingCallback, bool? isAlwaysExecuteAll, CancellationToken cancellationToken)
        {
            var subscribers = GetSubscribersFromPool(messageName)
                .Select(i => i.Value)
                .OrderBy(i => i.Sequence)
                .ToArray();
            MessageInstanceHelper helper;
            if (argumentConvertingCallback != null)
            {
                var converted = argumentConvertingCallback(argument);
                helper = await Sequencer.ExecuteOnceAsync(messageName, subscribers, converted, isAlwaysExecuteAll == true, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                helper = await Sequencer.ExecuteOnceAsync(messageName, subscribers, argument, isAlwaysExecuteAll == true, cancellationToken).ConfigureAwait(false);
            }

            return helper;
        }

        private static TReturn? ExecuteOnceInternalGetFinalResult<TReturn>(MessageInstanceHelper helper, TReturn? defaultReturnValue, Func<object?, TReturn?>? returnValueConvertingCallback)
        {
            if (helper.ReturnValueSourceSubscriberId == null) //default return required
            {
                return defaultReturnValue;
            }
            else
            {
                var result = helper.SubscriberResult;
                if (returnValueConvertingCallback != null)
                {
                    return returnValueConvertingCallback(result);
                }
                else if (result == null)
                {
                    return default;
                }
                else
                {
                    return (TReturn?)result;
                }
            }
        }
    }
}
