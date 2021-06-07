using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameFramework.Core;

public class GameplayModulesDebugWindow : EditorWindow
{
    class ViewEntry
    {
        public GameplayModule module;
        public bool showDebug;
        public Vector2 scrollPos;
    }

    Dictionary<GameplayModule, ViewEntry> entries = new Dictionary<GameplayModule, ViewEntry>();

    // Add menu named "My Window" to the Window menu
    [MenuItem("Game/Gameplay Modules")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        GameplayModulesDebugWindow window = (GameplayModulesDebugWindow)EditorWindow.GetWindow(typeof(GameplayModulesDebugWindow));
        window.Show();
    }

    void OnGUI()
    {
        if(!Application.isPlaying)
        {
            return;
        }
        List<GameplayModule> modules = IoCContainer.Get<GameplayService>().GameplayModules;
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < modules.Count; i++)
        {
            DrawModuleInfo(modules[i]);
        }
        EditorGUILayout.EndVertical();
    }

    void DrawModuleInfo(GameplayModule module)
    {
        ViewEntry entry;
        if (entries.TryGetValue(module, out ViewEntry e))
        {
            entry = e;
        }
        else
        {
            entry = new ViewEntry();
            entry.module = module;
            entries.Add(module, entry);
        }

        EditorGUILayout.BeginHorizontal();
        string moduleLabel = module.GetType().Name;
        if(!module.Enabled)
        {
            moduleLabel += " (Disabled)";
        }
        entry.showDebug = EditorGUILayout.Foldout(entry.showDebug, moduleLabel);
        EditorGUILayout.EndHorizontal();
        if (entry.showDebug)
        {
            GUILayout.BeginVertical("info", "window", GUILayout.MaxHeight(100f), GUILayout.ExpandHeight(true));
            GUILayout.BeginScrollView(entry.scrollPos);
            module.Enabled = EditorGUILayout.ToggleLeft("Enabled", module.Enabled);
            module.DrawDebugInfo();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}
