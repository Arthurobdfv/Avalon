using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpriteSheet2AnimDefinition))]
[Serializable]
public class SpriteSheet2AnimCustomEditor : Editor
{
    public List<string> CurrentFilesToParse = new List<string>();

    public static Dictionary<string, SpriteSheetDefinition> SpriteSheetDefinitionsLookup = new();

    string basePath = Application.dataPath;
    string FullPath => Path.GetFullPath(Path.Combine(basePath, Target.CharacterAssetDefinition?.CharacterAssetPath));
    SpriteSheet2AnimDefinition Target => target as SpriteSheet2AnimDefinition;
    void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        if (Target == null)
        {
            throw new System.Exception("The Target is null!");
        }

        base.OnInspectorGUI();
        var imagesToParse = new List<string>();

        if (GUILayout.Button("Find Assets"))
        {
            imagesToParse = FindSourceImageFiles(FullPath).ToList();
        }
        if (GUILayout.Button("Parse Sprites"))
        {
            if (imagesToParse.Count == 0)
            {
                imagesToParse = FindSourceImageFiles(FullPath).ToList();
            }
            SpriteSheetDefinitionsLookup = imagesToParse.Select(x => new { filePath = Path.GetFileName(x), Target.SpriteSheet }).ToDictionary(x => x.filePath, y => y.SpriteSheet);

            foreach (var item in SpriteSheetDefinitionsLookup)
            {
                ParseImageAt($"Assets/{Target.CharacterAssetDefinition?.CharacterAssetPath}/{Path.GetFileName(item.Key)}");
            }
            SpriteSheetDefinitionsLookup.Clear();
        }
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }

    private static string[] FindSourceImageFiles(string path)
    {
        if (Directory.Exists(path))
        {
            var imgFiles = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);
            Debug.Log($"Found {imgFiles.Length} files:{string.Join($"{Environment.NewLine} * ", imgFiles)}");
            return imgFiles;
        }
        else
        {
            Debug.LogError($"Path not found: {path}");
            return Array.Empty<string>();
        }
    }

    private void ParseImageAt(string path)
    {
        AssetDatabase.ImportAsset(path, ImportAssetOptions.Default);
    }
}
