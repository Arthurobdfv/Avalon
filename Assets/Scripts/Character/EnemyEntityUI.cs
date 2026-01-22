using UnityEngine;

public class EnemyEntityUI : EntityUI
{
    [field: SerializeField] private SpriteRenderer EnemySpriteRenderer { get; set; }
    [field: SerializeField] public string EntityAssetId { get; private set; }

    protected override void SpawnUISpecific()
    {
        if (string.IsNullOrWhiteSpace(EntityAssetId))
        {
            Debug.LogWarning("EntityAssetId is not set. Trying to get AssetId from parent...");
        }
        GlobalAssetLookupProvider.EnemyAssetDataDictionary.TryGetValue(EntityAssetId, out var assetData);
        if (assetData != null)
        {
            EnemySpriteRenderer.sprite = assetData.EnemySprite;
        }
    }

    public void SetEntityAssetId(string assetId)
    {
        EntityAssetId = assetId;
        SpawnUISpecific();
    }
}
