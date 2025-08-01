using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Debug = UnityEngine.Debug;

namespace Utilities.Logging
{
    public enum LogLevel
    {
        Verbose = 0,
        Important = 1,
        Critical = 2
    }

    public static class SmartLogger
    {
        /// <summary>
        /// Indicates whether logs should be displayed at all
        /// </summary>
        private static readonly bool IsDevelopmentModeEnabled = 
#if UNITY_EDITOR
            true;
#else
            Debug.isDebugBuild;
#endif
        
        /// <summary>
        /// The minimum level of importance of the logs displayed
        /// </summary>
        private const LogLevel LogDisplayLevel = LogLevel.Verbose;
        
        // Кэш для оптимизации повторных вызовов из того же места
        private static readonly ConcurrentDictionary<string, string> CallerDataCache 
            = new ConcurrentDictionary<string, string>();
        
        /// <summary>
        /// Output a regular message to the console
        /// </summary>
        /// <param name="messageText">Message to the console</param>
        /// <param name="importanceLevel">Importance level of the message</param>
        /// <param name="callerMemberName">Automatically filled by compiler</param>
        /// <param name="callerFilePath">Automatically filled by compiler</param>
        /// <param name="callerLineNumber">Automatically filled by compiler</param>
        public static void Log(
            string messageText, 
            LogLevel importanceLevel = LogLevel.Verbose,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            if (!ShouldLog(importanceLevel)) return;
            
            ValidateMessage(messageText);
            
            var callerInfo = GetOptimizedCallerInfo(callerMemberName, callerFilePath, callerLineNumber);
            Debug.Log($"{callerInfo}'{messageText}'");
        }

        /// <summary>
        /// Output a warning message to the console
        /// </summary>
        /// <param name="messageText">Message to the console</param>
        /// <param name="callerMemberName">Automatically filled by compiler</param>
        /// <param name="callerFilePath">Automatically filled by compiler</param>
        /// <param name="callerLineNumber">Automatically filled by compiler</param>
        public static void LogWarning(
            string messageText,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            if (!IsDevelopmentModeEnabled) return;

            ValidateMessage(messageText);
            
            var callerInfo = GetOptimizedCallerInfo(callerMemberName, callerFilePath, callerLineNumber);
            Debug.LogWarning($"<color=yellow>{callerInfo}'{messageText}'</color>");
        }

        /// <summary>
        /// Output a critical message to the console. Note that this pauses the editor when 'ErrorPause' is enabled.
        /// </summary>
        /// <param name="messageText">Message to the console</param>
        /// <param name="callerMemberName">Automatically filled by compiler</param>
        /// <param name="callerFilePath">Automatically filled by compiler</param>
        /// <param name="callerLineNumber">Automatically filled by compiler</param>
        public static void LogError(
            string messageText,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            if (!IsDevelopmentModeEnabled) return;

            ValidateMessage(messageText);
            
            var callerInfo = GetOptimizedCallerInfo(callerMemberName, callerFilePath, callerLineNumber);
            Debug.LogError($"<color=red>{callerInfo}'{messageText}'</color>");
        }

        /// <summary>
        /// Log an exception with full stack trace information
        /// </summary>
        /// <param name="exception">Exception to log</param>
        /// <param name="additionalMessage">Additional context message</param>
        /// <param name="callerMemberName">Automatically filled by compiler</param>
        /// <param name="callerFilePath">Automatically filled by compiler</param>
        /// <param name="callerLineNumber">Automatically filled by compiler</param>
        public static void LogException(
            Exception exception, 
            string additionalMessage = "",
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            if (!IsDevelopmentModeEnabled) return;
            if (exception == null) return;

            var callerInfo = GetOptimizedCallerInfo(callerMemberName, callerFilePath, callerLineNumber);
            var message = string.IsNullOrEmpty(additionalMessage) 
                ? $"{callerInfo}Exception: {exception.Message}" 
                : $"{callerInfo}'{additionalMessage}' - Exception: {exception.Message}";
                
            Debug.LogException(exception);
            Debug.LogError($"<color=red>{message}</color>");
        }

