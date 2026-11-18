using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvalonGameContext : MonoBehaviour
{
    public static AvalonGameContext Instance = null;
    public GAMETYPE CurrentGameType = GAMETYPE.NONE;
    public enum GAMETYPE
    {
        NONE,
        SERVER_BUILD,
        LOCAL_OFFLINE,
        LOCAL_HOSTED,
        LOCAL_HOSTED_CLIENT,
        SERVER_HOSTED_CLIENT
    }
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            CurrentGameType = GAMETYPE.NONE;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ConfigureGameType(GAMETYPE type)
    {
        CurrentGameType = type;
    }
}
