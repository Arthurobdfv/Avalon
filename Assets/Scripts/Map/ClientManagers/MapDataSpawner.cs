using UnityEngine;

public class MapDataSpawner : MonoBehaviour
{
    private ClientCommunicationLayerManager _clientCommunicationLayerManager;
    private ClientCommunicationLayerManager ClientCommunicationLayerManager
    {
        get
        {
            if (_clientCommunicationLayerManager == null)
            {
                _clientCommunicationLayerManager = FindObjectOfType<ClientCommunicationLayerManager>();
            }
            return _clientCommunicationLayerManager;
        }
    }
    private void OnEnable()
    {
        ClientCommunicationLayerManager.Handler.RegisterClientHandler<MapDataSpawnPacket>(SpawnMapData);
    }

    private void OnDisable()
    {
        ClientCommunicationLayerManager.Handler.UnregisterClientHandler<MapDataSpawnPacket>();
    }

    private void SpawnMapData(MapDataSpawnPacket packet)
    {
        MapData mapData = packet.MapData;
        //MapDataManager.Instance.SetMapData(mapData);
    }
}
