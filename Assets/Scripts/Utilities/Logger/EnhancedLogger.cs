using System.Diagnostics;
using System.Reflection;
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
        private const int LogDisplayLevel = 2;
        
        /// <summary>
        /// Output a regular message to the console
        /// </summary>
        /// <param name="message">Message to the console</param>
        /// <param name="importanceLevel">0 is critical, 1 is necessary to pay attention, 2 is the most unimportant</param>
        public static void Log(string message, int  importanceLevel = 2)
        {
            if(IsDevelopmentModeEnabled == false) return;
            
            if(LogDisplayLevel < importanceLevel) return;
            
            Debug.Log($"{GetCallerData()}'{message}'");
        }
        
        public static void LogFromAsync(string message, string caller, int  importanceLevel = 2)
        {
            if(IsDevelopmentModeEnabled == false) return;
            
            if(LogDisplayLevel < importanceLevel) return;
            
            Debug.Log($"Class: '{caller}', Message: '{message}'");
        }

        /// <summary>
        /// Output a warning message to the console
        /// </summary>
        /// <param name="message">Message to the console</param>
        public static void LogWarning(string message)
        {
            if(IsDevelopmentModeEnabled == false) return;

            Debug.LogWarning($"<color=yellow>{GetCallerData()}'{message}'</color>");
        }

        /// <summary>
        /// Output a critical message to the console, Note that this pauses the editor when 'ErrorPause' is enabled.
        /// </summary>
        /// <param name="message">Message to the console</param>
        public static void LogError(string message)
        {
            if(IsDevelopmentModeEnabled == false) return;

            Debug.LogError($"<color=red>{GetCallerData()}'{message}'</color>");
        }
        
        /// <summary>
        /// It does not work correctly for asynchronous methods
        /// </summary>
        /// <returns></returns>
        private static string GetCallerData()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame callingFrame = stackTrace.GetFrame(2);

            MethodBase callingMethod = callingFrame.GetMethod();
            string callingClassName = callingMethod.ReflectedType.Name;

            var callerData = $"Class: '{callingClassName}', Method: '{callingMethod.Name}', Message: ";
            return callerData;
        }
    }
}