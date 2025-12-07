using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEditorInternal;
using UnityEngine;

public class SpriteSheetParser : AssetPostprocessor
{
    void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
    {
        var key = Path.GetFileName(assetPath);
        if (!SpriteSheet2AnimCustomEditor.SpriteSheetDefinitionsLookup.ContainsKey(key))
        {
            return;
        }
        Debug.Log("Sprites: " + sprites.Length);

    }

    private void OnPostprocessTexture(Texture2D texture)
    {
        var key = Path.GetFileName(assetPath);
        if (!SpriteSheet2AnimCustomEditor.SpriteSheetDefinitionsLookup.ContainsKey(key))
        {
            return;
        }
        var settings = SpriteSheet2AnimCustomEditor.SpriteSheetDefinitionsLookup[key];
        var rects = InternalSpriteUtility.GenerateGridSpriteRectangles(texture, Vector2.zero, new Vector2(settings.SpriteWidth, settings.SpriteHeight), Vector2.zero).OrderByDescending(x => x.y).ThenBy(x => x.x).ToList();

        var fileName = Path.GetFileNameWithoutExtension(assetPath);
        var spriteRects = rects.Select((x,i) => new SpriteRect()
        {
            rect = x,
            pivot = new Vector2(0.5f, 0.5f),

            name = $"{fileName}_{i}"
        }).ToList();
        var spriteMetadata = new List<SpriteMetaData>();
        var filename = Path.GetFileNameWithoutExtension(assetPath);

        var importer = assetImporter as TextureImporter;
        var factory = new SpriteDataProviderFactories();
        factory.Init();
        var dataProvider = factory.GetSpriteEditorDataProviderFromObject(importer);
        dataProvider.InitSpriteEditorDataProvider();

        dataProvider.SetSpriteRects(spriteRects.ToArray());
        dataProvider.Apply();
    }


    void OnPreprocessTexture()
    {
        Debug.Log("OnPreprocessTexture: " + assetPath);
        var fileName = Path.GetFileName(assetPath);
        SpriteSheet2AnimCustomEditor.SpriteSheetDefinitionsLookup.TryGetValue(fileName, out var settings);
        if (settings == null)
        {
            Debug.Log($"No settings found for {fileName}");
            return;
        }
        var textureImporter = (TextureImporter)assetImporter;
        textureImporter.spriteImportMode = SpriteImportMode.Multiple;
        textureImporter.filterMode = FilterMode.Point;
        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.spritePixelsPerUnit = 50;
        textureImporter.mipmapEnabled = false;
    }
}
