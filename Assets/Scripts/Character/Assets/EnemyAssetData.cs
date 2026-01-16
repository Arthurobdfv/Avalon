using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAssetData", menuName = "Avalon/Character/Enemy Asset Data", order = 1)]
public class EnemyAssetData : AvalonAssetData<EnemyDataEnum>
{
    // TODO: Change this to lookup different types of assets (e.g., animations, sounds, etc.)
    public Sprite EnemySprite;
}
