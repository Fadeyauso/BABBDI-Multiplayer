using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public GameObject club;
    public GameObject climber;
    public GameObject propeller;
    public GameObject blower;
    public GameObject flashlight;
    public GameObject soap;
    public GameObject ball;
    public GameObject bigball;
    public GameObject stick;
    public GameObject grabber;
    public GameObject motorBike;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().pickup)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().pickup = false;
            if (GameObject.Find("GameManager").GetComponent<GameManager>().item == 0) 
            {
                GameObject obj = Instantiate(soap, GameObject.Find("ObjectPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
            }
            if (GameObject.Find("GameManager").GetComponent<GameManager>().item == 1) 
            {
                GameObject obj = Instantiate(club, GameObject.Find("ObjectPos2").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().club = obj;
            }
            if (GameObject.Find("GameManager").GetComponent<GameManager>().item == 2) 
            {
                GameObject obj = Instantiate(climber, GameObject.Find("Pickaxe01").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().climber = obj;
            }
            if (GameObject.Find("GameManager").GetComponent<GameManager>().item == 4) 
            {
                GameObject obj = Instantiate(flashlight, GameObject.Find("LightPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
            }
            if (GameObject.Find("GameManager").GetComponent<GameManager>().item == 5) 
            {
                GameObject obj = Instantiate(propeller, GameObject.Find("PropellerPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().propeller = obj;
            }
            if (GameObject.Find("GameManager").GetComponent<GameManager>().item == 6) 
            {
                GameObject obj = Instantiate(blower, GameObject.Find("BlowerPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().blower = obj;
            }
            if (GameObject.Find("GameManager").GetComponent<GameManager>().item == 7) 
            {
                GameObject obj = Instantiate(ball, GameObject.Find("ObjectPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
            }
            if (GameObject.Find("GameManager").GetComponent<GameManager>().item == 8) 
            {
                GameObject obj = Instantiate(bigball, GameObject.Find("BigBallPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
            }
            if (GameObject.Find("GameManager").GetComponent<GameManager>().item == 9) 
            {
                GameObject obj = Instantiate(stick, GameObject.Find("StickPos01").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
            }
            if (GameObject.Find("GameManager").GetComponent<GameManager>().item == 10) 
            {
                GameObject obj = Instantiate(grabber, GameObject.Find("GrabberPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().grabber = obj;
            }
            if (GameObject.Find("GameManager").GetComponent<GameManager>().item == 11) 
            {
                GameObject obj = Instantiate(motorBike, GameObject.Find("MotoPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().motorBike = obj;
            }
            
        }
    }
}
