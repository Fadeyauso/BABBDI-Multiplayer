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
    private Quaternion initRotation;
    public GameObject guidon;

    float rotationZ;




    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
    }

    void Start()
    {
        initRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton("Fire1") && GetComponent<InteractObject>().inHands)
        {
            isActive = true;
            float tiltY = Mathf.Clamp(-Input.GetAxis("Mouse X") * rotationAmount, -maxRotationAmount, maxRotationAmount);


            var desiredTilt = player.GetComponent<FirstPersonController>().verticalInputRaw;
            var desiredRotation = tiltY;
            if (rotationZ != desiredTilt) SideTilt(desiredTilt);
            if (guidon.transform.localRotation != desiredRotation) GuidonTilt(desiredRotation);

            transform.rotation = new Quaternion.Euler(0, 0, rotationZ);
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

        Vector3 desiredSpeed = isActive ? new Vector3(player.GetComponent<FirstPersonController>().playerCamera.transform.forward.x, 0, player.GetComponent<FirstPersonController>().playerCamera.transform.forward.z) * power : Vector3.zero;

        if (motorMovement != desiredSpeed)
        {
            GetSpeed(desiredSpeed);
        }
    }

    private void GetSpeed(Vector3 tempspeed)
    {
        motorMovement = Vector3.Lerp(motorMovement, tempspeed, acceleration * Time.deltaTime);
    }

    private void SideTilt(float desiredTilt)
    {
        rotationZ = Mathf.Lerp(rotationZ, temptiltAmount, tiltSpeed * Time.deltaTime);
    }

    private void GuidonTilt()
    {
        float tiltY = Mathf.Clamp(-Input.GetAxis("Mouse X") * rotationAmount, -maxRotationAmount, maxRotationAmount);
        Mathf.Lerp(guidon.transform.localRotation.z, tiltY, Time.deltaTime * smoothRotation);
    }
}
