using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MultiplayerContextLoader;
public class CommunicationLayerManager : MonoBehaviour
{
    public void Awake()
    {
        if (AvalonGameContext.Instance == null || AvalonGameContext.Instance.CurrentGameType == AvalonGameContext.GAMETYPE.NONE)
        {
            throw new System.Exception("AvalonGameContext instance not found. Make sure AvalonGameContext is initialized before CommunicationLayerManager.");
        }
        if (RequireClientInstance(AvalonGameContext.Instance.CurrentGameType))
        {
            BuildClientGameObject();
        }
        if (RequireServerInstance(AvalonGameContext.Instance.CurrentGameType))
        {
            BuildServerGameObject();
        }
    }

    private void BuildServerGameObject()
    {
        GameObject serverInstance = new GameObject("Server");
        if (RequireNetworkSetup(AvalonGameContext.Instance.CurrentGameType))
        {
            AddServerNetworkCommunicationLayer(serverInstance);
        }
        else
        {
            AddServerLocalCommunicationLayer(serverInstance);
        }
        AddServerCommunicationManager(serverInstance);
        //Instantiate(serverInstance, transform);
    }

    private void BuildClientGameObject()
    {
        GameObject clientInstance = new GameObject("Client");
        if (RequireNetworkSetup(AvalonGameContext.Instance.CurrentGameType))
        {
            AddClientNetworkCommunicationLayer(clientInstance);
        }
        else
        {
            AddClientLocalCommunicatonLayer(clientInstance);
        }
        AddClientCommunicationManager(clientInstance);
        //Instantiate(clientInstance, transform);
    }

    public void AddServerNetworkCommunicationLayer(GameObject serverInstance)
    {
        serverInstance.AddComponent<NetworkServer>();
        serverInstance.AddComponent<NetworkServerPacketReceiver>();
        serverInstance.AddComponent<NetworkServerPacketSender>();
    }

    public void AddClientNetworkCommunicationLayer(GameObject clientInstance)
    {
        clientInstance.AddComponent<NetworkClient>();
        clientInstance.AddComponent<MultiplayerClientPacketReceiver>();
        clientInstance.AddComponent<MultiplayerClientPacketSender>();
    }

    public void AddClientLocalCommunicatonLayer(GameObject clientInstance)
    {
        clientInstance.AddComponent<LocalClientPacketSender>();
    }

    public void AddServerLocalCommunicationLayer(GameObject serverInstance)
    {
        serverInstance.AddComponent<LocalServerPacketReceiver>();
        serverInstance.AddComponent<LocalServerPacketSender>();
    }
    public void AddServerCommunicationManager(GameObject serverInstance)
    {
        serverInstance.AddComponent<ServerCommunicationLayerManager>();
    }

    public void AddClientCommunicationManager(GameObject clientInstance)
    {
        clientInstance.AddComponent<ClientCommunicationLayerManager>();
    }
}
