using UnityEngine;

[CreateAssetMenu(fileName = "SpriteSheetDefinition", menuName = "SpriteSheet2Anim/SpriteSheetDefinition", order = 0)]
public class SpriteSheetDefinition : ScriptableObject
{
    [field: SerializeField] public int SpriteWidth;
    [field: SerializeField] public int SpriteHeight;
}
