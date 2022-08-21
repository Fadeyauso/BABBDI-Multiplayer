using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour
{
    private GameObject player;

    public AudioClip hold;
    public AudioClip hit;

    public bool trigger;

    public bool touch;

    
    
    


    public float impactForce = 2;

    void Awake(){
        player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {

        }
        
    }

    void OnTriggerEnter(Collider collisionInfo)
    {
        touch = true;
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.layer == 7 && GetComponent<InteractObject>().extended || collisionInfo.gameObject.layer == 0 && GetComponent<InteractObject>().extended)
        {
            trigger = true;
        }

        if (collisionInfo.gameObject.layer == 7 && GetComponent<InteractObject>().extended && touch || collisionInfo.gameObject.layer == 0 && GetComponent<InteractObject>().extended && touch)
        {
            SoundManager.Instance.PlaySound(hit);
            touch = false;
            if (player.GetComponent<FirstPersonController>().frontRay) 
            {
                player.GetComponent<FirstPersonController>().moveDirection += -player.GetComponent<FirstPersonController>().playerCamera.transform.forward * impactForce * 2;
            }
            else player.GetComponent<FirstPersonController>().moveDirection.y = impactForce;
        }

    }

    void OnTriggerExit(Collider collisionInfo)
    {
        trigger = false;
        
    }
}
