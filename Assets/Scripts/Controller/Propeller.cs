using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    public GameObject motor;
    public GameObject motorUp;
    public GameObject motorUp2;
    private GameObject player;
    [SerializeField] private AudioSource audio;
    public float power = 2f;
    public float flypower = 0.8f;
    public float rotateSpeed = 100f;
    public float rotation;
    public bool isActive;
    private bool turn;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && GetComponent<InteractObject>().inHands && player.transform.position.y < 120)
        {
            motorUp.transform.Rotate(0, 30, 0);
            motorUp2.transform.Rotate(0, -30, 0);
            player.GetComponent<FirstPersonController>().moveDirection.y += flypower * Time.deltaTime;
        }

        motor.transform.Rotate(0, rotation, 0);

        if (Input.GetButton("Fire1") && GetComponent<InteractObject>().inHands && player.transform.position.y < 120)
        {
            isActive = true;
            rotation = rotateSpeed;
            turn = true;
            if (player.GetComponent<FirstPersonController>().characterController.velocity.magnitude < 30)
                player.GetComponent<FirstPersonController>().moveDirection += player.GetComponent<FirstPersonController>().playerCamera.transform.forward * power * Time.deltaTime;


            
        }
        else 
        {
            isActive = false;
            if (turn) rotation -= 0.3f;
            if (rotation < 0) turn = false;
        }

        if (!player.GetComponent<FirstPersonController>().pause && player.transform.position.y < 120)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                audio.mute = false;
            }
            if (isActive == false) audio.mute = true;
        }

        

        


    }
}
