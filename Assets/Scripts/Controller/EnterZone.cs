using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterZone : MonoBehaviour
{

    private float indoortimer;
    private float outdoortimer;

    [HideInInspector] public bool inLift;

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
        if (collisionInfo.tag == "Achievement/wayClimber")
        {
            if (!GameObject.Find("GameManager").GetComponent<GameManager>().wayClimber) GameObject.Find("GameManager").GetComponent<GameManager>().Popup();
            GameObject.Find("GameManager").GetComponent<GameManager>().wayClimber = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().lastAchievement = "Way of the climber";
        }
        
        
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Lift")
        {
            transform.SetParent(GameObject.Find("RestaurantLift").transform);
            inLift = true;
        }
    }

    void OnTriggerExit(Collider collisionInfo)
    {
        transform.parent = null;
        inLift = false; 

    }
}
