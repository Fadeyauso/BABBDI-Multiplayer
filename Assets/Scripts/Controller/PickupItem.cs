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
    public GameObject compass;
    public GameObject trumpet;
    public GameObject secretfinder;

    void Awake()
    {
        g = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {

        

            if (g.soap == 1) 
            {
                GameObject obj = Instantiate(soap, GameObject.Find("ObjectPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
            }
            if (g.club == 1) 
            {
                GameObject obj = Instantiate(club, GameObject.Find("ObjectPos2").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().club = obj;
            }
            if (g.climber == 1) 
            {
                GameObject obj = Instantiate(climber, GameObject.Find("Pickaxe01").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().climber = obj;
            }
            if (g.flashlight == 1) 
            {
                GameObject obj = Instantiate(flashlight, GameObject.Find("LightPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
            }
            if (g.propeller == 1) 
            {
                GameObject obj = Instantiate(propeller, GameObject.Find("PropellerPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().propeller = obj;
            }
            if (g.blower == 1) 
            {
                GameObject obj = Instantiate(blower, GameObject.Find("BlowerPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().blower = obj;
            }
            if (g.ball == 1) 
            {
                GameObject obj = Instantiate(ball, GameObject.Find("ObjectPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
            }
            if (g.bigball == 1) 
            {
                GameObject obj = Instantiate(bigball, GameObject.Find("BigBallPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
            }
            if (g.stick == 1) 
            {
                GameObject obj = Instantiate(stick, GameObject.Find("StickPos01").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
            }
            if (g.grabber == 1) 
            {
                GameObject obj = Instantiate(grabber, GameObject.Find("GrabberPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().grabber = obj;
            }
            if (g.motorBike == 1) 
            {
                GameObject obj = Instantiate(motorBike, GameObject.Find("MotoPos").transform.position, Quaternion.identity, GameObject.Find("Player").transform);
                obj.GetComponent<InteractObject>().inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().motorBike = obj;
            }
            if (g.compass == 1) 
            {
                GameObject obj = Instantiate(compass, GameObject.Find("CompassPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
            }
            if (g.trumpet == 1) 
            {
                GameObject obj = Instantiate(trumpet, GameObject.Find("TrumpetPos00").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
            }
            if (g.secretfinder == 1) 
            {
                GameObject obj = Instantiate(secretfinder, GameObject.Find("SecretFinderPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
                obj.GetComponent<InteractObject>().inHands = true;
            }
    }

    public void GetCompass()
    {

        GameObject obj = Instantiate(compass, GameObject.Find("CompassPos").transform.position, Quaternion.identity, GameObject.Find("Main Camera").transform);
        obj.GetComponent<InteractObject>().inHands = true;

    }

    private GameManager g;



    // Update is called once per frame
    void Update()
    {
        
        if (g.pickup)
        {
            g.pickup = false;

            KNetworkManager.instance.messenger.SendGlobalMessage(new ItemPickedUpMessage() { playerId = KNetworkManager.instance.localPlayerId, objectId = g.itemToPickup.GetNetObject().objectId.uid });

            if (g.item == 0) 
            {
                g.itemToPickup.transform.position = GameObject.Find("ObjectPos").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;
            }
            if (g.item == 1) 
            {
                g.itemToPickup.transform.position = GameObject.Find("ObjectPos2").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().club = g.itemToPickup.gameObject;
            }
            if (g.item == 2) 
            {

                g.itemToPickup.transform.position = GameObject.Find("Pickaxe01").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().climber = g.itemToPickup.gameObject;
            }
            if (g.item == 4) 
            {
                g.itemToPickup.transform.position = GameObject.Find("LightPos").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;
            }
            if (g.item == 5) 
            {
                g.itemToPickup.transform.position = GameObject.Find("PropellerPos").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;

                GameObject.Find("Player").GetComponent<FirstPersonController>().propeller = g.itemToPickup.gameObject;
            }
            if (g.item == 6) 
            {
                g.itemToPickup.transform.position = GameObject.Find("BlowerPos").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().blower = g.itemToPickup.gameObject;
            }
            if (g.item == 7) 
            {
                g.itemToPickup.transform.position = GameObject.Find("ObjectPos").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;
            }
            if (g.item == 8) 
            {
                g.itemToPickup.transform.position = GameObject.Find("BigBallPos").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;
            }
            if (g.item == 9) 
            {
                g.itemToPickup.transform.position = GameObject.Find("StickPos01").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;
            }
            if (g.item == 10) 
            {
                g.itemToPickup.transform.position = GameObject.Find("GrabberPos").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().grabber = g.itemToPickup.gameObject;
            }
            if (g.item == 11) 
            {
                g.itemToPickup.transform.position = GameObject.Find("MotoPos").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;
                GameObject.Find("Player").GetComponent<FirstPersonController>().motorBike = g.itemToPickup.gameObject;
            }
            if (g.item == 12) 
            {
                g.itemToPickup.transform.position = GameObject.Find("CompassPos").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;
            }
            if (g.item == 13) 
            {
                g.itemToPickup.transform.position = GameObject.Find("TrumpetPos00").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;
            }
            if (g.item == 16) 
            {
                g.itemToPickup.transform.position = GameObject.Find("SecretFinderPos").transform.position;
                g.itemToPickup.transform.parent = GameObject.Find("Main Camera").transform;
                g.itemToPickup.inHands = true;
            }
            
            
        }
    }
}
