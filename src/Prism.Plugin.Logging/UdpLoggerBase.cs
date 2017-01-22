using Prism.Logging.Syslog;

namespace Prism.Logging
{
    public class UdpLoggerBase : ILogger, ILoggerFacade
    {
        protected IUdpMessenger _messenger { get; }
        protected string _hostOrIp { get; }
        protected int _port { get; }
        protected Filter _filter { get; }
        const string messageFormat = "{0}: {1}";
        const string prismMessageFormat = "{0}: Priority {1} -\n\t{2}\n\n";

        public UdpLoggerBase( IUdpMessenger messenger, string hostOrIp = "", int port = 514, Filter filter = Filter.All )
        {
            _filter = filter;
            _messenger = messenger;
            _hostOrIp = hostOrIp;
            _port = port;
        }

        #region ILogger

        public virtual void Alert( string message )
        {
            if( ShouldFilterMessage( message, Level.Alert ) ) return;

            SendMessage( messageFormat, "Alert", message );
        }

        public virtual void Debug( string message )
        {
            if( ShouldFilterMessage( message, Level.Debug ) ) return;

            SendMessage( messageFormat, "Debug", message );
        }

        public virtual void Error( string message )
        {
            if( ShouldFilterMessage( message, Level.Error ) ) return;

            SendMessage( messageFormat, "Error", message );
        }

        public virtual void Info( string message )
        {
            if( ShouldFilterMessage( message, Level.Information ) ) return;

            SendMessage( messageFormat, "Info", message );
        }

        public virtual void Notice( string message )
        {
            if( ShouldFilterMessage( message, Level.Notice ) ) return;

            SendMessage( messageFormat, "Notice", message );
        }

        public virtual void Warn( string message )
        {
            if( ShouldFilterMessage( message, Level.Warning ) ) return;

            SendMessage( messageFormat, "Warn", message );
        }

        #endregion

        #region ILoggerFacade

        public virtual void Log( string message, Category category, Priority priority )
        {
            if( ShouldFilterMessage( message, category, priority ) ) return;

            SendMessage( prismMessageFormat, category, priority, message );
        }

        #endregion

        public virtual void SendMessage( string format, params object[] args ) => 
            SendMessage( GetFormattedString( format, args ) );

        public virtual async void SendMessage( string formattedMessage )
        {
            if( string.IsNullOrEmpty( _hostOrIp ) )
            {
                await _messenger.SendMulticastMessage( formattedMessage, _port );
            }
            else
            {
                await _messenger.SendMessage( formattedMessage, _hostOrIp, _port );
            }
        }

        protected virtual bool ShouldFilterMessage( string message, Category category, Priority priority )
        {
            if( string.IsNullOrWhiteSpace( message ) ) return true;

            switch( _filter )
            {
                case Filter.None:
                    return true;
                case Filter.All:
                case Filter.Debug:
                    return false;
                case Filter.Emergency:
                case Filter.Alert:
                case Filter.Critical:
                case Filter.Error:
                    return category != Category.Exception;
                case Filter.Warning:
                    return category == Category.Debug || category == Category.Info;
                case Filter.Information:
                    return category == Category.Debug;
                default:
                    // We shouldn't ever get here...
                    return false;
            }
        }

        protected virtual bool ShouldFilterMessage( string message, Level level )
        {
            if( _filter == Filter.None || string.IsNullOrWhiteSpace( message ) ) return true;

            int filterValue = ( int )_filter;
            int levelValue = ( int )level;

            if( filterValue >= 7 ) return false;

            return !( filterValue >= levelValue );
        }

        protected string GetFormattedString( string format, params object[] args ) =>
            args?.Length > 0 ? string.Format( format, args ) : format;
    }
}
