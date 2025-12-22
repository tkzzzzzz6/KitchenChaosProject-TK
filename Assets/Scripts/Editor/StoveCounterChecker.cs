using UnityEngine;
using UnityEditor;

public class StoveCounterChecker : EditorWindow
{
    [MenuItem("Tools/Check StoveCounters")]
    public static void CheckAllStoveCounters()
    {
        StoveCounter[] stoveCounters = FindObjectsOfType<StoveCounter>();

        Debug.Log($"Found {stoveCounters.Length} StoveCounter(s) in scene:");

        int missingCount = 0;
        foreach (StoveCounter stove in stoveCounters)
        {
            // Use reflection to check private fields
            var fryingRecipeListField = typeof(StoveCounter).GetField("fryingRecipeList",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var burningRecipeListField = typeof(StoveCounter).GetField("burningRecipeList",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var fryingList = fryingRecipeListField?.GetValue(stove);
            var burningList = burningRecipeListField?.GetValue(stove);

            bool hasMissing = false;
            string message = $"[{stove.gameObject.name}]";

            if (fryingList == null)
            {
                message += " ⚠️ Missing FryingRecipeList";
                hasMissing = true;
            }
            else
            {
                message += " ✅ FryingRecipeList OK";
            }

            if (burningList == null)
            {
                message += " ⚠️ Missing BurningRecipeList";
                hasMissing = true;
            }
            else
            {
                message += " ✅ BurningRecipeList OK";
            }

            if (hasMissing)
            {
                Debug.LogError(message, stove.gameObject);
                missingCount++;
            }
            else
            {
                Debug.Log(message, stove.gameObject);
            }
        }

        if (missingCount > 0)
        {
            Debug.LogError($"⚠️ {missingCount} StoveCounter(s) have missing recipe lists!");
            Debug.Log("💡 Click on the errors above to select the objects in the scene");
        }
        else
        {
            Debug.Log("✅ All StoveCounters are properly configured!");
        }
    }

    [MenuItem("Tools/Auto-Assign StoveCounter Recipes")]
    public static void AutoAssignRecipes()
    {
        // Find the recipe assets
        string[] fryingGuids = AssetDatabase.FindAssets("FryingRecipeList t:FryingRecipeListSO");
        string[] burningGuids = AssetDatabase.FindAssets("BurningRecipeList t:FryingRecipeListSO");

        if (fryingGuids.Length == 0 || burningGuids.Length == 0)
        {
            Debug.LogError("Could not find FryingRecipeList or BurningRecipeList assets!");
            return;
        }

        FryingRecipeListSO fryingRecipeList = AssetDatabase.LoadAssetAtPath<FryingRecipeListSO>(
            AssetDatabase.GUIDToAssetPath(fryingGuids[0]));
        FryingRecipeListSO burningRecipeList = AssetDatabase.LoadAssetAtPath<FryingRecipeListSO>(
            AssetDatabase.GUIDToAssetPath(burningGuids[0]));

        StoveCounter[] stoveCounters = FindObjectsOfType<StoveCounter>();
        int fixedCount = 0;

        foreach (StoveCounter stove in stoveCounters)
        {
            SerializedObject so = new SerializedObject(stove);
            SerializedProperty fryingProp = so.FindProperty("fryingRecipeList");
            SerializedProperty burningProp = so.FindProperty("burningRecipeList");

            bool changed = false;

            if (fryingProp.objectReferenceValue == null)
            {
                fryingProp.objectReferenceValue = fryingRecipeList;
                changed = true;
            }

            if (burningProp.objectReferenceValue == null)
            {
                burningProp.objectReferenceValue = burningRecipeList;
                changed = true;
            }

            if (changed)
            {
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(stove);
                Debug.Log($"✅ Fixed {stove.gameObject.name}", stove.gameObject);
                fixedCount++;
            }
        }

        if (fixedCount > 0)
        {
            Debug.Log($"🎉 Auto-assigned recipes to {fixedCount} StoveCounter(s)!");
            Debug.Log("⚠️ Don't forget to save the scene! (Ctrl+S)");
        }
        else
        {
            Debug.Log("ℹ️ All StoveCounters already have recipes assigned");
        }
    }
}
