using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pogo : MonoBehaviour
{
    private GameObject player;

    public AudioClip hit;

    public bool active;
    public float timer;

    public Vector3 pogoMovement;



    
    
    


    public float impactForce = 4;

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
        if (Input.GetButton("Fire1"))
        {
            active = true;
        }
        else    
            active = false;

  

        
    }

    void OnCollisionStay(Collision collision)
    {
        if (active)
        {
            timer = 0.3f;
            ContactPoint contact = collision.contacts[0];
            pogoMovement += contact.normal * impactForce;
        }
        
    }

    void OnCollisionExit(Collision collision)
    {
        pogoMovement = Vector3.Lerp(pogoMovement, new Vector3(0,0,0), 1f);
        
    }

}
