using UnityEngine;

[CreateAssetMenu(fileName = "new WarpPoint", menuName = "MapData/WarpPoint")]
public class WarpPoint : ScriptableObject
{
    [field: SerializeField] public string warpID { get; private set; }
    [field: SerializeField] public Vector2 warpPosition { get; private set; }
    [field: SerializeField] public MapData destinationMap { get; private set; }
}