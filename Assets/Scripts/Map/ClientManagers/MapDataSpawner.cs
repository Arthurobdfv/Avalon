using UnityEngine;

public class MapDataSpawner : MonoBehaviour
{
    private ClientPacketHandler _clientPacketHandler;
    private void OnEnable()
    {
        if(_clientPacketHandler == null)
        {
            _clientPacketHandler = FindObjectOfType<ClientPacketHandler>();
        }
        _clientPacketHandler.RegisterClientHandler<MapDataSpawnPacket>(SpawnMapData);
    }

    private void OnDisable()
    {
        _clientPacketHandler.UnregisterClientHandler<MapDataSpawnPacket>();
    }

    private void SpawnMapData(MapDataSpawnPacket packet)
    {
        MapData mapData = packet.MapData;
        //MapDataManager.Instance.SetMapData(mapData);
    }
}
