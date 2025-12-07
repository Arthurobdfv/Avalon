using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationSetDefinition", menuName = "SpriteSheet2Anim/AnimationSetDefinition", order = 2)]
public class AnimationSetDefinition : ScriptableObject
{
    [field: SerializeField] public string AnimationName { get; set; }
    [field: SerializeField] public int SpritePerAnimation { get; set; }
    [field: SerializeField] public int DirectionCount { get; set; }
    [field: SerializeField] public List<string> Directions { get; set; }
    [field: SerializeField] public List<SpriteSequenceDefinition> Sequences { get; set; }
}
