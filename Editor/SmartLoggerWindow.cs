using UnityEngine;
using UnityEditor;

namespace SmartLogger.Editor
{
    public class SmartLoggerWindow : EditorWindow
    {
        [MenuItem("Tools/Smart Logger/Settings")]
        public static void ShowWindow()
        {
            GetWindow<SmartLoggerWindow>("Smart Logger Settings");
        }

        private void OnGUI()
        {
            GUILayout.Label("Smart Logger Settings", EditorStyles.boldLabel);
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Cache Statistics:");
            var stats = SmartLogger.GetCacheStats();
            EditorGUILayout.LabelField($"Cache Size: {stats.count}/{stats.maxSize}");
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Clear Cache"))
            {
                SmartLogger.ClearCache();
            }
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Configuration:");
            EditorGUILayout.HelpBox("Edit SmartLoggerConfig asset in Resources folder", MessageType.Info);
        }
    }
}