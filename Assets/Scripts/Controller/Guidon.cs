using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guidon : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationAmount = 4f;
    public float maxRotationAmount = 5f;
    public float smoothRotation = 12f;

    [Space]
    public bool rotationX = true;
    public bool rotationY = true;
    public bool rotationZ = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.parent.GetComponent<InteractObject>().inHands)
        {
            TiltSway(Quaternion.Euler(0, GameObject.Find("MotoPos").transform.rotation.y, 32.997f));
        }
    }

    public void TiltSway(Quaternion initialRotation)
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().gamepad)
        {
            float tiltY = Mathf.Clamp(-Input.GetAxis("CameraHorizontal") * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(-Input.GetAxis("CameraVertical") * rotationAmount, -maxRotationAmount, maxRotationAmount);
                Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? tiltX : 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, Time.deltaTime * smoothRotation);
        }
        else{
            float tiltY = Mathf.Clamp(-Input.GetAxis("Mouse X") * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(-Input.GetAxis("Mouse Y") * rotationAmount, -maxRotationAmount, maxRotationAmount);
                Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? tiltX : 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, Time.deltaTime * smoothRotation);
        }


    }

    public void TiltSwayGlobal(Quaternion initialRotation)
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().gamepad)
        {
float tiltY = Mathf.Clamp(-Input.GetAxis("CameraHorizontal") * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(-Input.GetAxis("CameraVertical") * rotationAmount, -maxRotationAmount, maxRotationAmount);
        
        Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? tiltX : 0f, rotationY ? tiltX : 0f, rotationZ ? tiltY : 0f));

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation * initialRotation, Time.deltaTime * smoothRotation);
        }
        else 
        {
float tiltY = Mathf.Clamp(-Input.GetAxis("Mouse X") * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(-Input.GetAxis("Mouse Y") * rotationAmount, -maxRotationAmount, maxRotationAmount);
        
        Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? tiltX : 0f, rotationY ? tiltX : 0f, rotationZ ? tiltY : 0f));

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation * initialRotation, Time.deltaTime * smoothRotation);
        }
        

    }
}
