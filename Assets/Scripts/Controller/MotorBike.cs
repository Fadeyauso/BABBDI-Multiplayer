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
    [SerializeField] private AudioClip idle;
    [SerializeField] private AudioClip run;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Start()
    {
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButton("Fire1") && GetComponent<InteractObject>().onMoto)
        {
            isActive = true;
            
        }
        else 
        {
            isActive = false;
        }

        if (Input.GetButtonDown("Fire1")) audio.clip = run;
        if (Input.GetButtonUp("Fire1")) audio.clip = idle;

        if (!player.GetComponent<FirstPersonController>().pause)
        {
            if (!GetComponent<InteractObject>().inHands) audio.Stop();
            else{
                if (Input.GetButtonDown("Fire1"))
                {
                    audio.Play();
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    audio.Play();
                }
            }
            
        }

        

        if (player.GetComponent<FirstPersonController>().characterController.isGrounded) desiredSpeed = isActive ? new Vector3(player.transform.forward.x, 0, player.transform.forward.z) * power : Vector3.zero;

        if (motorMovement != desiredSpeed)
        {
            if (player.GetComponent<FirstPersonController>().characterController.isGrounded) GetSpeed(desiredSpeed);
        }
    }

    private void GetSpeed(Vector3 tempspeed)
    {
        motorMovement = Vector3.Lerp(motorMovement, tempspeed, acceleration * Time.deltaTime);
    }
}
