using System;
using static System.Diagnostics.Debug;

namespace Prism.Logging
{
    public static class StaticLogger
    {
        public static Action<object> AlertLogger = ( message ) => WriteLine( message );
        public static Action<object> DebugLogger = ( message ) => WriteLine( message );
        public static Action<object> ErrorLogger = ( message ) => WriteLine( message );
        public static Action<Exception> ExceptionLogger = ( exception ) => WriteLine( exception );
        public static Action<object> InfoLogger = ( message ) => WriteLine( message );
        public static Action<object> NoticeLogger = ( message ) => WriteLine( message );
        public static Action<object> WarningLogger = ( message ) => WriteLine( message );
    }
}
