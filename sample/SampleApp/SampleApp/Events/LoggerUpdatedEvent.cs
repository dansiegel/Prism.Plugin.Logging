using Prism.Events;
using Prism.Logging;

namespace SampleApp.Events
{
    public class LoggerUpdatedEvent : PubSubEvent<ILogger>
    {
    }
}
