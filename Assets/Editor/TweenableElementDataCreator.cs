using UnityEngine;
using UnityEditor;
using System.Linq;

public class TweenableElementDataCreator
{
    [MenuItem("Tools/Create TweenableElementData Instances")]
    public static void CreateTweenableElementDataInstances()
    {
        // Find all TweenableElement instances in the scene
        TweenableElement[] elements = GameObject.FindObjectsOfType<TweenableElement>(true);

        // Iterate over each TweenableElement
        foreach (TweenableElement element in elements)
        {
            // Check if a TweenableElementData already exists for this element
            bool dataExists = AssetDatabase.FindAssets("t:TweenableElementData")
                                            .Select(guid => AssetDatabase.LoadAssetAtPath<TweenableElementData>(AssetDatabase.GUIDToAssetPath(guid)))
                                            .Any(data => data.name == element.name);

            if (dataExists)
            {
                Debug.Log($"TweenableElementData already exists for {element.gameObject.name}, skipping...");
                continue;
            }

            // Create a new TweenableElementData instance
            TweenableElementData data = ScriptableObject.CreateInstance<TweenableElementData>();

            // Save the ScriptableObject asset
            string assetPath = $"Assets/Animations/Tweenable Elements/{element.gameObject.name}.asset";
            AssetDatabase.CreateAsset(data, assetPath);

            // Ensure the data is properly serialized
            EditorUtility.SetDirty(data);
        }

        // Save all assets to ensure all changes are written to disk
        AssetDatabase.SaveAssets();

        // Refresh the AssetDatabase to ensure the assets are shown in the Project window
        AssetDatabase.Refresh();

        Debug.Log("TweenableElementData instances created for each TweenableElement in the scene.");
    }
}