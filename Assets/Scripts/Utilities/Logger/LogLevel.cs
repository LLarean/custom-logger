namespace DebugTools
{
    /// <summary>
    /// Defines the importance levels for log messages
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Detailed debug information (most verbose level)
        /// </summary>
        Verbose = 0,

        /// <summary>
        /// Important messages that should be monitored
        /// </summary>
        Important = 1,
    
        /// <summary>
        /// Critical errors that require immediate attention
        /// </summary>
        Critical = 2,
    }
}