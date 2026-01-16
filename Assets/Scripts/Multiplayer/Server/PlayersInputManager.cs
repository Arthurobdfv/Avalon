using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerInputHandler;

public class PlayersInputManager : MonoBehaviour
{
    Dictionary<string, PlayerInputState> _pendingPlayerInputs;

    private void OnEnable()
    {
        ServerPacketHandler.RegisterServerHandler<PlayerInputState>(HandlePlayerInputState);
    }

    private void HandlePlayerInputState(PlayerInputState state)
    {
        throw new NotImplementedException();
    }

    private void OnDisable()
    {
        //ServerPacketHandler.UnregisterServerHandler<PlayerInputState>();
    }
}
