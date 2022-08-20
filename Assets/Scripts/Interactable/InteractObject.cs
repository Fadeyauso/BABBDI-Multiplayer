using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : Interactable
{
    public bool inHands;
    public bool throwObject;
    public bool extended;

    public Rigidbody rb;

    private float timer;
    private float defaultYPos;

    public AudioClip interactionClip;

    public Collider collider;


    public override void OnFocus()
    {
        Debug.Log("asccaca");
    }

    public override void OnInteract()
    {
        SoundManager.Instance.PlaySound(interactionClip);

        if (GetComponent<ItemProperties>().id == 0 || GetComponent<ItemProperties>().id == 1)
        {
            transform.position = GameObject.Find("ObjectPos").transform.position;
            inHands = !inHands;
            throwObject = true;
        }


        
    }

    public override void OnLoseFocus()
    {
        Debug.Log("aaaa");

    }

    public void Awake()
    {
        defaultYPos = transform.position.y;

        collider = GetComponent<Collider>();
    }

    public void Update()
    {
        if (inHands)
        {
            collider.isTrigger = true;
        }
        else
            collider.isTrigger = false;

        if (GetComponent<ItemProperties>().id == 0)
        {
            if (inHands) 
            {
                transform.position = GameObject.Find("ObjectPos").transform.position;
                rb.useGravity = false;
                rb.velocity = new Vector3(0,0,0);
                transform.rotation = GameObject.Find("ObjectPos").transform.rotation;
            }
            else if (throwObject) 
            {
                rb.AddForce(GameObject.Find("Main Camera").transform.forward * 10f, ForceMode.Impulse);
                //rb.constraints = 0;
                //rb.constraints = RigidbodyConstraints.FreezeRotation;
                rb.useGravity = true;
                throwObject = false;
            }
        }

        if (GetComponent<ItemProperties>().id == 1)
        {
            if (inHands) 
            {
                
                rb.useGravity = false;
                rb.velocity = new Vector3(0,0,0);
                

                if (Input.GetButton("Fire1"))
                {
                    transform.position = GameObject.Find("ObjectPos3").transform.position;
                    transform.rotation = GameObject.Find("ObjectPos3").transform.rotation;
                    extended = true;
                }
                else
                {
                    transform.rotation = GameObject.Find("ObjectPos2").transform.rotation;
                    transform.position = GameObject.Find("ObjectPos2").transform.position;
                    extended = false;
                }
            }
            else if (throwObject) 
            {
                
                rb.AddForce(GameObject.Find("Main Camera").transform.forward * 10f, ForceMode.Impulse);
                //rb.constraints = 0;
                //rb.constraints = RigidbodyConstraints.FreezeRotation;
                rb.useGravity = true;
                throwObject = false;
            }
        }


        
    }

    public void FixedUpdate()
    {
        
    }
}
