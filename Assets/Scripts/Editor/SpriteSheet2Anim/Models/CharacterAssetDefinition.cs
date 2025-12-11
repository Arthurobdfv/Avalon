using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAssetDefinition", menuName = "SpriteSheet2Anim/CharacterAssetDefinition", order = 1)]
public class CharacterAssetDefinition : ScriptableObject
{
    [field: SerializeField] public string CharacterAssetPath { get; set; }
}
