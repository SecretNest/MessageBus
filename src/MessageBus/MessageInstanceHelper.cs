using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.MessageBus
{
    internal class MessageInstanceHelper
    {
        public MessageInstanceHelper(string messageName)
        {
            MessageName = messageName;
        }

        public string MessageName { get; }

        public Guid? ReturnValueSourceSubscriberId { get; private set; }

        public object? SubscriberResult { get; private set; }

        public bool IsSubscriberResultSet { get; private set; }



        public MessageInstance GetMessageInstance()
        {

        }

        public void SetSubscriberResult(object? value, Guid? subscriberId)
        {

        }
        
        public void SetSubscriberResult<TSubscriberReturn>(TSubscriberReturn? value, Guid? subscriberId)
        {

        }

        public void SetFinalResult<TExecutorReturn>(TExecutorReturn? value)
        {

        }

        public void AcceptFinalResult<TExecutorReturn>() //use current value, check null, transform type
        {

        }

        public TExecutorReturn? GetFinalResult<TExecutorReturn>()
        {

        }

        public MessageInstanceWithVoidReturnValue GetMessageInstanceWithVoidReturnValue()
        {

        }

        public MessageInstanceWithReturnValue<TExecutorReturn> GetMessageInstanceWithReturnValue<TExecutorReturn>()
        {

        }

        public MessageInstanceWithReturnValue GetMessageInstanceWithReturnValue()
        {
            // not used 
        }
    }
}
