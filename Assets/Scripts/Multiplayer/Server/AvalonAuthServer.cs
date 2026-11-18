using System.Collections.Generic;
using UnityEngine;

public class AvalonAuthServer : MonoBehaviour
{
    private static Dictionary<string, string> AccountPlayerDict = new();

    int nextClientId = 1;

    public static bool Authorize(string playerId, string token)
    {
        return true;
    }

    private void Awake()
    {
        AccountPlayerDict = new Dictionary<string, string>();
    }

    public static string Connect(string playerId, string token)
    {
        if (AccountPlayerDict.ContainsKey(playerId))
        {
            // Player is already connected
            throw new System.Exception("Player is already connected");
        }

        // Validate the token
        if (!Authorize(playerId, token))
        {
            throw new System.Exception("Invalid token");
        }

        // Add the player to the connected players
        AccountPlayerDict[playerId] = token;
        return playerId;
    }
}
