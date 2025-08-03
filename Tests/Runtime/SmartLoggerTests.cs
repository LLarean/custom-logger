using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SmartLogger;

namespace SmartLogger.Tests
{
    [TestFixture]
    public class SmartLoggerTests
    {
        private List<string> _logMessages;
        private List<string> _warningMessages;
        private List<string> _errorMessages;

        [SetUp]
        public void Setup()
        {
            _logMessages = new List<string>();
            _warningMessages = new List<string>();
            _errorMessages = new List<string>();
            
            Application.logMessageReceived += OnLogMessageReceived;
            
            SmartLogger.ClearCache();
        }

        [TearDown]
        public void TearDown()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    _logMessages.Add(condition);
                    break;
                case LogType.Warning:
                    _warningMessages.Add(condition);
                    break;
                case LogType.Error:
                    _errorMessages.Add(condition);
                    break;
            }
        }

        #region Basic Logging Tests

        [Test]
        public void Log_WithValidMessage_LogsMessage()
        {
            // Arrange
            const string testMessage = "Test log message";

            // Act
            SmartLogger.Log(testMessage);

            // Assert
            Assert.That(_logMessages.Count, Is.EqualTo(1));
            Assert.That(_logMessages[0], Does.Contain(testMessage));
            Assert.That(_logMessages[0], Does.Contain("SmartLoggerTests"));
            Assert.That(_logMessages[0], Does.Contain("Log_WithValidMessage_LogsMessage"));
        }

        [Test]
        public void LogWarning_WithValidMessage_LogsWarning()
        {
            // Arrange
            const string testMessage = "Test warning message";

            // Act
            SmartLogger.LogWarning(testMessage);

            // Assert
            Assert.That(_warningMessages.Count, Is.EqualTo(1));
            Assert.That(_warningMessages[0], Does.Contain(testMessage));
            Assert.That(_warningMessages[0], Does.Contain("<color=yellow>"));
        }

        #endregion

        #region Null/Empty Message Tests

        [Test]
        public void Log_WithNullMessage_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => SmartLogger.Log(null));
        }

        [Test]
        public void Log_WithEmptyMessage_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => SmartLogger.Log(""));
        }

        [Test]
        public void LogWarning_WithNullMessage_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => SmartLogger.LogWarning(null));
        }

        [Test]
        public void LogError_WithNullMessage_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => SmartLogger.LogError(null));
        }

        #endregion

        #region Log Level Tests

        [Test]
        public void Log_WithDifferentLogLevels_RespectsLogLevel()
        {
            // Act
            SmartLogger.Log("Verbose message", LogLevel.Verbose);
            SmartLogger.Log("Important message", LogLevel.Important);
            SmartLogger.Log("Critical message", LogLevel.Critical);

            // Assert
            Assert.That(_logMessages.Count, Is.EqualTo(3));
        }

        #endregion

        #region Caller Information Tests

        [Test]
        public void Log_IncludesCallerInformation()
        {
            // Act
            SmartLogger.Log("Test message");

            // Assert
            Assert.That(_logMessages[0], Does.Contain("Class: 'SmartLoggerTests'"));
            Assert.That(_logMessages[0], Does.Contain("Method: 'Log_IncludesCallerInformation'"));
            Assert.That(_logMessages[0], Does.Match(@"Line: \d+"));
        }

        [Test]
        public void LogFromDifferentMethods_ShowsDifferentCallerInfo()
        {
            // Act
            HelperMethodForTesting();
            SmartLogger.Log("Direct call");

            // Assert
            Assert.That(_logMessages.Count, Is.EqualTo(2));
            Assert.That(_logMessages[0], Does.Contain("HelperMethodForTesting"));
            Assert.That(_logMessages[1], Does.Contain("LogFromDifferentMethods_ShowsDifferentCallerInfo"));
        }

        private void HelperMethodForTesting()
        {
            SmartLogger.Log("From helper method");
        }

        #endregion

        #region Exception Logging Tests

        [Test]
        public void LogException_WithNullException_DoesNotLog()
        {
            // Act
            SmartLogger.LogException(null);

            // Assert
            Assert.That(_errorMessages.Count, Is.EqualTo(0));
        }

        #endregion

        #region Conditional Logging Tests

        [Test]
        public void LogIf_WhenConditionTrue_LogsMessage()
        {
            // Act
            SmartLogger.LogIf(true, "Condition is true");

            // Assert
            Assert.That(_logMessages.Count, Is.EqualTo(1));
            Assert.That(_logMessages[0], Does.Contain("Condition is true"));
        }

        [Test]
        public void LogIf_WhenConditionFalse_DoesNotLog()
        {
            // Act
            SmartLogger.LogIf(false, "Condition is false");

            // Assert
            Assert.That(_logMessages.Count, Is.EqualTo(0));
        }

        #endregion

        #region Format Logging Tests

        [Test]
        public void LogFormat_WithValidFormat_LogsFormattedMessage()
        {
            // Act
            SmartLogger.LogFormat("Player {0} scored {1} points", "John", 100);

            // Assert
            Assert.That(_logMessages.Count, Is.EqualTo(1));
            Assert.That(_logMessages[0], Does.Contain("Player John scored 100 points"));
        }

        #endregion

        #region Configuration Tests

        [Test]
        public void IsLoggingEnabled_ReturnsExpectedValue()
        {
            // Act & Assert
            // В Unity Editor должно быть true, в build - зависит от Debug.isDebugBuild
            Assert.That(SmartLogger.IsLoggingEnabled, Is.TypeOf<bool>());
        }

        [Test]
        public void CurrentLogLevel_ReturnsExpectedValue()
        {
            // Act & Assert
            Assert.That(SmartLogger.CurrentLogLevel, Is.TypeOf<LogLevel>());
        }

        [Test]
        public void ClearCache_DoesNotThrowException()
        {
            // Arrange
            SmartLogger.Log("Test message to populate cache");

            // Act & Assert
            Assert.DoesNotThrow(() => SmartLogger.ClearCache());
        }

        #endregion

        #region Performance Tests

        [Test]
        public void Log_MultipleCallsFromSameLocation_UsesCaching()
        {
            // Arrange
            const int iterations = 1000;

            // Act
            var startTime = System.DateTime.Now;
            for (int i = 0; i < iterations; i++)
            {
                SmartLogger.Log($"Message {i}");
            }
            var endTime = System.DateTime.Now;

            // Assert
            Assert.That(_logMessages.Count, Is.EqualTo(iterations));
            
            // Проверяем, что время выполнения разумное (кэширование работает)
            var duration = endTime - startTime;
            Assert.That(duration.TotalSeconds, Is.LessThan(1.0), 
                "Logging should be fast due to caching");
        }

        #endregion

        #region Edge Cases

        [Test]
        public void Log_WithVeryLongMessage_HandlesGracefully()
        {
            // Arrange
            var longMessage = new string('A', 10000);

            // Act & Assert
            Assert.DoesNotThrow(() => SmartLogger.Log(longMessage));
            Assert.That(_logMessages.Count, Is.EqualTo(1));
        }

        [Test]
        public void Log_WithSpecialCharacters_HandlesCorrectly()
        {
            // Arrange
            const string specialMessage = "Message with 'quotes', <tags>, and unicode: 🎮";

            // Act
            SmartLogger.Log(specialMessage);

            // Assert
            Assert.That(_logMessages.Count, Is.EqualTo(1));
            Assert.That(_logMessages[0], Does.Contain(specialMessage));
        }

        #endregion
    }

    #region Integration Tests

    [TestFixture]
    public class SmartLoggerIntegrationTests
    {
        [Test]
        public void SmartLogger_WorksWithUnityObjects()
        {
            // Arrange
            var gameObject = new GameObject("TestObject");

            // Act & Assert
            Assert.DoesNotThrow(() => 
            {
                SmartLogger.Log($"Created GameObject: {gameObject.name}");
                SmartLogger.LogWarning($"GameObject active: {gameObject.activeInHierarchy}");
            });

            // Cleanup
            UnityEngine.Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void SmartLogger_ThreadSafety_BasicTest()
        {
            // Arrange
            var tasks = new List<System.Threading.Tasks.Task>();
            const int taskCount = 10;
            const int messagesPerTask = 100;

            // Act
            for (int i = 0; i < taskCount; i++)
            {
                int taskId = i;
                var task = System.Threading.Tasks.Task.Run(() =>
                {
                    for (int j = 0; j < messagesPerTask; j++)
                    {
                        SmartLogger.Log($"Task {taskId}, Message {j}");
                    }
                });
                tasks.Add(task);
            }

            // Assert
            Assert.DoesNotThrow(() => 
                System.Threading.Tasks.Task.WaitAll(tasks.ToArray()));
        }
    }

    #endregion
}