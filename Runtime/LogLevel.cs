namespace SmartLogger
{
    /// <summary>
    /// Defines logging severity levels for message filtering.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Detailed debug-level messages.
        /// </summary>
        Verbose = 0,

        /// <summary>
        /// Significant events (e.g., service start/stop).
        /// </summary>
        Important = 1,

        /// <summary>
        /// Critical errors requiring immediate intervention.
        /// </summary>
        Critical = 2
    }
}