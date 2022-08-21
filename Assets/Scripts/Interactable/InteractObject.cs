using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : Interactable
{
    public bool inHands;
    public bool throwObject;
    public bool extended;

    public Rigidbody rb;

    private float defaultYPos;

    public AudioClip interactionClip;

    public Collider collider;

    float timer = 0;


    public override void OnFocus()
    {
        
    }

    public override void OnInteract()
    {
        SoundManager.Instance.PlaySound(interactionClip);

        if (GetComponent<ItemProperties>().id == 0 || GetComponent<ItemProperties>().id == 1 || GetComponent<ItemProperties>().id == 2 || GetComponent<ItemProperties>().id == 4)
        {
            transform.position = GameObject.Find("ObjectPos").transform.position;
            inHands = !inHands;

            throwObject = true;
            timer = 0.3f;
        
        }

        if (GetComponent<ItemProperties>().id == 3)
        {
            GameManager.secretsFound ++;
            //Debug.Log(GameManager.secretsFound);
            Destroy(this.gameObject);
        }
        
    }

    public override void OnLoseFocus()
    {
        

    }

    public void Awake()
    {
        defaultYPos = transform.position.y;

        collider = GetComponent<Collider>();
    }

    public void Update()
    {
        timer -= Time.deltaTime;

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
            else if (throwObject ) 
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
                if (!extended && timer < 0)
                {
                    if (Input.GetKeyDown(KeyCode.E) && GameObject.Find("Player").GetComponent<FirstPersonController>().currentInteractable == null) 
                    {
                        throwObject = true;
                        inHands = false;
                    }
                }
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

        if (GetComponent<ItemProperties>().id == 2)
        {
            if (inHands) 
            {
                if (!extended && timer < 0)
                {
                    if (Input.GetKeyDown(KeyCode.E) && GameObject.Find("Player").GetComponent<FirstPersonController>().currentInteractable == null && GetComponent<Climber>().trigger == false) 
                    {
                        throwObject = true;
                        inHands = false;
                    }
                }
                rb.useGravity = false;
                rb.velocity = new Vector3(0,0,0);
                
                if (extended && GameObject.Find("Climber").GetComponent<Climber>().trigger)
                {
                    transform.position = transform.position;
                }
                else if (Input.GetButton("Fire1") && GameObject.Find("Climber").GetComponent<Climber>().pick)
                {
                    transform.position = GameObject.Find("Pickaxe00").transform.position;
                    transform.rotation = GameObject.Find("Pickaxe00").transform.rotation;
                    extended = true;
                }
                else
                {
                    transform.rotation = GameObject.Find("Pickaxe01").transform.rotation;
                    transform.position = GameObject.Find("Pickaxe01").transform.position;
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

        if (GetComponent<ItemProperties>().id == 4)
        {
            if (inHands) 
            {
 
                if (Input.GetKeyDown(KeyCode.E) && GameObject.Find("Player").GetComponent<FirstPersonController>().currentInteractable == null) 
                {
                    throwObject = true;
                    inHands = false;
                }
                
                rb.useGravity = false;
                rb.velocity = new Vector3(0,0,0);
                

                transform.rotation = GameObject.Find("LightPos").transform.rotation;
                transform.position = GameObject.Find("LightPos").transform.position;
                
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
