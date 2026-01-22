using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAssetLookupProvider : MonoBehaviour
{
    [field: SerializeField] public AssetLookupTable AssetLookupTable { get; private set; }
    public static Dictionary<string, EnemyAssetData> EnemyAssetDataDictionary = new Dictionary<string, EnemyAssetData>();
    public static Dictionary<string, PlayerBaseAssetData> PlayerBaseAssetDataDictionary = new Dictionary<string, PlayerBaseAssetData>();
    // Start is called before the first frame update
    void Start()
    {
        InitializeEnemyAssetDataDictionary();
        InitializePlayerBaseAssetDataDictionary();
    }

    private void InitializeEnemyAssetDataDictionary()
    {
        if (AssetLookupTable == null) return;

        foreach (var enemyAssetData in AssetLookupTable.EnemyAssetDataList)
        {
            var assetId = enemyAssetData.GetAssetId();
            if (!EnemyAssetDataDictionary.ContainsKey(assetId))
            {
                EnemyAssetDataDictionary[assetId] = enemyAssetData;
            }
        }
    }

    private void InitializePlayerBaseAssetDataDictionary()
    {
        if (AssetLookupTable == null) return;
        foreach (var playerBaseAssetData in AssetLookupTable.PlayerBaseAssetDataList)
        {
            var assetId = playerBaseAssetData.GetAssetId();
            if (!PlayerBaseAssetDataDictionary.ContainsKey(assetId))
            {
                PlayerBaseAssetDataDictionary[assetId] = playerBaseAssetData;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
