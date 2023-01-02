using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;
using Shared.ExternalServices.Constant;
using Shared.ExternalServices.DTOs;
using Shared.ExternalServices.Interfaces;
using System.Text;

namespace Shared.ExternalServices.APIServices
{
    public class DmsAzureMessageBus : IDmsAzureMessageBus
    {

        public async Task PublishMessage(IntegrationBaseMessage message, string topicName)
        {
            message.Id = Guid.NewGuid();

            var jsonMessage = JsonConvert.SerializeObject(message);
            var busMessage = new Message(Encoding.UTF8.GetBytes(jsonMessage))
            {
                PartitionKey = Guid.NewGuid().ToString()
            };

            ISenderClient topicClient = new TopicClient(Settings.SERVICE_BUS_CONNECTION_STRING, topicName);
            await topicClient.SendAsync(busMessage);
            Console.WriteLine($"Sent message to {topicClient.Path}");
            await topicClient.CloseAsync();

        }

        public async Task PublishMessage(List<string> messages, string topicName)
        {
            List<Message> busMessages = new();
            var partitionId = Guid.NewGuid().ToString();
            messages.ForEach(x =>
            {
                var busMessage = new Message(Encoding.UTF8.GetBytes(x))
                {
                    PartitionKey = partitionId
                };
                busMessages.Add(busMessage);
            });

            ISenderClient topicClient = new TopicClient(Settings.SERVICE_BUS_CONNECTION_STRING, topicName);
            await topicClient.SendAsync(busMessages);
            Console.WriteLine($"Sent message to {topicClient.Path}");
            await topicClient.CloseAsync();

        }
    }
}
