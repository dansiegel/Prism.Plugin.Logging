namespace Prism.Logging.Syslog
{
    // https://en.wikipedia.org/wiki/Syslog
    public enum Facility
    {
        /// <summary>
        /// kernel messages
        /// </summary>
        Kernel = 0,

        /// <summary>
        /// user-level messages
        /// </summary>
        User = 1,

        /// <summary>
        /// mail system
        /// </summary>
        Mail = 2,

        /// <summary>
        /// system daemons
        /// </summary>
        Daemon = 3,

        /// <summary>
        /// security/authorization messages
        /// </summary>
        Auth = 4,

        /// <summary>
        /// messages generated internally by syslogd
        /// </summary>
        Syslog = 5,

        /// <summary>
        /// line printer subsystem
        /// </summary>
        Lpr = 6,

        /// <summary>
        /// network news subsystem
        /// </summary>
        News = 7,

        /// <summary>
        /// UUCP subsystem
        /// </summary>
        UUCP = 8,

        /// <summary>
        /// clock daemon
        /// </summary>
        Clock = 9,

        /// <summary>
        /// security/authorization messages
        /// </summary>
        AuthPriv = 10,

        /// <summary>
        /// FTP daemon
        /// </summary>
        FTP = 11,

        /// <summary>
        /// NTP subsystem
        /// </summary>
        NTP = 12,

        /// <summary>
        /// log audit
        /// </summary>
        LogAudit = 13,

        /// <summary>
        /// log alert
        /// </summary>
        LogAlert = 14,

        /// <summary>
        /// scheduling daemon
        /// </summary>
        Cron = 15,

        /// <summary>
        /// local use 0  (local0)
        /// </summary>
        Local0 = 16,

        /// <summary>
        /// local use 1  (local1)
        /// </summary>
        Local1 = 17,

        /// <summary>
        /// local use 2  (local2)
        /// </summary>
        Local2 = 18,

        /// <summary>
        /// local use 3  (local3)
        /// </summary>
        Local3 = 19,

        /// <summary>
        /// local use 4  (local4)
        /// </summary>
        Local4 = 20,

        /// <summary>
        /// local use 5  (local5)
        /// </summary>
        Local5 = 21,

        /// <summary>
        /// local use 6  (local6)
        /// </summary>
        Local6 = 22,

        /// <summary>
        /// local use 7  (local7)
        /// </summary>
        Local7 = 23,
    }
}