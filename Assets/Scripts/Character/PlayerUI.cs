using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : EntityUI
{
    [SerializeField] Character _character;
    [SerializeField] PlayerBaseAssetEnum _currentPlayerBaseAsset = PlayerBaseAssetEnum.PLAYER_ADVENTURER_BASE_01;
    [SerializeField] Animator _animator;

    public void UpdatePlayerBaseAsset(PlayerBaseAssetEnum newAsset)
    {
        if(_currentPlayerBaseAsset != newAsset && newAsset != PlayerBaseAssetEnum.NONE)
        {
            _currentPlayerBaseAsset = newAsset;
            _animator.runtimeAnimatorController = (RuntimeAnimatorController)GlobalAssetLookupProvider.PlayerBaseAssetDataDictionary[newAsset.ToString()].AssetAnimatorController;
            //var assetData = GlobalAssetLookupProvider.PlayerBaseAssetDataDictionary[newAsset.ToString()];
            //_character.ApplyPlayerBaseAssetData(assetData);
        }
    }

    protected override void SpawnUISpecific()
    {
        UpdatePlayerBaseAsset(_currentPlayerBaseAsset);
    }
}
