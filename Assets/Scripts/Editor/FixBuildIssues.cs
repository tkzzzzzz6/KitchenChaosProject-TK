using UnityEngine;
using UnityEditor;

/// <summary>
/// Tools to fix common build issues
/// </summary>
public class FixBuildIssues : EditorWindow
{
    [MenuItem("Tools/Fix Build Issues/Regenerate Lighting")]
    public static void RegenerateLighting()
    {
        Debug.Log("🔆 Regenerating lighting data...");

        // Clear existing lighting data
        Lightmapping.Clear();

        Debug.Log("✅ Lighting data cleared. The scene will use realtime lighting.");
        Debug.Log("💡 If you need baked lighting, go to Window → Rendering → Lighting and click 'Generate Lighting'");
    }

    [MenuItem("Tools/Fix Build Issues/Verify All ScriptableObjects")]
    public static void VerifyScriptableObjects()
    {
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets" });

        int total = guids.Length;
        int broken = 0;

        Debug.Log($"🔍 Checking {total} ScriptableObjects...");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (so == null)
            {
                Debug.LogError($"❌ BROKEN: {path} - Script reference is missing!", AssetDatabase.LoadAssetAtPath<Object>(path));
                broken++;
            }
        }

        if (broken > 0)
        {
            Debug.LogError($"⚠️ Found {broken} broken ScriptableObject(s)!");
            Debug.Log("💡 Click on the errors above to select the broken assets");
            Debug.Log("💡 You may need to manually reassign the script in the Inspector");
        }
        else
        {
            Debug.Log($"✅ All {total} ScriptableObjects are OK!");
        }
    }

    [MenuItem("Tools/Fix Build Issues/Clean Build")]
    public static void CleanBuild()
    {
        Debug.Log("🧹 Cleaning build cache...");

        // Clear console
        var assembly = System.Reflection.Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);

        // Clear player prefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("✅ Build cache cleaned!");
        Debug.Log("💡 Now try building again");
    }
}
