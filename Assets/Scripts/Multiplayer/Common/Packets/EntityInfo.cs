using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfo
{
    public EntityTypeEnum EntityType { get; set; }
    public string EntityAssetId { get; set; }
    public Vector3 Position { get; set; }
    public DirectionEnum Rotation { get; set; }
    public int Movement { get; set; }
}
