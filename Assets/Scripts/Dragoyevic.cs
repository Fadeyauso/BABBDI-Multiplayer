using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragoyevic : MonoBehaviour
{

    public bool giveCompass;
    public bool compassAvailable;
    public bool activateBridge;
    public bool compassOK;

    private bool compassGive;
    private GameManager gameManager;
    private PickupItem player;
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        compassAvailable = true;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<PickupItem>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (giveCompass && GetComponent<NPC>().dialogueIndex2 == 0 && compassAvailable && compassOK)
        {
            giveCompass = false;
            compassAvailable = false;
            Debug.Log("cacou");
            player.GetCompass();
        }

        if (activateBridge) {
            gameManager.bridgeTimer = 5;
            activateBridge = false;
        }
        if (gameManager.bridgeTimer > 0) ActivateBridge();

        if (gameManager.bridgeTimer < 1 && gameManager.bridgeTimer > 0) 
        {
            gameManager.bridgeCam.SetActive(false);
            gameManager.userInterface.SetActive(true);
            gameManager.userInterface1.SetActive(true);
        }
    }

    void ActivateBridge()
    {
        gameManager.bridgeCam.SetActive(true);
        gameManager.userInterface.SetActive(false);
        gameManager.userInterface1.SetActive(false);
    
        gameManager.anim1.SetBool("Trig1", true);
        gameManager.anim2.SetBool("Trig1", true);

    }
}
