using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteSequenceDefinition", menuName = "SpriteSheet2Anim/SpriteSequenceDefinition", order = 3)]
public class SpriteSequenceDefinition : ScriptableObject
{
    [field: SerializeField] public int SpriteCount { get; set; }
}
