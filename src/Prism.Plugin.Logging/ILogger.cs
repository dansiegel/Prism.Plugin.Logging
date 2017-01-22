using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prism.Logging
{
    /// <summary>
    /// A more advanced logging interface
    /// </summary>
    public interface ILogger
    {
        
        /// <summary>
        /// Logs the message as an alert
        /// </summary>
        /// <param name="message">Message to log</param>
        void Alert( string message );

        /// <summary>
        /// Logs the message as a debug notice.
        /// </summary>
        /// <param name="message">Message to log</param>
        void Debug( string message );

        /// <summary>
        /// Logs the message as an informational notice.
        /// </summary>
        /// <param name="message">Message to log</param>
        void Info( string message );

        /// <summary>
        /// Logs the message as a notice.
        /// </summary>
        /// <param name="message">Message to log</param>
        void Notice( string message );

        /// <summary>
        /// Logs the message as a warning.
        /// </summary>
        /// <param name="message">Message to log</param>
        void Warn( string message );

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">Message to log</param>
        void Error( string message );
    }
}
