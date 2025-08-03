using UnityEngine;
using UnityEngine.UI;

namespace SmartLogger.Samples.BasicUsage
{
    public class LoggerExample : MonoBehaviour
    {
        [SerializeField] private Button _log;
        [SerializeField] private Button _logWarning;
        [SerializeField] private Button _logError;

        private void Start()
        {
            _log.onClick.AddListener(Log);
            _logWarning.onClick.AddListener(LogWarning);
            _logError.onClick.AddListener(LogError);
        }

        private void OnDestroy()
        {
            _log.onClick.RemoveAllListeners();
            _logWarning.onClick.RemoveAllListeners();
            _logError.onClick.RemoveAllListeners();
        }

        private void Log()
        {
            Utilities.Logging.SmartLogger.Log("Hello World!");
        }

        private void LogWarning()
        {
            Utilities.Logging.SmartLogger.LogWarning("Hello World!");
        }

        private void LogError()
        {
            Utilities.Logging.SmartLogger.LogError("Hello World!");
        }
    }
}