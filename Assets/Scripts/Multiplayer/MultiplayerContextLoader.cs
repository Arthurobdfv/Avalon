using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MultiplayerContextLoader : MonoBehaviour
{
    private static GameObject ServerInstanceGameObject;
    private static GameObject ClientInstanceGameObject;

    [SerializeField] SceneAsset GameScene;
    [SerializeField] SceneAsset MenuScene;

    public GameObject ServerPrefab;
    public GameObject ClientPrefab;

    // Gametypes that require Local Server Setup
    public static List<AvalonGameContext.GAMETYPE> LocalHostedServer = new List<AvalonGameContext.GAMETYPE>() { AvalonGameContext.GAMETYPE.LOCAL_OFFLINE };
    public static List<AvalonGameContext.GAMETYPE> RequireNetworkSetupTypes = new List<AvalonGameContext.GAMETYPE>() { AvalonGameContext.GAMETYPE.LOCAL_HOSTED, AvalonGameContext.GAMETYPE.LOCAL_HOSTED_CLIENT, AvalonGameContext.GAMETYPE.SERVER_HOSTED_CLIENT, AvalonGameContext.GAMETYPE.SERVER_HOSTED_CLIENT };
    public static List<AvalonGameContext.GAMETYPE> RequreClientInstanceTypes = new List<AvalonGameContext.GAMETYPE>() { AvalonGameContext.GAMETYPE.LOCAL_OFFLINE, AvalonGameContext.GAMETYPE.LOCAL_HOSTED_CLIENT, AvalonGameContext.GAMETYPE.LOCAL_HOSTED, AvalonGameContext.GAMETYPE.SERVER_HOSTED_CLIENT };
    public static List<AvalonGameContext.GAMETYPE> RequreServerInstanceTypes = new List<AvalonGameContext.GAMETYPE>() { AvalonGameContext.GAMETYPE.LOCAL_OFFLINE, AvalonGameContext.GAMETYPE.LOCAL_HOSTED, AvalonGameContext.GAMETYPE.LOCAL_HOSTED };
    // Start is called before the first frame update
    void Start()
    {
        if(AvalonGameContext.Instance == null || AvalonGameContext.Instance.CurrentGameType == AvalonGameContext.GAMETYPE.NONE)
        {
            Debug.LogError("AvalonGameContext Instance is null. Returning to Menu Scene.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(MenuScene.name);
            return;
        }
        if(ServerInstanceGameObject != null)
        {
            Destroy(ServerInstanceGameObject);
        }
        if(ClientInstanceGameObject != null)
        {
            Destroy(ClientInstanceGameObject);
        }
        if(RequireServerInstance(AvalonGameContext.Instance.CurrentGameType))
        {
            ServerInstanceGameObject = Instantiate(ServerPrefab);
            DontDestroyOnLoad(ServerInstanceGameObject);
        }
        if(RequireClientInstance(AvalonGameContext.Instance.CurrentGameType))
        {
            ClientInstanceGameObject = Instantiate(ClientPrefab);
            DontDestroyOnLoad(ClientInstanceGameObject);
        }   
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameScene.name);
    }

    public static bool RequireNetworkSetup(AvalonGameContext.GAMETYPE selectedGameType)
    {
        return RequireNetworkSetupTypes.Contains(selectedGameType);
    }

    public static bool RequireClientInstance(AvalonGameContext.GAMETYPE selectedGameType)
    {
        return RequreClientInstanceTypes.Contains(selectedGameType);
    }

    public static bool RequireServerInstance(AvalonGameContext.GAMETYPE selectedGameType)
    {
        return RequreServerInstanceTypes.Contains(selectedGameType);
    }
}
