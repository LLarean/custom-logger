# SmartLogger Documentation

**Version:** 1.2.0  
**Unity Compatibility:** 2020.3 LTS and newer  
**Package Name:** com.yourcompany.smartlogger

## Overview

SmartLogger is an advanced logging system for Unity that provides automatic caller information, performance optimization, and development-focused features. It enhances Unity's built-in Debug.Log with additional context, configurability, and better performance for production builds.

## Key Features

- **Automatic Caller Information** - Automatically captures class, method, and line number
- **Performance Optimized** - Cached caller information and conditional compilation
- **Rich Console Output** - Color-coded messages with timestamps
- **Configurable** - ScriptableObject-based configuration system
- **Thread Safe** - Safe to use from multiple threads
- **Memory Efficient** - Intelligent cache management
- **Editor Tools** - Built-in settings window and statistics

## Quick Start

### Installation

#### Via Unity Package Manager (Git URL)
1. Open **Window → Package Manager**
2. Click **+ → Add package from git URL**
3. Enter: `https://github.com/yourusername/smart-logger-unity.git`

#### Via Package Manager (Local)
1. Download or clone the repository
2. Open **Window → Package Manager**
3. Click **+ → Add package from disk**
4. Select the package folder

### Basic Usage

```csharp
using Utilities.Logging;

public class PlayerController : MonoBehaviour
{
    void Start()
    {
        // Initialize logger (optional, but recommended)
        SmartLogger.Initialize();
        
        // Basic logging
        SmartLogger.Log("Player controller started");
        SmartLogger.Log("Loading player data", LogLevel.Important);
        SmartLogger.Log("Critical system initialized", LogLevel.Critical);
    }
    
    void Update()
    {
        // Conditional logging
        SmartLogger.LogIf(Input.GetKeyDown(KeyCode.Space), "Space key pressed");
        
        // Warning and error logging
        if (health <= 0)
        {
            SmartLogger.LogWarning("Player health is critical!");
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        try
        {
            // Your collision logic
        }
        catch (System.Exception ex)
        {
            SmartLogger.LogException(ex, "Error during collision handling");
        }
    }
}
```

## API Reference

### Core Logging Methods

#### Log
```csharp
SmartLogger.Log(string messageText, LogLevel importanceLevel = LogLevel.Verbose)
```
Outputs a regular message to the console with automatic caller information.

**Parameters:**
- `messageText` - The message to log
- `importanceLevel` - The importance level (Verbose, Important, Critical)

**Example:**
```csharp
SmartLogger.Log("Game started successfully");
SmartLogger.Log("Loading level data", LogLevel.Important);
```

#### LogWarning
```csharp
SmartLogger.LogWarning(string messageText)
```
Outputs a warning message with yellow color coding.

**Example:**
```csharp
SmartLogger.LogWarning("Low memory detected");
```

#### LogError
```csharp
SmartLogger.LogError(string messageText)
```
Outputs an error message with red color coding.

**Example:**
```csharp
SmartLogger.LogError("Failed to load save file");
```

#### LogException
```csharp
SmartLogger.LogException(Exception exception, string additionalMessage = "")
```
Logs an exception with full stack trace and optional additional context.

**Example:**
```csharp
try
{
    // Risky operation
}
catch (Exception ex)
{
    SmartLogger.LogException(ex, "Failed to process user input");
}
```

#### LogIf
```csharp
SmartLogger.LogIf(bool condition, string messageText, LogLevel importanceLevel = LogLevel.Verbose)
```
Conditional logging - only logs if the condition is true.

**Example:**
```csharp
SmartLogger.LogIf(debugMode, "Debug information", LogLevel.Verbose);
```

#### LogFormat
```csharp
SmartLogger.LogFormat(string format, params object[] args)
```
Formatted string logging similar to string.Format.

**Example:**
```csharp
SmartLogger.LogFormat("Player {0} scored {1} points", playerName, score);
```

