using UnityEngine;

public class EnemyEntityUI : EntityUI
{
    [field:SerializeField] private SpriteRenderer EnemySpriteRenderer { get; set; } 
    public override void SpawnUISpecific(string assetId)
    {
        GlobalAssetLookupProvider.EnemyAssetDataDictionary.TryGetValue(assetId, out var assetData);
        if (assetData != null)
        {
            EnemySpriteRenderer.sprite = assetData.EnemySprite;
        }
    }
}
