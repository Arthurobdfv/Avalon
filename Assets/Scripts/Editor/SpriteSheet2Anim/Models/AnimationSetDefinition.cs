using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationSetDefinition", menuName = "SpriteSheet2Anim/AnimationSetDefinition", order = 2)]
public class AnimationSetDefinition : ScriptableObject
{
    [field: SerializeField] public int SpritePerAnimation { get; set; }
    [SerializeField] public int DirectionCount => Directions.Count;
    [field: SerializeField] public List<string> Directions { get; set; }
    public int TotalFrameCount => SpritePerAnimation * DirectionCount;
}
