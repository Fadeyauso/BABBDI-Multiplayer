using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragoyevic : MonoBehaviour
{

    public bool giveCompass;
    public bool compassAvailable;
    public bool activateBridge;

    private bool compassGive;

    // Start is called before the first frame update
    void Start()
    {
        compassAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (giveCompass && GetComponent<NPC>().dialogueIndex2 == 0 && compassAvailable)
        {
            giveCompass = false;
            compassAvailable = false;
            Debug.Log("cacou");
            GameObject.Find("Player").GetComponent<PickupItem>().GetCompass();
        }

        if (activateBridge) {
            GameObject.Find("GameManager").GetComponent<GameManager>().bridgeTimer = 5;
            activateBridge = false;
        }
        if (GameObject.Find("GameManager").GetComponent<GameManager>().bridgeTimer > 0) ActivateBridge();

        if (GameObject.Find("GameManager").GetComponent<GameManager>().bridgeTimer < 1 && GameObject.Find("GameManager").GetComponent<GameManager>().bridgeTimer > 0) 
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().bridgeCam.SetActive(false);
            GameObject.Find("GameManager").GetComponent<GameManager>().userInterface.SetActive(true);
            GameObject.Find("GameManager").GetComponent<GameManager>().userInterface1.SetActive(true);
        }
    }

    void ActivateBridge()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().bridgeCam.SetActive(true);
        GameObject.Find("GameManager").GetComponent<GameManager>().userInterface.SetActive(false);
        GameObject.Find("GameManager").GetComponent<GameManager>().userInterface1.SetActive(false);
        
        GameObject.Find("GameManager").GetComponent<GameManager>().anim1.SetBool("Trig1", true);
        GameObject.Find("GameManager").GetComponent<GameManager>().anim2.SetBool("Trig1", true);

    }
}
