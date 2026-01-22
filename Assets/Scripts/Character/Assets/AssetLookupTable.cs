using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetLookupTable", menuName = "Avalon/Character/Asset Lookup Table")]
public class AssetLookupTable : ScriptableObject
{
    public List<EnemyAssetData> EnemyAssetDataList;

    public List<PlayerBaseAssetData> PlayerBaseAssetDataList;
}
