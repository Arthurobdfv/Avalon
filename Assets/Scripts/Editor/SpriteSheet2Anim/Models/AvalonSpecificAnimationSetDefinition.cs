using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AvalonSpecificAnimationSetDefinition", menuName = "AvalonGame/SpriteSheet2Anim/Avalon Specific Animation Set Definition", order = 1)]
[Serializable]
public class AvalonSpecificAnimationSetDefinition : AnimationSetDefinition
{
    public AvalonSpecificAnimationSetDefinition()
    {
        Directions = AvalonSpriteSheet2AnimConstants.CharacterSheetDirectionOrder.ToList();
    }
}
