namespace Prism.Logging.Logger
{
    public interface IPlatformStringStorage
    {
        string Read();
        bool Write(string data);
    }
}