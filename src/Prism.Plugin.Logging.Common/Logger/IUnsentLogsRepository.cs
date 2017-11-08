namespace Prism.Logging.Logger
{
    public interface IUnsentLogsRepository
    {
        bool IsEmpty { get; }

        bool Add(Log log);
        bool Remove(Log log);
        Log GetLog();
    }
}