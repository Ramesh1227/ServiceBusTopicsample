using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
namespace ServiceBusTpoicExample
{
    public static class SendMessagetoTopic
    {
        static ServiceBusClient client;
        static ServiceBusSender sender;

        private const int numOfMessages = 3;
        public async static Task sendmessage(string connectionString, string topicName, string msg)
        {
            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(topicName);

            var message = new ServiceBusMessage (msg);
            // use this for more message to send as batch
            //for (int i = 1; i <= numOfMessages; i++)
            //{
            //    // try adding a message to the batch
            //    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
            //    {
            //        // if it is too large for the batch
            //        throw new Exception($"The message {i} is too large to fit in the batch.");
            //    }
            //}

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus topic
                await sender.SendMessageAsync(message);
                Console.WriteLine($"A batch of messages has been published to the topic.");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }

        }
    }
}
