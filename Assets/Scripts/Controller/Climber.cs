using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climber : MonoBehaviour
{
    private GameObject player;
    float timer = 0;

    public AudioClip hold;
    public AudioClip hit;

    public bool trigger;
    public bool pick = false;

    public bool touch;

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
        timer -= Time.deltaTime;
        if (Input.GetButtonDown("Jump") && trigger)
        {
            pick = false;
            timer = 0.2f;
            trigger = false;
            player.GetComponent<FirstPersonController>().moveDirection.y = player.GetComponent<FirstPersonController>().jumpForce;
        }

        if (!Input.GetButton("Fire1"))
        {
            trigger = false;

            
        }

        if (Input.GetButtonDown("Fire1"))
        {
            timer = 0f;
            hitb = true;
            pick = true;
        }

        if (GetComponent<InteractObject>().extended && timer > 0 && timer < 0.05f)
        {
            hitb = true;
        }
        
    }
    bool hitb;
    void OnTriggerEnter(Collider collisionInfo)
    {
        touch = true;
        
    }

    void OnTriggerStay(Collider collisionInfo)
    {

        if (collisionInfo.gameObject.layer == 7 && GetComponent<InteractObject>().extended && pick || collisionInfo.gameObject.layer == 0 && GetComponent<InteractObject>().extended && pick)
        {
            pick = false;
            touch = false;
            if (timer < 0) trigger = true;

            if (hitb) 
            {
                SoundManager.Instance.PlaySound(hit);
                hitb = false;
            }
            

            if (player.GetComponent<FirstPersonController>().climbRay) 
            {
                
            }
        }

    }

}
