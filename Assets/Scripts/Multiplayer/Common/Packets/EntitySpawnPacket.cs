using System.Collections.Generic;

public class EntitySpawnPacket : AvalonPacket
{
    public string MapId { get; set; }
    public Dictionary<string, EntityInfo> Entities { get; set; }
}
