using System.Text;

namespace Prism.Logging.Sockets
{
    public interface ILogMessage
    {
        byte[] GetBytes();
    }
}