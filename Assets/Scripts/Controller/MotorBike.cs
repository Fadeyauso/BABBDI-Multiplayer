using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorBike : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private AudioSource audio;
    public Vector3 motorMovement;
    public float power = 2f;
    public float acceleration = 10f;
    public bool isActive;
    public GameObject guidon;
    Vector3 desiredSpeed;

    [SerializeField] private float tiltAmount = 7f;
    [SerializeField] private float tiltSpeed = 7f;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton("Fire1") && GetComponent<InteractObject>().inHands)
        {
            isActive = true;
        }
        else 
        {
            isActive = false;
        }

        if (!player.GetComponent<FirstPersonController>().pause)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                audio.mute = false;
                audio.Play();
            }
            if (isActive == false) 
            {
                audio.mute = true;
                audio.Stop();
            }
        }

        if (player.GetComponent<FirstPersonController>().characterController.isGrounded) desiredSpeed = isActive ? new Vector3(player.transform.forward.x, 0, player.transform.forward.z) * power : Vector3.zero;

        if (motorMovement != desiredSpeed)
        {
            GetSpeed(desiredSpeed);
        }
    }

    private void GetSpeed(Vector3 tempspeed)
    {
        motorMovement = Vector3.Lerp(motorMovement, tempspeed, acceleration * Time.deltaTime);
    }
}
