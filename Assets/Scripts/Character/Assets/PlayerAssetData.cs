using System;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAssetData<T> : AvalonAssetData<T> where T : struct, System.Enum
{
    [field: SerializeField] public AnimatorController AssetAnimatorController;
}
