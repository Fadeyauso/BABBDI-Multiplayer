using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour
{
    private GameObject player;
    
    


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
        if (collisionInfo.gameObject.layer == 7 && GetComponent<InteractObject>().extended || collisionInfo.gameObject.layer == 0 && GetComponent<InteractObject>().extended)
        {
            player.GetComponent<FirstPersonController>().moveDirection.y = impactForce;
        }
    }
}
