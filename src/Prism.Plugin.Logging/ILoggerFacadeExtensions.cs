using System;
#if !PROFILE259
using System.Net;
using System.Net.Http;
#endif

namespace Prism.Logging
{
    public static class ILoggerFacadeExtensions
    {
        public static void Log( this ILoggerFacade logger, Exception e, Priority priority = Priority.High ) =>
            logger.Log( $"{e}", Category.Exception, priority );

        public static void Log( this ILoggerFacade logger, object message, Category category = Category.Debug, Priority priority = Priority.None ) =>
            logger.Log( $"{message}", category, priority );

        public static void Log( this ILoggerFacade logger, string message ) =>
            logger.Log( message, Category.Debug, Priority.None );

#if !PROFILE259

        public static async void Log( this ILoggerFacade logger, HttpResponseMessage response )
        {
            if( response == null )
            {
                logger.Log( "Unable to log HttpResponseMessage. Message object is null.", Category.Warn );
                return;
            }

            Category category = Category.Debug;

            if( response.IsSuccessStatusCode ) category = Category.Info;

            switch( response.StatusCode )
            {
                case HttpStatusCode.Ambiguous:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.ServiceUnavailable:
                    category = Category.Exception;
                    break;
                case HttpStatusCode.Conflict:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.GatewayTimeout:
                case HttpStatusCode.LengthRequired:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.NotImplemented:
                case HttpStatusCode.Redirect:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.UnsupportedMediaType:
                    category = Category.Warn;
                    break;
            }

            logger.Log( await response.Content.ReadAsStringAsync(), category );
        }

#endif

        public static void WarnIfNull( this ILoggerFacade logger, object obj, string name )
        {
            if( obj == null )
                logger.Log( $"The provided object for {name} is null", Category.Warn );
        }

        public static void WarnIfNull( this ILoggerFacade logger, string obj, string name )
        {
            if( string.IsNullOrWhiteSpace( obj ) )
                logger.Log( $"The provided string {name} is null, empty or whitespace.", Category.Warn );
        }

        public static void LogIf( this ILoggerFacade logger, bool condition, string message, Category category = Category.Debug, Priority priority = Priority.None )
        {
            if( condition )
                logger.Log( message, category, priority );
        }

        public static void LogIf( this ILoggerFacade logger, bool condition, object message, Category category = Category.Debug, Priority priority = Priority.None )
        {
            if( condition )
                logger.Log( message, category, priority );
        }
    }
}
