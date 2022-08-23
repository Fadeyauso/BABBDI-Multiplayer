using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blower : MonoBehaviour
{
    public Transform origin;
    private GameObject player;
    public float power = 0.2f; 
    public float burstPower = 4f;
    public bool isActive;
    private bool canBurst;
    public AudioClip burstClip;
    public AudioClip motorClip;
    public Vector3 blowMovement;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<FirstPersonController>().characterController.isGrounded) canBurst = true;

        if (Input.GetButtonDown("Fire1") && GetComponent<InteractObject>().inHands && canBurst)
        {
            SoundManager.Instance.PlaySound(burstClip);
            player.GetComponent<FirstPersonController>().moveDirection = -player.GetComponent<FirstPersonController>().playerCamera.transform.forward * burstPower;
            canBurst = false;
        }

        if (Input.GetButton("Fire1") && GetComponent<InteractObject>().inHands)
        {
            SoundManager.Instance.PlaySound(motorClip);
            isActive = true;
            blowMovement += -player.GetComponent<FirstPersonController>().playerCamera.transform.forward * power / 4;
            player.GetComponent<FirstPersonController>().moveDirection += -player.GetComponent<FirstPersonController>().playerCamera.transform.forward * power;
        }
        else 
        {
            blowMovement = new Vector3(0,0,0);
            isActive = false;
        }

        


    }
}
