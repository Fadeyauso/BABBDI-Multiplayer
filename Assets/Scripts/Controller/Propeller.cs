    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    public GameObject motor;
    public GameObject motorUp;
    private GameObject player;
    [SerializeField] private AudioSource audio;
    public Vector3 propellerMovement;
    public float power = 2f;
    public float acceleration = 10f;
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
        if (!player.GetComponent<FirstPersonController>().pause)
        {
            if ((Input.GetKey(KeyCode.Space) || Input.GetButton("Jump")) && GetComponent<InteractObject>().inHands && player.transform.position.y < 120)
            {
                motorUp.transform.Rotate(1000 * Time.deltaTime, 0, 0);
                player.GetComponent<FirstPersonController>().moveDirection.y += flypower * Time.deltaTime;
            }

            motor.transform.Rotate(0, rotation * Time.deltaTime, 0);

            if ((Input.GetButton("Fire1") || Input.GetAxis("LeftClick") > 0.1f) && GetComponent<InteractObject>().inHands && player.transform.position.y < 120)
            {
                isActive = true;
                rotation = rotateSpeed;
                turn = true;
                player.GetComponent<FirstPersonController>().moveDirection.y += propellerMovement.y * Time.deltaTime;

            }
            else 
            {
                isActive = false;
                if (turn) rotation -= 300f * Time.deltaTime;
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
            else if (isActive == false) audio.mute = true;

            Vector3 desiredSpeed = isActive ? player.GetComponent<FirstPersonController>().playerCamera.transform.forward * power : Vector3.zero;

            if (propellerMovement != desiredSpeed)
            {
                GetSpeed(desiredSpeed);
            }
        }
        else audio.mute = true;
        
    }

    private void GetSpeed(Vector3 tempspeed)
    {
        propellerMovement = Vector3.Lerp(propellerMovement, tempspeed, acceleration * Time.deltaTime);
    }
}
