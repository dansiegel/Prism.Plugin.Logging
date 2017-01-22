using System;

namespace Prism.Logging
{
    public static class ILoggerExtensions
    {
        /// <summary>
        /// Logs the message as an alert.
        /// </summary>
        /// <param name="logger">The ILogger Instance</param>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public static void Alert( this ILogger logger, string format, params object[] args ) =>
            logger.Alert( GetFormattedString( format, args ) );

        /// <summary>
        /// Logs the message as an alert
        /// </summary>
        /// <param name="logger">The ILogger Instance</param>
        /// <param name="message">Message to log</param>
        public static void Alert( this ILogger logger, object message ) =>
            logger.Alert( message.ToString() );

        /// <summary>
        /// Logs the message as a debug notice.
        /// </summary>
        /// <param name="logger">The ILogger Instance</param>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public static void Debug( this ILogger logger, string format, params object[] args ) =>
            logger.Debug( GetFormattedString( format, args ) );

        /// <summary>
        /// Logs the message as a debug notice.
        /// </summary>
        /// <param name="logger">The ILogger Instance</param>
        /// <param name="message">Message to log</param>
        public static void Debug( this ILogger logger, object message ) =>
            logger.Debug( message.ToString() );

        /// <summary>
        /// Logs the message as an informational notice.
        /// </summary>
        /// <param name="logger">The ILogger Instance</param>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public static void Info( this ILogger logger, string format, params object[] args ) =>
            logger.Info( GetFormattedString( format, args ) );

        /// <summary>
        /// Logs the message as an informational notice.
        /// </summary>
        /// <param name="logger">The ILogger Instance</param>
        /// <param name="message">Message to log</param>
        public static void Info( this ILogger logger, object message ) =>
            logger.Info( message.ToString() );

        /// <summary>
        /// Logs the message as a notice.
        /// </summary>
        /// <param name="logger">The ILogger Instance</param>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public static void Notice( this ILogger logger, string format, params object[] args ) =>
            logger.Notice( GetFormattedString( format, args ) );

        /// <summary>
        /// Logs the message as a debug notice.
        /// </summary>
        /// <param name="logger">The ILogger Instance</param>
        /// <param name="message">Message to log</param>
        public static void Notice( this ILogger logger, object message ) =>
            logger.Notice( message.ToString() );

        /// <summary>
        /// Logs the message as a warning.
        /// </summary>
        /// <param name="logger">The ILogger Instance</param>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public static void Warn( this ILogger logger, string format, params object[] args ) =>
            logger.Warn( GetFormattedString( format, args ) );

        /// <summary>
        /// Logs the message as a warning.
        /// </summary>
        /// <param name="logger">The ILogger Instance</param>
        /// <param name="message">Message to log</param>
        public static void Warn( this ILogger logger, object message ) =>
            logger.Warn( message.ToString() );

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="logger">The ILogger Instance</param>
        /// <param name="exception">The exception.</param>
        public static void Error( this ILogger logger, Exception exception ) =>
            logger.Error( exception.ToString() );

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="logger">The ILogger Instance</param>
        /// <param name="message">Message to log</param>
        public static void Error( this ILogger logger, object message ) =>
            logger.Error( message.ToString() );

        /// <summary>
        /// Logs the error
        /// </summary>
        /// <param name="logger">The ILogger Instance</param>
        /// <param name="format">A formatted message</param>
        /// <param name="args">Parameters to be injected into the message</param>
        public static void Error( this ILogger logger, string format, params object[] args ) =>
            logger.Error( GetFormattedString( format, args ) );

        private static string GetFormattedString( string format, object[] args ) =>
            args?.Length > 0 ? string.Format( format, args ) : format;
    }
}
