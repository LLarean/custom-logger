using UnityEngine;

namespace Utilities.Logging
{
    [CreateAssetMenu(fileName = "SmartLoggerConfig", menuName = "SmartLogger/Config")]
    public class LoggerConfig : ScriptableObject
    {
        [SerializeField] private LogLevel _logDisplayLevel = LogLevel.Verbose;
        [SerializeField] private bool _enableInBuild = false;
        [SerializeField] private bool _showTimestamp = true;
        [SerializeField] private bool _enableColorCoding = true;
        [SerializeField] private int _maxCacheSize = 1000;

        public LogLevel LogDisplayLevel => _logDisplayLevel;
        public bool EnableInBuild => _enableInBuild;
        public bool ShowTimestamp => _showTimestamp;
        public bool EnableColorCoding => _enableColorCoding;
        public int MaxCacheSize => _maxCacheSize;

        private static LoggerConfig _instance;
        
        public static LoggerConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<LoggerConfig>("SmartLoggerConfig");
                    if (_instance == null)
                    {
                        _instance = CreateInstance<LoggerConfig>();
                    }
                }
                return _instance;
            }
        }
    }
}