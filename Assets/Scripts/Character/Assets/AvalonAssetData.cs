using System;
using UnityEngine;

public class AvalonAssetData<T> : AssetData where T : Enum
{
    [field: SerializeField] T AssetIdEnum { get; set; }
    public string GetAssetId() => AssetIdEnum.ToString();
}
