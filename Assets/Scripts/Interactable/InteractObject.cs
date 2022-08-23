using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : Interactable
{
    public bool inHands;
    public bool throwObject;
    public bool extended;

    private Material[] materials;

    public Rigidbody rb;

    private float defaultYPos;

    public AudioClip interactionClip;

    public Collider collider;

    float timer = 0;


    public override void OnFocus()
    {
        foreach (Material mat in materials)
        {
            //mat.SetColor("_Color", new Color(mat.color.g, mat.color.b, 0.25f));
        }
    }

    public override void OnInteract()
    {
        

        if (!inHands)
        {
            SoundManager.Instance.PlaySound(interactionClip);
            if (GetComponent<ItemProperties>().id == 0 || GetComponent<ItemProperties>().id == 1 || GetComponent<ItemProperties>().id == 2 || GetComponent<ItemProperties>().id == 4|| GetComponent<ItemProperties>().id == 5 || GetComponent<ItemProperties>().id == 6 || GetComponent<ItemProperties>().id == 7 || GetComponent<ItemProperties>().id == 8)
            {
            // if ()
                transform.position = GameObject.Find("ObjectPos").transform.position;
                inHands = !inHands;

                throwObject = true;
                timer = 0.3f;

                GameObject.Find("GameManager").GetComponent<GameManager>().pickup = true;
                GameObject.Find("GameManager").GetComponent<GameManager>().item = GetComponent<ItemProperties>().id;
                Destroy(gameObject);
            
            }

            if (GetComponent<ItemProperties>().id == 3)
            {
                GameManager.secretsFound ++;
                //Debug.Log(GameManager.secretsFound);
                Destroy(this.gameObject);
            }
        }
        
        
    }

    public override void OnLoseFocus()
    {
        foreach (Material mat in materials)
        {
            //mat.SetColor("_Color", new Color(mat.color.g, mat.color.b, 1f));
        }

    }
    private GameObject player;

    public void Awake()
    {
        defaultYPos = transform.position.y;

        collider = GetComponent<Collider>();
        player = GameObject.Find("Player");

        materials = GetComponent<Renderer>().materials;
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if  (!player.GetComponent<FirstPersonController>().pause)
        {
            if (inHands)
            {
                if (GetComponent<ItemProperties>().id != 8) collider.isTrigger = true;
            }
            else
                collider.isTrigger = false;

            if (GetComponent<ItemProperties>().id == 0 || GetComponent<ItemProperties>().id == 7)
            {
                if (inHands) 
                {
                    if (Input.GetKeyDown(KeyCode.E) && player.GetComponent<FirstPersonController>().canThrow) 
                    {
                        throwObject = true;
                        inHands = false;
                    }

                    transform.position = GameObject.Find("ObjectPos").transform.position;
                    rb.useGravity = false;
                    rb.velocity = new Vector3(0,0,0);
                    transform.rotation = GameObject.Find("ObjectPos").transform.rotation;
                }
                else if (throwObject ) 
                {
                    transform.SetParent(null);
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
                    transform.SetParent(null);
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
                        if (Input.GetKeyDown(KeyCode.E) && player.GetComponent<FirstPersonController>().canThrow && GetComponent<Climber>().trigger == false) 
                        {
                            throwObject = true;
                            inHands = false;
                        }
                    }
                    rb.useGravity = false;
                    rb.velocity = new Vector3(0,0,0);
                    
                    if (extended && GameObject.Find("Climber(Clone)").GetComponent<Climber>().trigger)
                    {
                        transform.position = transform.position;
                    }
                    else if (Input.GetButton("Fire1") && GameObject.Find("Climber(Clone)").GetComponent<Climber>().pick)
                    {
                        transform.position = GameObject.Find("Pickaxe00").transform.position;
                        transform.rotation = GameObject.Find("Pickaxe00").transform.rotation;
                        extended = true;
                    }
                    else
                    {
                        transform.SetParent(null);
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
    
                    if (Input.GetKeyDown(KeyCode.E) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0) 
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
                    transform.SetParent(null);
                    rb.AddForce(GameObject.Find("Main Camera").transform.forward * 10f, ForceMode.Impulse);
                    //rb.constraints = 0;
                    //rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = true;
                    throwObject = false;
                }
            }

            if (GetComponent<ItemProperties>().id == 5)
            {
                if (inHands) 
                {
    
                    if (Input.GetKeyDown(KeyCode.E) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0) 
                    {
                        throwObject = true;
                        inHands = false;
                    }
                    
                    rb.useGravity = false;
                    rb.velocity = new Vector3(0,0,0);
                    

                    transform.rotation = GameObject.Find("PropellerPos").transform.rotation;
                    transform.position = GameObject.Find("PropellerPos").transform.position;
                    
                }
                else if (throwObject) 
                {
                    transform.SetParent(null);
                    rb.AddForce(GameObject.Find("Main Camera").transform.forward * 10f, ForceMode.Impulse);
                    //rb.constraints = 0;
                    //rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = true;
                    throwObject = false;
                }
            }

            if (GetComponent<ItemProperties>().id == 6)
            {
                if (inHands) 
                {
    
                    if (Input.GetKeyDown(KeyCode.E) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0) 
                    {
                        throwObject = true;
                        inHands = false;
                    }
                    
                    rb.useGravity = false;
                    rb.velocity = new Vector3(0,0,0);
                    

                    transform.rotation = GameObject.Find("BlowerPos").transform.rotation;
                    transform.position = GameObject.Find("BlowerPos").transform.position;
                    
                }
                else if (throwObject) 
                {
                    transform.SetParent(null);
                    rb.AddForce(GameObject.Find("Main Camera").transform.forward * 10f, ForceMode.Impulse);
                    //rb.constraints = 0;
                    //rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = true;
                    throwObject = false;
                }
            }

            if (GetComponent<ItemProperties>().id == 8)
            {

                if (inHands) 
                {
                    rb.useGravity = false;
                    rb.velocity = new Vector3(0,0,0);
                    

                    if (Input.GetButton("Fire1"))
                    {
                        transform.position = GameObject.Find("PogoPos01").transform.position;
                        transform.rotation = GameObject.Find("PogoPos01").transform.rotation;
                        extended = true;
                    }
                    else
                    {
                        transform.rotation = GameObject.Find("PogoPos00").transform.rotation;
                        transform.position = GameObject.Find("PogoPos00").transform.position;
                        extended = false;
                    }
                }
                else if (throwObject) 
                {
                    transform.SetParent(null);
                    rb.AddForce(GameObject.Find("Main Camera").transform.forward * 10f, ForceMode.Impulse);
                    //rb.constraints = 0;
                    //rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = true;
                    throwObject = false;
                }
                
                
            }
        }

        


        
    }

    public void FixedUpdate()
    {
        
    }
}
