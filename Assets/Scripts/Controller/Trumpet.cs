using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trumpet : MonoBehaviour
{
    private bool active;
    private AudioSource audio;
    // Start is called before the first frame update
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<InteractObject>().inHands && !GameObject.Find("Player").GetComponent<FirstPersonController>().pause)
        
        {
            if (Input.GetButtonDown("Fire1"))
            {
                audio.Play();
                active = true;
            }

            if (Input.GetButtonUp("Fire1") || Input.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
            {
                audio.Stop();
                active = false;
            }

            audio.pitch = -GameObject.Find("Player").GetComponent<FirstPersonController>().rotationX / 90f + 1;
        }


    }
}
