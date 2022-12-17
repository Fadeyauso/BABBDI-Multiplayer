using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamIntegration : MonoBehaviour
{
    public static SteamIntegration Instance;
    private bool connected;

    void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {

            Steamworks.SteamClient.Init(2240530);
            PrintYourName();
            connected = true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }


    private void PrintYourName()
    {
        Debug.Log("Steam Marche");
    }

    [ContextMenu("IsThisAchievementUnlocked")]
    public void IsThisAchievementUnlocked(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);

    }

    [ContextMenu("UnlockAchievement")]
    public void UnlockAchivement(string id)
    {
        if (connected)
        {
            var ach = new Steamworks.Data.Achievement(id);
            if (ach.State == false) ach.Trigger();
            Debug.Log("AchievementUnlocked : " + id);
        }

    }

    [ContextMenu("ClearAchievementStatus")]
    public void ClearAchievementStatus(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Clear();
    }


}