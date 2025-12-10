using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteSheet2AnimDefinition", menuName = "SpriteSheet2Anim/SpriteSheet2AnimDefinition", order = 1)]
public class SpriteSheet2AnimDefinition : ScriptableObject
{
    [field: SerializeField] public string CharacterName { get; set; }
    [field: SerializeField] public List<SpriteSheetAnimationDescriptor> SpriteSheetAnimationDescriptors { get; set; }
    [field: SerializeField] public SpriteSheetDefinition SpriteSheet { get; set; }
    [field: SerializeField] public CharacterAssetDefinition CharacterAssetDefinition { get; set; }
}
