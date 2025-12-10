using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteSheetAnimationDescriptor", menuName = "SpriteSheet2Anim/Sprite Sheet Animation Descriptor", order = 3)]
[Serializable]
public class SpriteSheetAnimationDescriptor : ScriptableObject
{
    [field: SerializeField] public uint StartIndex { get; set; }
    [field: SerializeField] public string AnimationName { get; set; }
    [SerializeField] public int FrameCount => AnimationSetDefinition.TotalFrameCount;

    [field: SerializeField] public AnimationSetDefinition AnimationSetDefinition { get; set; }
}
