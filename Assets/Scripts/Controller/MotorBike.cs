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
    public float mouseLook;
    public float timeToImpress = 3;
    public float impressTimer;
    

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

        if (!player.GetComponent<FirstPersonController>().characterController.isGrounded && GetComponent<InteractObject>().inHands)
        {
            mouseLook -= Input.GetAxis("Mouse X") * GameObject.Find("Player").GetComponent<FirstPersonController>().lookSpeedX;

            if (mouseLook > 360 || mouseLook < -360)
            {
                if (!GameObject.Find("GameManager").GetComponent<GameManager>().bikeAir) GameObject.Find("GameManager").GetComponent<GameManager>().Popup();
                GameObject.Find("GameManager").GetComponent<GameManager>().bikeAir = true;
                GameObject.Find("GameManager").GetComponent<GameManager>().bikeAirState = 1;
                GameObject.Find("GameManager").GetComponent<GameManager>().lastAchievement = "Way of the biker";
            }
        }
        else mouseLook = 0;

        if (player.GetComponent<EnterZone>().inGirlZone && GetComponent<InteractObject>().inHands)
        {
            if (Input.GetButton("Fire1") && player.GetComponent<FirstPersonController>().characterController.velocity.magnitude > 1)
            {
                impressTimer += Time.deltaTime;
                if (impressTimer > timeToImpress)
                {
                    if (!GameObject.Find("GameManager").GetComponent<GameManager>().impressGirl) GameObject.Find("GameManager").GetComponent<GameManager>().Popup();
                    GameObject.Find("GameManager").GetComponent<GameManager>().impressGirl = true;
                    GameObject.Find("GameManager").GetComponent<GameManager>().impressGirlState = 1;
                    GameObject.Find("GameManager").GetComponent<GameManager>().lastAchievement = "Way of the seducer";
                }
            }
            else impressTimer = 0;
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
