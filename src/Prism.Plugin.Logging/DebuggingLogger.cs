using static System.Diagnostics.Debug;

namespace Prism.Logging
{
    public class DebuggingLogger : ILogger, ILoggerFacade
    {
        public void Alert( string message ) => 
            WriteFormatedMessage( "Alert", message );

        public void Debug( string message ) =>
            WriteFormatedMessage( "Debug", message );

        public void Error( string message ) =>
            WriteFormatedMessage( "Error", message );

        public void Info( string message ) =>
            WriteFormatedMessage( "Info", message );

        public void Log( string message, Category category, Priority priority ) =>
            WriteLine( $"{category} - Priority {priority}: {message}" );

        public void Notice( string message ) =>
            WriteFormatedMessage( "Notice", message );

        public void Warn( string message ) =>
            WriteFormatedMessage( "Warn", message );

        private void WriteFormatedMessage( string type, string message ) => WriteLine( $"{type}: {message}" );
    }
}
