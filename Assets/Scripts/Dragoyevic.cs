using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragoyevic : MonoBehaviour
{

    public bool giveCompass;
    public bool compassAvailable;
    // Start is called before the first frame update
    void Start()
    {
        compassAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (giveCompass && GetComponent<NPC>().dialogueIndex2 == 1)
        {
            giveCompass = false;
            compassAvailable = false;
            Debug.Log("cacou");
            GameObject.Find("Player").GetComponent<PickupItem>().GetCompass();
        }
    }
}
