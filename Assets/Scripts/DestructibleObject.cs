using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    private Rigidbody r;
    private GameObject player;
    private bool separate;
    
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
        if (transform.parent == null && separate)
        {
            separate = false;
            r.AddForce(player.GetComponent<FirstPersonController>().playerCamera.transform.forward * 60f, ForceMode.Impulse);
        }
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        
        if (transform.parent != null)
        {
            if (transform.parent.GetComponent<PlankParent>().destroyed)
            {
                var caca = Random.Range(0, 10);
                if (caca < 2.5f) SoundManager.Instance.PlaySound(GameObject.Find("GameManager").GetComponent<GameManager>().destroy);
                transform.parent.GetComponent<PlankParent>().destroyed = true;
                transform.parent = null;
                gameObject.layer = 12;
                r.constraints = 0;
                r.AddForce(player.GetComponent<FirstPersonController>().playerCamera.transform.forward * 60f, ForceMode.Impulse);
                separate = true;
            }
            else if ((collisionInfo.gameObject.layer == 16 || collisionInfo.gameObject.layer == 7) && Input.GetButton("Fire1"))
            {
                transform.parent.GetComponent<PlankParent>().destroyed = true;
                transform.parent = null;
                gameObject.layer = 12;
                r.constraints = 0;
                r.AddForce(player.GetComponent<FirstPersonController>().playerCamera.transform.forward * 60f, ForceMode.Impulse);
                separate = true;
            }
        }
        

        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Rigidbody>() != null && (collision.collider.gameObject.layer == 16 || collision.collider.gameObject.layer == 7))
        {
            transform.parent = null;
            gameObject.layer = 12;
            r.constraints = 0;
            r.AddForce(player.GetComponent<FirstPersonController>().playerCamera.transform.forward * 10f, ForceMode.Impulse);
        }

        
    }
}
