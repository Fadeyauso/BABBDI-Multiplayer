using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterZone : MonoBehaviour
{
    public AudioClip indoor;
    public AudioClip outdoor;

    [HideInInspector] public bool inLift;
    // Start is called before the first frame update
    void Start()
    {
         SoundManager.Instance.PlayMusic(outdoor, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Door/Indoor")
        {
            SoundManager.Instance.StopAllMusic();
            SoundManager.Instance.PlayMusic(indoor, 1);
        }
        if (collisionInfo.tag == "Door/Outdoor")
        {
            SoundManager.Instance.StopAllMusic();
            SoundManager.Instance.PlayMusic(outdoor, 1);
        }
        if (collisionInfo.tag == "Achievement/wayClimber")
        {
            if (!GameObject.Find("GameManager").GetComponent<GameManager>().wayClimber) GameObject.Find("GameManager").GetComponent<GameManager>().Popup();
            GameObject.Find("GameManager").GetComponent<GameManager>().wayClimber = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().lastAchievement = "Way of the climber";
        }
        if (collisionInfo.tag == "Lift")
        {
            Debug.Log("caca");
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