### Utility Methods

#### Initialize
```csharp
SmartLogger.Initialize()
```
Initializes the logger and sets the session start time for timestamps.

#### ClearCache
```csharp
SmartLogger.ClearCache()
```
Clears the caller information cache to free memory.

#### GetCacheStats
```csharp
(int count, int maxSize) stats = SmartLogger.GetCacheStats()
```
Returns current cache statistics for debugging purposes.

### Properties

#### IsLoggingEnabled
```csharp
bool isEnabled = SmartLogger.IsLoggingEnabled;
```
Indicates whether logging is currently enabled.

#### CurrentLogLevel
```csharp
LogLevel currentLevel = SmartLogger.CurrentLogLevel;
```
Returns the current minimum log level being displayed.

## Configuration

### LoggerConfig ScriptableObject

SmartLogger uses a ScriptableObject for configuration. Create one via:
**Assets → Create → SmartLogger → Config**

#### Configuration Options

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| **Log Display Level** | LogLevel | Verbose | Minimum importance level to display |
| **Enable In Build** | bool | false | Enable logging in production builds |
| **Show Timestamp** | bool | true | Show elapsed time since session start |
| **Enable Color Coding** | bool | true | Use color coding for different log types |
| **Max Cache Size** | int | 1000 | Maximum number of cached caller info entries |

### Runtime Configuration

```csharp
// Access configuration at runtime
var config = LoggerConfig.Instance;
bool timestampsEnabled = config.ShowTimestamp;
LogLevel minLevel = config.LogDisplayLevel;
```

## Log Levels

SmartLogger supports three log levels:

| Level | Value | Description | Use Case |
|-------|-------|-------------|----------|
| **Verbose** | 0 | Detailed information | Debug traces, detailed flow |
| **Important** | 1 | Significant events | State changes, milestones |
| **Critical** | 2 | Essential information | Errors, warnings, critical events |

### Usage Examples

```csharp
// Verbose - for detailed debugging
SmartLogger.Log("Entering update loop", LogLevel.Verbose);

// Important - for significant events
SmartLogger.Log("Level completed", LogLevel.Important);

// Critical - for essential information
SmartLogger.Log("Save system initialized", LogLevel.Critical);
```

## Editor Tools

### SmartLogger Settings Window

Access via **Tools → Smart Logger → Settings**

Features:
- View current cache statistics
- Clear cache manually
- Quick access to configuration
- Real-time logging status

### Console Output Format

```
[12.34s] [PlayerController.Start:25] 'Player initialized successfully'
[12.35s] [GameManager.LoadLevel:142] 'Loading level: Forest'
```

Format: `[Timestamp] [Class.Method:Line] 'Message'`

## Performance Considerations

### Caching System
- Caller information is cached to avoid reflection overhead
- Cache automatically clears when size limit is reached
- Thread-safe cache operations

### Production Builds
- Logging can be completely disabled in builds
- Conditional compilation removes logging code
- Minimal performance impact when disabled

### Memory Management
```csharp
// Manually clear cache if needed
SmartLogger.ClearCache();

// Check cache statistics
var (count, maxSize) = SmartLogger.GetCacheStats();
if (count > maxSize * 0.8f)
{
    SmartLogger.ClearCache();
}
```

## Best Practices

### 1. Initialize Early
```csharp
// In your main game script
void Awake()
{
    SmartLogger.Initialize();
}
```

### 2. Use Appropriate Log Levels
```csharp
// Good - use appropriate levels
SmartLogger.Log("Minor state change", LogLevel.Verbose);
SmartLogger.Log("Player died", LogLevel.Important);
SmartLogger.Log("Game crashed", LogLevel.Critical);

// Avoid - everything as Verbose
SmartLogger.Log("Everything"); // Uses Verbose by default
```

