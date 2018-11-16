using Prism.Events;

namespace SampleApp.Events
{
    public sealed class UpdateLoggerEvent : PubSubEvent<LoggerType>
    {
    }
}