        /// <summary>
        /// Conditional logging - only logs if condition is true
        /// </summary>
        /// <param name="condition">Condition to check</param>
        /// <param name="messageText">Message to log if condition is true</param>
        /// <param name="importanceLevel">Importance level</param>
        /// <param name="callerMemberName">Automatically filled by compiler</param>
        /// <param name="callerFilePath">Automatically filled by compiler</param>
        /// <param name="callerLineNumber">Automatically filled by compiler</param>
        public static void LogIf(
            bool condition, 
            string messageText, 
            LogLevel importanceLevel = LogLevel.Verbose,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            if (condition)
            {
                Log(messageText, importanceLevel, callerMemberName, callerFilePath, callerLineNumber);
            }
        }

        /// <summary>
        /// Log with formatted string (similar to string.Format)
        /// </summary>
        /// <param name="format">Format string</param>
        /// <param name="args">Arguments for formatting</param>
        public static void LogFormat(string format, params object[] args)
        {
            if (!ShouldLog(LogLevel.Verbose)) return;
            
            try
            {
                var message = string.Format(format, args);
                Log(message);
            }
            catch (FormatException ex)
            {
                LogError($"Format error in LogFormat: {ex.Message}. Original format: '{format}'");
            }
        }

        #region Private Helper Methods

        /// <summary>
        /// Check if logging should occur based on development mode and log level
        /// </summary>
        private static bool ShouldLog(LogLevel importanceLevel)
        {
            return IsDevelopmentModeEnabled && LogDisplayLevel <= importanceLevel;
        }

        /// <summary>
        /// Validate that message is not null or empty
        /// </summary>
        private static void ValidateMessage(string messageText)
        {
            if (string.IsNullOrEmpty(messageText)) 
                throw new ArgumentNullException(nameof(messageText), "Message cannot be null or empty");
        }

        /// <summary>
        /// Get optimized caller information using compiler attributes (more reliable than StackTrace)
        /// </summary>
        private static string GetOptimizedCallerInfo(string memberName, string filePath, int lineNumber)
        {
            var cacheKey = $"{memberName}:{Path.GetFileName(filePath)}:{lineNumber}";
            
            return CallerDataCache.GetOrAdd(cacheKey, _ =>
            {
                var className = Path.GetFileNameWithoutExtension(filePath);
                return $"Class: '{className}', Method: '{memberName}', Line: {lineNumber}, Message: ";
            });
        }

        /// <summary>
        /// Fallback method using StackTrace (kept for backward compatibility)
        /// Note: This method may not work correctly with async methods
        /// </summary>
        private static string GetCallerDataFromStackTrace()
        {
            try
            {
                var stackTrace = new StackTrace(3, false); // Skip this method, Log method, and direct caller
                var frame = stackTrace.GetFrame(0);
                
                if (frame?.GetMethod() is MethodBase method)
                {
                    var className = method.ReflectedType?.Name ?? "UnknownClass";
                    var methodName = method.Name;
                    return $"Class: '{className}', Method: '{methodName}', Message: ";
                }
            }
            catch (Exception)
            {
                // Ignore exceptions in debug code
            }
            
            return "Unknown caller: ";
        }

        #endregion

        #region Configuration Methods

        /// <summary>
        /// Check if development mode is enabled
        /// </summary>
        public static bool IsLoggingEnabled => IsDevelopmentModeEnabled;

        /// <summary>
        /// Get current log display level
        /// </summary>
        public static LogLevel CurrentLogLevel => LogDisplayLevel;

        /// <summary>
        /// Clear the caller data cache (useful for memory management in long-running applications)
        /// </summary>
        public static void ClearCache()
        {
            CallerDataCache.Clear();
        }

        #endregion
    }
}