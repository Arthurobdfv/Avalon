using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuGameTypeSelection : MonoBehaviour
{
    [SerializeField] private SceneAsset LoadingScene;
    [SerializeField] private TMPro.TMP_InputField _playerNameField;
    public void SetSinglePlayerGame()
    {
        AvalonGameContext.Instance.CurrentGameType = AvalonGameContext.GAMETYPE.LOCAL_OFFLINE;
        StartLoadingScene();
    }

    public void SetHostLocalGame()
    {
        AvalonGameContext.Instance.CurrentGameType = AvalonGameContext.GAMETYPE.LOCAL_HOSTED;
        if (ValidatePlayerName(_playerNameField.text))
        {
            LocalServerPacketSender.CurrentPlayerId = _playerNameField.text;
            StartLoadingScene();
        }
    }

    public void SetClientLocalGame()
    {
        AvalonGameContext.Instance.CurrentGameType = AvalonGameContext.GAMETYPE.LOCAL_HOSTED_CLIENT;
        if (ValidatePlayerName(_playerNameField.text))
        {
            LocalServerPacketSender.CurrentPlayerId = _playerNameField.text;
            StartLoadingScene();
        }
    }

    private bool ValidatePlayerName(string name)
    {
        return name.Length >= 6 && name.Length <= 20;
    }

    private void StartLoadingScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(LoadingScene.name);
    }
}
