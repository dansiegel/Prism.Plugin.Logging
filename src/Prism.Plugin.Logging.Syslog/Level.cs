namespace Prism.Logging.Syslog
{
    public enum Level
    {
        /// <summary>
        /// Emergency: system is unusable
        /// </summary>
        Emergency = 0,

        /// <summary>
        /// Alert: action must be taken immediately
        /// </summary>
        Alert = 1,

        /// <summary>
        /// Critical: critical conditions
        /// </summary>
        Critical = 2,

        /// <summary>
        /// Error: error conditions
        /// </summary>
        Error = 3,

        /// <summary>
        /// Warning: warning conditions
        /// </summary>
        Warning = 4,

        /// <summary>
        /// Notice: normal but significant condition
        /// </summary>
        Notice = 5,

        /// <summary>
        /// Informational: informational messages
        /// </summary>
        Information = 6,

        /// <summary>
        /// Debug: debug-level messages
        /// </summary>
        Debug = 7,
    }
}