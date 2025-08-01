using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Debug = UnityEngine.Debug;

namespace DebugTools
{
    public static class EnhancedLogger
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
        /// The minimum level of importance of the logs displayed, 0 only critical, 1 only important and critical, 2 all logs
        /// </summary>
        private const LogLevel LogDisplayLevel = LogLevel.Verbose;
        
        private static string _lastCallerData;
        private static string _lastMessage;
        
        /// <summary>
        /// Output a regular message to the console
        /// </summary>
        /// <param name="messageText">Message to the console</param>
        /// <param name="importanceLevel">-</param>
        public static void Log(string messageText, LogLevel importanceLevel = LogLevel.Verbose)
        {
            if(!IsDevelopmentModeEnabled) return;
            
            if(LogDisplayLevel < importanceLevel) return;
            
            if (string.IsNullOrEmpty(messageText)) throw new ArgumentNullException(nameof(messageText));
            
            Debug.Log($"{GetCallerData()}'{messageText}'");
        }
        
        public static void LogFromAsync(string messageText, [CallerMemberName] string caller = "", LogLevel importanceLevel = LogLevel.Verbose)
        {
            if(!IsDevelopmentModeEnabled) return;
            
            if(LogDisplayLevel < importanceLevel) return;

            if (string.IsNullOrEmpty(messageText)) throw new ArgumentNullException(nameof(messageText));
            
            
            Debug.Log($"Class: '{caller}', Message: '{messageText}'");
        }

        /// <summary>
        /// Output a warning message to the console
        /// </summary>
        /// <param name="messageText">Message to the console</param>
        public static void LogWarning(string messageText)
        {
            if(!IsDevelopmentModeEnabled) return;

            if (string.IsNullOrEmpty(messageText)) throw new ArgumentNullException(nameof(messageText));
            
            Debug.LogWarning($"<color=yellow>{GetCallerData()}'{messageText}'</color>");
        }

        /// <summary>
        /// Output a critical message to the console, Note that this pauses the editor when 'ErrorPause' is enabled.
        /// </summary>
        /// <param name="messageText">Message to the console</param>
        public static void LogError(string messageText)
        {
            if(!IsDevelopmentModeEnabled) return;

            if (string.IsNullOrEmpty(messageText)) throw new ArgumentNullException(nameof(messageText));
            
            Debug.LogError($"<color=red>{GetCallerData()}'{messageText}'</color>");
        }
        
        /// <summary>
        /// It does not work correctly for asynchronous methods
        /// </summary>
        /// <returns></returns>
        // private static string GetCallerData()
        // {
        //     StackTrace stackTrace = new StackTrace();
        //     StackFrame callingFrame = stackTrace.GetFrame(2);
        //     
        //     if (callingFrame == null) return "Unknown caller: ";
        //     
        //     MethodBase callingMethod = callingFrame.GetMethod();
        //     string callingClassName = callingMethod.ReflectedType.Name;
        //
        //     
        //     string className = callingMethod?.ReflectedType?.Name ?? "UnknownClass";
        //     string methodName = callingMethod?.Name ?? "UnknownMethod";
        //     
        //     var callerData = $"Class: '{className}', Method: '{methodName}', Message: ";
        //     return callerData;
        // }
        
        private static string GetCallerData() => 
            new StackTrace(2, false).GetFrame(0)?.GetMethod() is MethodBase method 
                ? $"Class: '{method.ReflectedType?.Name}', Method: '{method.Name}', Message: "
                : "Unknown caller: ";
    }
}