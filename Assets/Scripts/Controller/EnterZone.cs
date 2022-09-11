using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterZone : MonoBehaviour
{

    private float indoortimer;
    private float outdoortimer;

    [HideInInspector] public bool inLift;

    public AudioClip endGameClip;
    

    // Update is called once per frame
    void Update()
    {
        indoortimer -= Time.deltaTime;
        outdoortimer -= Time.deltaTime;

        if(indoortimer > 0) SoundManager.Instance.IndoorBlend();
        if(outdoortimer > 0) SoundManager.Instance.OutdoorBlend();
    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Door/Indoor")
        {
            indoortimer = 4;
            outdoortimer = 0;
        }
        if (collisionInfo.tag == "Door/Outdoor")
        {
            outdoortimer = 4;
            indoortimer = 0;
        }
        if (collisionInfo.tag == "Subway/In")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().inSubway = true;
        }
        if (collisionInfo.tag == "Subway/Out")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().inSubway = false;
        }
        if (collisionInfo.tag == "Club/In")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().inClub = true;
        }
        if (collisionInfo.tag == "Club/Out")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().inClub = false;
        }
        if (collisionInfo.tag == "Achievement/wayClimber")
        {
            if (!GameObject.Find("GameManager").GetComponent<GameManager>().wayClimber) GameObject.Find("GameManager").GetComponent<GameManager>().Popup();
            GameObject.Find("GameManager").GetComponent<GameManager>().wayClimber = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().wayClimberState = 1;
            GameObject.Find("GameManager").GetComponent<GameManager>().lastAchievement = "Way of the climber";
        }
        if (collisionInfo.tag == "Train")
        {
            SoundManager.Instance.PlayRealMusic(endGameClip, 1);
            GameObject.Find("GameManager").GetComponent<GameManager>().endGame = true;
        }
        if (collisionInfo.tag == "KillZ")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().ReturnHome();
        }
        if (collisionInfo.tag == "MusicZone" && collisionInfo.GetComponent<MusicZone>().timer < 0)
        {
            SoundManager.Instance.PlayMusic(collisionInfo.GetComponent<MusicZone>().music, 0);
            collisionInfo.GetComponent<MusicZone>().timer = collisionInfo.GetComponent<MusicZone>().countown;
        }
        if (collisionInfo.tag == "MusicZonePartTwo" && GameObject.Find("GameManager").GetComponent<GameManager>().secondPart == 1 && collisionInfo.GetComponent<MusicZone>().timer < 0)
        {
            Debug.Log("camarche");
            SoundManager.Instance.PlayMusic(collisionInfo.GetComponent<MusicZone>().music, 0);
            collisionInfo.GetComponent<MusicZone>().timer = collisionInfo.GetComponent<MusicZone>().countown;
        }
        if (collisionInfo.tag == "MusicZonePartOne" && GameObject.Find("GameManager").GetComponent<GameManager>().secondPart == 0 && collisionInfo.GetComponent<MusicZone>().timer < 0)
        {
            SoundManager.Instance.PlayMusic(collisionInfo.GetComponent<MusicZone>().music, 0);
            collisionInfo.GetComponent<MusicZone>().timer = collisionInfo.GetComponent<MusicZone>().countown;
        }

        
        
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Lift")
        {
            transform.SetParent(collisionInfo.GetComponent<SetLift>().lift.transform);
            inLift = true;
        }
        if (collisionInfo.tag == "Train")
        {
            transform.parent = collisionInfo.transform;
        }
    }

    void OnTriggerExit(Collider collisionInfo)
    {
        transform.parent = null;
        inLift = false; 

    }
}