### 3. Conditional Logging for Performance
```csharp
// Good - use LogIf for expensive operations
SmartLogger.LogIf(debugMode && expensiveCheck(), "Debug info");

// Avoid - always computing expensive values
SmartLogger.Log($"Complex calculation: {ExpensiveMethod()}");
```

### 4. Exception Handling
```csharp
// Good - provide context
try
{
    LoadPlayerData();
}
catch (Exception ex)
{
    SmartLogger.LogException(ex, "Failed to load player data from file");
}
```

### 5. Cache Management
```csharp
// Clear cache periodically in long-running applications
void Update()
{
    if (Time.time % 300f < Time.deltaTime) // Every 5 minutes
    {
        var (count, maxSize) = SmartLogger.GetCacheStats();
        if (count > maxSize * 0.9f)
        {
            SmartLogger.ClearCache();
        }
    }
}
```

## Troubleshooting

### Common Issues

#### 1. Logs Not Appearing
**Problem:** Logs are not showing in the console.
**Solution:** 
- Check if `IsLoggingEnabled` returns true
- Verify log level configuration
- Ensure development build is enabled for builds

#### 2. Performance Issues
**Problem:** Logging is impacting performance.
**Solution:**
- Use appropriate log levels
- Clear cache regularly in long-running apps
- Disable logging in production builds

#### 3. Thread Safety
**Problem:** Crashes when logging from multiple threads.
**Solution:** SmartLogger is thread-safe, but ensure Unity objects aren't accessed from background threads.

### Debug Information

```csharp
// Check logger status
Debug.Log($"Logging Enabled: {SmartLogger.IsLoggingEnabled}");
Debug.Log($"Current Log Level: {SmartLogger.CurrentLogLevel}");

var (count, maxSize) = SmartLogger.GetCacheStats();
Debug.Log($"Cache: {count}/{maxSize}");
```

## Integration Examples

### With Unity Analytics
```csharp
public void TrackEvent(string eventName, Dictionary<string, object> parameters)
{
    SmartLogger.Log($"Tracking event: {eventName}", LogLevel.Important);
    
    try
    {
        // Analytics.CustomEvent(eventName, parameters);
        SmartLogger.Log("Event tracked successfully");
    }
    catch (Exception ex)
    {
        SmartLogger.LogException(ex, $"Failed to track event: {eventName}");
    }
}
```

### With Save System
```csharp
public void SaveGame()
{
    SmartLogger.Log("Starting game save", LogLevel.Important);
    
    try
    {
        var saveData = CreateSaveData();
        WriteSaveFile(saveData);
        
        SmartLogger.Log("Game saved successfully", LogLevel.Important);
    }
    catch (Exception ex)
    {
        SmartLogger.LogException(ex, "Critical: Failed to save game");
        SmartLogger.LogError("Save operation failed - data may be lost!");
    }
}
```

## Migration Guide

### From Unity Debug.Log
```csharp
// Old
Debug.Log("Message");
Debug.LogWarning("Warning");
Debug.LogError("Error");

// New
SmartLogger.Log("Message");
SmartLogger.LogWarning("Warning");
SmartLogger.LogError("Error");
```

### From Custom Logger
```csharp
// Old custom logger
MyLogger.Info("Message");
MyLogger.Warn("Warning");
MyLogger.Error("Error");

// New SmartLogger
SmartLogger.Log("Message", LogLevel.Verbose);
SmartLogger.LogWarning("Warning");
SmartLogger.LogError("Error");
```

## Changelog

See [CHANGELOG.md](../CHANGELOG.md) for version history and breaking changes.

## Support

- **Issues:** [GitHub Issues](https://github.com/yourusername/smart-logger-unity/issues)
- **Discussions:** [GitHub Discussions](https://github.com/yourusername/smart-logger-unity/discussions)
- **Email:** support@yourcompany.com

## License

This package is licensed under the MIT License. See [LICENSE.md](../LICENSE.md) for details.