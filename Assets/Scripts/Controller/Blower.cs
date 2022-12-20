using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blower : MonoBehaviour
{
    public Transform origin;
    private GameObject player;
    public float power = 0.2f; 
    public bool isActive;
    private bool canfly;

    private float timer = 0;
    [SerializeField] private float battery;
    [SerializeField] private float batteryMax = 2;

    public AudioClip motorClip;
    public AudioClip endClip;
    public Vector3 blowMovement;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (player.GetComponent<FirstPersonController>().characterController.isGrounded) battery = batteryMax;

        if ((Input.GetButtonDown("Fire1")) && GetComponent<InteractObject>().inHands && battery > 0)
        {
            if (!GameObject.Find("Player").GetComponent<FirstPersonController>().pause) SoundManager.Instance.PlayContinuousSound(motorClip);
        }

        if ((Input.GetButton("Fire1")) && GetComponent<InteractObject>().inHands)
        {
            timer = 0.05f;
            isActive = true;
            battery -= Time.deltaTime;
            if (battery > 0)
            {
                if (Physics.Raycast(player.transform.position, player.GetComponent<FirstPersonController>().playerCamera.transform.forward, out RaycastHit slopeHit, 3f))
                {
                    if (player.GetComponent<FirstPersonController>().characterController.isGrounded && Vector3.Angle(slopeHit.normal, Vector3.up) < player.GetComponent<FirstPersonController>().characterController.slopeLimit) player.GetComponent<FirstPersonController>().moveDirection.y = 4;
                }
                blowMovement += -player.GetComponent<FirstPersonController>().playerCamera.transform.forward * power / 4 * Time.deltaTime;
                player.GetComponent<FirstPersonController>().moveDirection += -player.GetComponent<FirstPersonController>().playerCamera.transform.forward * power * Time.deltaTime;
            }
            else blowMovement = new Vector3(0,0,0);
            
        }
        
        if (((Input.GetButtonUp("Fire1")) && GetComponent<InteractObject>().inHands) && battery > 0 || (battery < 0 && battery > -0.03f))
        {
            
            SoundManager.Instance.StopSound();
            if (!GameObject.Find("Player").GetComponent<FirstPersonController>().pause) SoundManager.Instance.PlaySound(endClip);
        }

        if (GetComponent<InteractObject>().inHands && (!Input.GetButton("Fire1")))
        {
            blowMovement = new Vector3(0,0,0);
            isActive = false;
        }
        else if (!GetComponent<InteractObject>().inHands)
        {
            blowMovement = new Vector3(0,0,0);
            isActive = false;
        }



        


        


    }
}
