using Shared.ExternalServices.DTOs;

namespace Shared.ExternalServices.Interfaces
{
    public interface IDmsAzureMessageBus
    {
        /// <summary>
        /// To Publish a message to Azure Service Bus
        /// </summary>
        /// <param name="message">The message Object to Publish</param>
        /// <param name="topicName"></param>
        /// <returns></returns>
        Task PublishMessage(IntegrationBaseMessage message, string topicName);

        /// <summary>
        /// To Publish a list of messages to Azure Service Bus
        /// </summary>
        /// <param name="messages">The List of messages to publish. These messages must be in a Json Serialized form</param>
        /// <param name="topicName">The name of the topic to Publish messages to.</param>
        /// <returns></returns>
        Task PublishMessage(List<string> messages, string topicName);
    }
}
