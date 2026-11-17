using System;

class AvalonAuthorizedAttribute : Attribute
{
    public string PlayerId { get; private set; }

    public void SetPlayerId(string playerId)
    {
        PlayerId = playerId;
    }
}