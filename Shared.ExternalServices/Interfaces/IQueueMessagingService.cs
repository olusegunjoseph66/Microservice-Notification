using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.Interfaces
{
    public interface IQueueMessagingService
    {
        Task PublishMessage(dynamic request, string topicName);
    }
}
