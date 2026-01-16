using UnityEngine;

public class EntityUI : MonoBehaviour
{
    [field: SerializeField] public string EntityAssetId { get; private set; }
    public void SpawnUI(string assetId)
    {
        EntityAssetId = assetId;
        if (string.IsNullOrWhiteSpace(EntityAssetId))
        {
            Debug.LogWarning("EntityAssetId is not set. Trying to get AssetId from parent...");
        }
        GlobalAssetLookupProvider.EnemyAssetDataDictionary.TryGetValue(EntityAssetId, out var assetData);
        if (assetData != null)
        {
            SpawnUISpecific(EntityAssetId);
        }
        else
        {
            Debug.LogError($"No asset data found for EntityAssetId: {EntityAssetId}");
        }
    }
    //TODO Improve this, currently a hack for future inheritance implementation
    public virtual void SpawnUISpecific(string assetId)
    {

    }
}
