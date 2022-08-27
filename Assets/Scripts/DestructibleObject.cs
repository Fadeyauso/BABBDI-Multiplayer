using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    private Rigidbody r;
    private GameObject player;

    void Awake()
    {
        player = GameObject.Find("Player");
        r = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.layer == 7 && Input.GetButton("Fire1"))
        {
            transform.parent = null;
            r.constraints = 0;
            r.AddForce(player.GetComponent<FirstPersonController>().playerCamera.transform.forward * 10f, ForceMode.Impulse);
        }

        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 7 && Input.GetButton("Fire1"))
        {
            transform.parent = null;
            r.constraints = 0;
            r.AddForce(player.GetComponent<FirstPersonController>().playerCamera.transform.forward * 10f, ForceMode.Impulse);
        }

        
    }
}
