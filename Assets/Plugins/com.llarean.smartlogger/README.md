# ![unity](https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white) SmartLogger

[![Releases](https://img.shields.io/github/v/release/llarean/unity-smart-logger)](https://github.com/llarean/unity-smart-logger/releases)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](https://github.com/LLarean/unity-smart-logger/blob/master/LICENSE.txt)
![stability-stable](https://img.shields.io/badge/stability-unstable-yellow.svg)
[![Tests](https://img.shields.io/badge/Tests-NUnit-green.svg)]()
[![CodeFactor](https://www.codefactor.io/repository/github/llarean/unity-smart-logger/badge)](https://www.codefactor.io/repository/github/llarean/unity-smart-logger)
[![Maintenance](https://img.shields.io/badge/Maintained%3F-yes-green.svg)](https://GitHub.com/llarean/unity-smart-logger/graphs/commit-activity)

# **Attention! This is a template that is still being worked on!**

**Professional logging system for Unity with automatic caller information, performance optimization, and development-focused features.**

SmartLogger enhances Unity's built-in Debug.Log with rich context, configurability, and production-ready performance. Perfect for professional game development teams who need reliable debugging and monitoring tools.

## Key Features

- **Automatic Caller Information** - Class, method, and line number tracking
- **Performance Optimized** - Intelligent caching and conditional compilation
- **Rich Console Output** - Color-coded messages with timestamps
- **Fully Configurable** - ScriptableObject-based settings system
- **Thread Safe** - Safe to use from background threads
- **Memory Efficient** - Smart cache management with size limits
- **Editor Integration** - Built-in settings window and tools
- **Production Ready** - Zero performance impact when disabled

## Quick Start

### Installation

<details>
<summary><strong>📦 Via Unity Package Manager (Git URL)</strong></summary>

1. Open **Window → Package Manager**
2. Click **+ → Add package from git URL**
3. Enter: `https://github.com/llarean/unity-smart-logger.git`

</details>

<details>
<summary><strong>📦 Via Package Manager (Local)</strong></summary>

1. Download or clone this repository
2. Open **Window → Package Manager**
3. Click **+ → Add package from disk**
4. Select the package folder

</details>

### Basic Usage

```csharp
using SmartLogger;

public class PlayerController : MonoBehaviour
{
    void Start()
    {
        // Basic logging with automatic caller info
        SmartLogger.Log("Player controller started");
        
        // Different log levels
        SmartLogger.Log("Debug information", LogLevel.Verbose);
        SmartLogger.Log("Important milestone", LogLevel.Important);
        SmartLogger.Log("Critical system event", LogLevel.Critical);
        
        // Warnings and errors
        SmartLogger.LogWarning("Low health detected");
        SmartLogger.LogError("Failed to load save file");
    }
    
    void Update()
    {
        // Conditional logging
        SmartLogger.LogIf(Input.GetKeyDown(KeyCode.Space), "Space pressed!");
        
        // Formatted logging
        SmartLogger.LogFormat("Player at position: {0}", transform.position);
    }
    
    void OnTriggerEnter(Collider other)
    {
        try
        {
            ProcessCollision(other);
        }
        catch (System.Exception ex)
        {
            SmartLogger.LogException(ex, "Error processing collision");
        }
    }
}
```

### Console Output 

```
[12.34s] [PlayerController.Start:15] 'Player controller started'
[12.35s] [GameManager.LoadLevel:42] 'Loading level: Forest'
[12.36s] [SaveSystem.LoadData:128] 'Save file loaded successfully'
```

## Documentation

**[Full Documentation](Documentation~/index.md)**

## 🛠️ Configuration

SmartLogger uses a ScriptableObject for configuration. Create one via:
**Assets → Create → SmartLogger → Config**

### Configuration Options

| Setting | Description | Default |
|---------|-------------|---------|
| **Log Display Level** | Minimum importance level to display | Verbose |
| **Enable In Build** | Enable logging in production builds | false |
| **Show Timestamp** | Show elapsed time since session start | true |
| **Enable Color Coding** | Use color coding for different log types | true |
| **Max Cache Size** | Maximum cached caller info entries | 1000 |

### Editor Tools

Access SmartLogger settings via **Tools → Smart Logger → Settings**

- View cache statistics
- Clear cache manually
- Monitor performance impact
- Quick configuration access

## Examples

<details>
<summary><strong>Basic Logging</strong></summary>

```csharp
// Simple messages
SmartLogger.Log("Game started");
SmartLogger.LogWarning("Low memory warning");
SmartLogger.LogError("Critical system failure");

// With importance levels
SmartLogger.Log("Detailed debug info", LogLevel.Verbose);
SmartLogger.Log("Player reached checkpoint", LogLevel.Important);
SmartLogger.Log("Game saved", LogLevel.Critical);
```

</details>

<details>
<summary><strong>Exception Handling</strong></summary>

```csharp
try
{
    LoadPlayerData();
}
catch (FileNotFoundException ex)
{
    SmartLogger.LogException(ex, "Save file not found");
}
catch (System.Exception ex)
{
    SmartLogger.LogException(ex, "Unexpected error during load");
}
```

</details>

<details>
<summary><strong>Performance Monitoring</strong></summary>

```csharp
public class PerformanceMonitor : MonoBehaviour
{
    void Update()
    {
        float frameTime = Time.deltaTime * 1000f;
        
        SmartLogger.LogIf(
            frameTime > 16.67f, 
            $"Frame drop detected: {frameTime:F2}ms", 
            LogLevel.Important
        );
        
        // Cache management
        var (count, maxSize) = SmartLogger.GetCacheStats();
        SmartLogger.LogIf(
            count > maxSize * 0.9f,
            "Cache nearly full, consider clearing",
            LogLevel.Verbose
        );
    }
}
```

</details>

<details>
<summary><strong>Game System Integration</strong></summary>

```csharp
public class SaveSystem : MonoBehaviour
{
    public void SaveGame()
    {
        SmartLogger.Log("Starting save operation", LogLevel.Important);
        
        try
        {
            var saveData = GatherSaveData();
            WriteSaveFile(saveData);
            
            SmartLogger.Log("Game saved successfully", LogLevel.Critical);
        }
        catch (System.Exception ex)
        {
            SmartLogger.LogException(ex, "Failed to save game");
            ShowErrorToPlayer("Save failed!");
        }
    }
}
```

</details>

## Performance

SmartLogger is designed for production use with minimal performance impact:

- **Caller Information Caching**: Reflection-based caller info is cached
- **Conditional Compilation**: Logging can be completely compiled out
- **Thread-Safe Operations**: Safe for use in multithreaded scenarios
- **Memory Management**: Automatic cache cleanup prevents memory leaks

### Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/unity-smart-logger.git
   ```

2. **Open in Unity**
  - Unity 2020.3 LTS or newer required
  - Import as local package

3. **Run Tests**
  - Open **Window → General → Test Runner**
  - Run both EditMode and PlayMode tests

### Reporting Issues

Found a bug? Have a feature request? Please [open an issue](https://github.com/llarean/unity-smart-logger/issues/new/choose).

### Feature Requests

We're always looking to improve! Check our [roadmap](https://github.com/llarean/unity-smart-logger/projects/1) or suggest new features.

## Roadmap

- [ ] **Remote Logging** - Send logs to external services
- [ ] **Log Categories** - Filter logs by custom categories
- [ ] **File Output** - Save logs to files
- [ ] **Visual Log Viewer** - In-editor log visualization
- [ ] **Performance Profiler** - Built-in performance metrics
- [ ] **Unity Cloud Build** - Integration with Unity's build system

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for a detailed history of changes.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments

- **Unity Technologies** - For the amazing game engine
- **Community Contributors** - For feedback and improvements
- **Early Adopters** - For testing and bug reports

## Support

- **📧 Email**: llarean@ya.ru
- **🐛 Issues**: [GitHub Issues](https://github.com/llarean/unity-smart-logger/issues)

---

<div align="center">

**Made with ❤️ for the Unity community**

[⭐ Star us on GitHub](https://github.com/llarean/unity-smart-logger) • [📚 Read the Docs](Documentation~/index.md)

</div>

