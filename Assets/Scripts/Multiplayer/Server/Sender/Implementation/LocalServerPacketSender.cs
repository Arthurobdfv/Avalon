using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is responsible for sending packets from the server to the local client in a local server setup.
/// It will have one implementation for each type of connection (e.g., local server, remote server).
/// For remote server it will be responsible for parsing the packet and sending it over the network.
/// </summary>
public class LocalServerPacketSender : MonoBehaviour, IAvalonPacketServerSender
{
    [SerializeField] ClientCommunicationLayerManager clientCommunicationLayerManager;
    ClientCommunicationLayerManager ClientCommunicationLayerManager
    {
        get
        {
            if (clientCommunicationLayerManager == null)
            {
                clientCommunicationLayerManager = FindFirstObjectByType<ClientCommunicationLayerManager>() ?? throw new System.NullReferenceException("No ClientCommunicationLayerManager found");
            }
            return clientCommunicationLayerManager;
        }
    }
    public static string CurrentPlayerId = "LOCAL_SERVER_CLIENT";
    public static string CurrentObserverId = string.Empty;
    public void Send(AvalonPacket packet)
    {
        ClientCommunicationLayerManager.Handle(packet);
    }

    public void Send(AvalonPacket packet, IEnumerable<string> recipientIds)
    {
        if (recipientIds.Contains(CurrentPlayerId) || recipientIds.Contains(CurrentObserverId))
        {
            ClientCommunicationLayerManager.Handle(packet);
        }
    }
}
