using System;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAssetData<T> : AvalonAssetData<T> where T : struct, System.Enum
{
    //[field: SerializeField] public AnimatorController AssetAnimatorController;
    [SerializeField] AnimationClip IdleAnimation;
    [SerializeField] AnimationClip WalkAnimation;
    [SerializeField] AnimationClip RunAnimation;
    [SerializeField] AnimationClip AttackAnimation;

    [field: SerializeField]
    PlayerCharacterAnimation[] PlayerCharacterAnimations = new[]
    {
        new PlayerCharacterAnimation { AnimationType = AvalonAnimationType.IDLE },
        new PlayerCharacterAnimation { AnimationType = AvalonAnimationType.WALK },
        new PlayerCharacterAnimation { AnimationType = AvalonAnimationType.RUN },
        new PlayerCharacterAnimation { AnimationType = AvalonAnimationType.ATTACK },

    };
}


[Serializable]
public class PlayerCharacterAnimation
{
    public AvalonAnimationType AnimationType;
    public AnimationClip AnimationClip;
}