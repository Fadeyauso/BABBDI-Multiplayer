using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour
{
    private GameObject player;

    public AudioClip hold;
    public AudioClip hit;

    public bool trigger;
    private bool hitb;
    public float timer = 0;

    public bool touch;

    public bool canTouch = false;

    
    
    


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
        if (Input.GetButtonDown("Fire1"))
        {
            hitb = true;
        }
        timer -= Time.deltaTime;

        if (player.GetComponent<FirstPersonController>().frontRay && player.GetComponent<FirstPersonController>().clubRay) timer = 0.3f;

        canTouch = true;
        
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

        if (collisionInfo.gameObject.layer == 7 && GetComponent<InteractObject>().extended && touch && hitb|| collisionInfo.gameObject.layer == 0 && GetComponent<InteractObject>().extended && touch && hitb)
        {
            if (canTouch)
            {
                SoundManager.Instance.PlaySound(hit);
                touch = false;
                hitb = false;

                if (timer > 0) 
                {
                    player.GetComponent<FirstPersonController>().moveDirection += -player.transform.forward * impactForce;
                    player.GetComponent<FirstPersonController>().moveDirection.y =  impactForce / 4;
                }
                else player.GetComponent<FirstPersonController>().moveDirection.y = impactForce;
            }
            
        }

    }

    void OnTriggerExit(Collider collisionInfo)
    {
        trigger = false;
        
    }
}
