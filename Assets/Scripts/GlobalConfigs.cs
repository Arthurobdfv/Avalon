using UnityEngine;

[CreateAssetMenu(fileName = "GlobalConfigs", menuName = "Configs/GlobalConfigs")]
public class GlobalConfigs : ScriptableObject
{
    [SerializeField] private bool _multiplayerEnabled;
    public bool MultiplayerEnabled => _multiplayerEnabled;
}