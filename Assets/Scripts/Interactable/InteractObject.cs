using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : Interactable
{
    public bool inHands;
    public bool throwObject;
    public bool extended;

    public LayerMask defaultMask;
    public LayerMask inHandsMask;

    private Material[] materials;

    public Rigidbody rb;

    private float defaultYPos;

    public AudioClip interactionClip;

    public Collider collider;

    public GameObject secret;

    float timer = 0;
    private float climbTimer = 0;

    private bool club;
    private bool climber;
    private bool tinyobject;
    private bool stick;
    private bool moto;
    private bool trumpet;


    public override void OnFocus()
    {
        foreach (Material mat in materials)
        {
            if (!inHands) mat.SetFloat("_OutlineWidth", 0.015f);
        }
        if (!inHands) GameObject.Find("Player").GetComponent<FirstPersonController>().grabPopup.SetActive(true);
    }

    public override void OnInteract()
    {
        GameObject.Find("Player").GetComponent<FirstPersonController>().grabPopup.SetActive(false);

        if (!inHands)
        {
            SoundManager.Instance.PlaySound(interactionClip);
            if (GetComponent<ItemProperties>().id == 0 || GetComponent<ItemProperties>().id == 1 || GetComponent<ItemProperties>().id == 2 || GetComponent<ItemProperties>().id == 4|| GetComponent<ItemProperties>().id == 5 || GetComponent<ItemProperties>().id == 6 || GetComponent<ItemProperties>().id == 7 || GetComponent<ItemProperties>().id == 8 || GetComponent<ItemProperties>().id == 9 || GetComponent<ItemProperties>().id == 10 || GetComponent<ItemProperties>().id == 11 || GetComponent<ItemProperties>().id == 13 || GetComponent<ItemProperties>().id == 16 || (GetComponent<ItemProperties>().id == 12 && GameObject.Find("GameManager").GetComponent<GameManager>().haveTicket == 1)) 
            {
            // if ()
                transform.position = GameObject.Find("ObjectPos").transform.position;
                inHands = !inHands;

                if (GetComponent<ItemProperties>().id == 10) Destroy(GameObject.Find("Arrow"));

                throwObject = true;
                timer = 0.3f;

                GameObject.Find("GameManager").GetComponent<GameManager>().pickup = true;
                GameObject.Find("GameManager").GetComponent<GameManager>().item = GetComponent<ItemProperties>().id;
                if (gameObject.tag == "guidon") Destroy(transform.parent.gameObject);
                Destroy(gameObject);
            
            }

            if (GetComponent<ItemProperties>().id == 3)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().secretsFound ++;
                //Debug.Log(GameManager.secretsFound);
                GameObject.Find("GameManager").GetComponent<GameManager>().secretPopup = true;
                Destroy(this.gameObject);
                GameObject.Find("GameManager").GetComponent<GameManager>().secretState[secret.GetComponent<SecretObjectDisplay>().secretId] = 1;
            }
            if (GetComponent<ItemProperties>().id == 50)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().haveTicket = 1;
                if (!GameObject.Find("GameManager").GetComponent<GameManager>().ticket) GameObject.Find("GameManager").GetComponent<GameManager>().Popup();
                GameObject.Find("GameManager").GetComponent<GameManager>().ticket = true;
                GameObject.Find("GameManager").GetComponent<GameManager>().ticketState = 1;
                GameObject.Find("GameManager").GetComponent<GameManager>().lastAchievement = "Babbdi quest";
                SteamIntegration.Instance.UnlockAchivement("BabbdiQuest");
                Destroy(this.gameObject);
            }
            if (GetComponent<ItemProperties>().id == 14)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().haveMap = 1;
                GameObject.Find("GameManager").GetComponent<GameManager>().mapPopup = true;
                Destroy(this.gameObject);
            }
        }
        
        
    }

    public override void OnLoseFocus()
    {
        
        GameObject.Find("Player").GetComponent<FirstPersonController>().grabPopup.SetActive(false);

    }
    private GameObject player;

    public void Awake()
    {
        defaultYPos = transform.position.y;

        collider = GetComponent<Collider>();
        player = GameObject.Find("Player");

        materials = GetComponent<Renderer>().materials;

        defaultMask = gameObject.layer;
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (inHands) player.GetComponent<FirstPersonController>().inHands = true;

        if (inHands && GetComponent<ItemProperties>().id != 11)
        {
            gameObject.layer = 16;
        }
        else gameObject.layer = 7;


        if (player.GetComponent<FirstPersonController>().currentObject != gameObject)
        {
            foreach (Material mat in materials)
            {
                if (!inHands) mat.SetFloat("_OutlineWidth", 0);

            }
        }

        if (throwObject) 
        {
            player.GetComponent<FirstPersonController>().inHands = false;
            GameObject.Find("Player").GetComponent<FirstPersonController>().grabPopup.SetActive(false);
        }
        

        if  (!player.GetComponent<FirstPersonController>().pause)
        {


            if (GetComponent<ItemProperties>().id == 0 || GetComponent<ItemProperties>().id == 7)
            {
                if (inHands) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().ball = 1;
                    collider.isTrigger = true;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Interact")) && player.GetComponent<FirstPersonController>().canThrow && !player.GetComponent<EnterZone>().inLift) 
                    {
                        throwObject = true;
                        inHands = false;
                    }

                    rb.useGravity = false;
                    rb.isKinematic = true;
                    rb.velocity = new Vector3(0,0,0);
                    tinyobject = true;
                    WeaponSway(GameObject.Find("ObjectPos").transform.position);
                    
                }
                else if (throwObject ) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().ball = 0;
                    collider.isTrigger = false;
                    transform.SetParent(null);
                    rb.isKinematic = false;
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
                    GameObject.Find("GameManager").GetComponent<GameManager>().club = 1;
                    collider.isTrigger = true;
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) && player.GetComponent<FirstPersonController>().canThrow && !player.GetComponent<EnterZone>().inLift) 
                        {
                            throwObject = true;
                            inHands = false;
                        }

                    if (Input.GetButton("Fire1"))
                    {
                        transform.position = GameObject.Find("ObjectPos3").transform.position;
                        transform.rotation = GameObject.Find("ObjectPos3").transform.rotation;
                        //rb.position = GameObject.Find("ObjectPos3").transform.position;
                        extended = true;
                        club = false;
                    }
                    else
                    {
                        club = true;
                        ItemMovement(GameObject.Find("ObjectPos2").transform.position, GameObject.Find("ObjectPos2").transform);
                        //rb.position = GameObject.Find("ObjectPos2").transform.position;
                        //TiltSway(GameObject.Find("ObjectPos2").transform.localRotation);
                        extended = false;
                    }
                }
                else if (throwObject) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().club = 0;
                    collider.isTrigger = false;
                    transform.SetParent(null);
                    rb.isKinematic = false;
                    rb.AddForce(GameObject.Find("Main Camera").transform.forward * 10f, ForceMode.Impulse);
                    //rb.constraints = 0;
                    //rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = true;
                    club = false;
                    
                    throwObject = false;
                }
            }
            
            climbTimer -= Time.deltaTime;
            if (GetComponent<ItemProperties>().id == 2)
            {
                if (inHands) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().climber = 1;
                    collider.isTrigger = true;
                    
                    if (!extended && timer < 0)
                    {
                        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) && player.GetComponent<FirstPersonController>().canThrow && GetComponent<Climber>().trigger == false && !player.GetComponent<EnterZone>().inLift) 
                        {
                            throwObject = true;
                            inHands = false;
                        }
                    }
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    
                    if (extended && GetComponent<Climber>().trigger)
                    {
                        transform.position = GetComponent<Climber>().pickPos;
                        transform.rotation = GetComponent<Climber>().pickRot;
                        climber = false;
                    }
                    else if ((Input.GetButton("Fire1")) && GetComponent<Climber>().pick)
                    {
                        transform.position = GameObject.Find("Pickaxe00").transform.position;
                        transform.rotation = GameObject.Find("Pickaxe00").transform.rotation;
                        //rb.position = GameObject.Find("Pickaxe00").transform.position;
                        extended = true;
                        climbTimer = 0.01f;
                        climber = false;
                    }
                    else
                    {
                        climber = true;
                        ItemMovement(GameObject.Find("Pickaxe01").transform.position, GameObject.Find("Pickaxe01").transform);
                        //rb.position = GameObject.Find("Pickaxe01").transform.position;
                        extended = false;
                    }
                }
                else if (throwObject) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().climber = 0;
                    collider.isTrigger = false;
                    transform.SetParent(null);
                    rb.isKinematic = false;
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
                    GameObject.Find("GameManager").GetComponent<GameManager>().flashlight = 1;
                    collider.isTrigger = true;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0 && !player.GetComponent<EnterZone>().inLift) 
                    {
                        throwObject = true;
                        inHands = false;
                    }
                    
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    //rb.velocity = new Vector3(0,0,0);
                    

                    transform.rotation = GameObject.Find("LightPos").transform.rotation;
                    transform.position = GameObject.Find("LightPos").transform.position;
                    
                }
                else if (throwObject) 
                {
                    rb.isKinematic = false;
                    GameObject.Find("GameManager").GetComponent<GameManager>().flashlight = 0;
                    collider.isTrigger = false;
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
                    GameObject.Find("GameManager").GetComponent<GameManager>().propeller = 1;
                    collider.isTrigger = true;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0 && !player.GetComponent<EnterZone>().inLift) 
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
                    GameObject.Find("GameManager").GetComponent<GameManager>().propeller = 0;
                    collider.isTrigger = false;
                    transform.SetParent(null);
                    rb.AddForce(GameObject.Find("Main Camera").transform.forward * 1f, ForceMode.Impulse);
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
                    GameObject.Find("GameManager").GetComponent<GameManager>().blower = 1;
                    collider.isTrigger = true;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0 && !player.GetComponent<EnterZone>().inLift) 
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
                    GameObject.Find("GameManager").GetComponent<GameManager>().blower = 0;
                    collider.isTrigger = false;
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
                    GameObject.Find("GameManager").GetComponent<GameManager>().bigball = 1;
                    collider.isTrigger = true;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0 && !player.GetComponent<EnterZone>().inLift) 
                    {
                        throwObject = true;
                        inHands = false;
                    }
                    
                    rb.useGravity = false;
                    rb.velocity = new Vector3(0,0,0);

                    GameObject.Find("Player").GetComponent<FirstPersonController>().bigball = true;
                    

                    transform.rotation = GameObject.Find("BigBallPos").transform.rotation;
                    transform.position = GameObject.Find("BigBallPos").transform.position;
                    
                }
                else if (throwObject) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().bigball = 0;
                    collider.isTrigger = false;
                    GameObject.Find("Player").GetComponent<FirstPersonController>().bigball = false;
                    transform.SetParent(null);
                    rb.AddForce(GameObject.Find("Main Camera").transform.forward * 10f, ForceMode.Impulse);
                    //rb.constraints = 0;
                    //rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = true;
                    throwObject = false;
                }
            }

            if (GetComponent<ItemProperties>().id == 9)
            {
                if (inHands) 
                {
                    collider.isTrigger = false;
                    rb.useGravity = false;
                    //rb.isKinematic = true;
                    rb.velocity = new Vector3(0,0,0);
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) && player.GetComponent<FirstPersonController>().canThrow && !player.GetComponent<EnterZone>().inLift) 
                        {
                            throwObject = true;
                            inHands = false;
                        }

                    if (Input.GetButton("Fire1") || Input.GetAxis("LeftClick") > 0.1f)
                    {
                        rb.useGravity = false;
                        ItemMovement(GameObject.Find("StickPos00").transform.position, GameObject.Find("StickPos00").transform);
                        extended = true;
                        stick = false;
                    }
                    else
                    {
                        rb.useGravity = false;
                        stick = true;
                        ItemMovement(GameObject.Find("StickPos01").transform.position, GameObject.Find("StickPos01").transform);
                        extended = false;
                    }
                }
                else if (throwObject) 
                {
                    transform.SetParent(null);
                    //rb.isKinematic = false;
                    rb.AddForce(GameObject.Find("Main Camera").transform.forward * 10f, ForceMode.Impulse);
                    //rb.constraints = 0;
                    //rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = true;
                    stick = false;
                    
                    throwObject = false;
                }
            }

            if (GetComponent<ItemProperties>().id == 10)
            {
                if (inHands) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().grabber = 1;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0 && !player.GetComponent<EnterZone>().inLift) 
                    {
                        throwObject = true;
                        inHands = false;
                    }
                    
                    rb.useGravity = false;
                    rb.velocity = new Vector3(0,0,0);
                    
                    transform.rotation = GameObject.Find("GrabberPos").transform.rotation;
                    transform.position = GameObject.Find("GrabberPos").transform.position;
                    
                }
                else if (throwObject) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().grabber = 0;
                    collider.isTrigger = false;
                    GameObject.Find("Player").GetComponent<FirstPersonController>().bigball = false;
                    transform.SetParent(null);
                    rb.AddForce(GameObject.Find("Main Camera").transform.forward * 10f, ForceMode.Impulse);
                    //rb.constraints = 0;
                    //rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = true;
                    throwObject = false;
                }
            }
            if (GetComponent<ItemProperties>().id == 12)
            {
                if (inHands) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().compass = 1;
                    collider.isTrigger = true;
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    //rb.velocity = new Vector3(0,0,0);
                    if (((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) && player.GetComponent<FirstPersonController>().canThrow && !player.GetComponent<EnterZone>().inLift) && GameObject.Find("GameManager").GetComponent<GameManager>().secondPart == 0) 
                        {
                            throwObject = true;
                            inHands = false;
                        }

                    if (Input.GetButton("Fire2") && player.GetComponent<FirstPersonController>().compassTimer < 0)
                    {
                        player.GetComponent<FirstPersonController>().initRotationX = player.GetComponent<FirstPersonController>().playerCamera.transform.rotation;
                        player.GetComponent<FirstPersonController>().initRotationY = player.transform.eulerAngles.y;
                    }

                    if (Input.GetButton("Fire2"))
                    {
                        player.GetComponent<FirstPersonController>().compassLook = true;
                    }
                    else
                    {
                        player.GetComponent<FirstPersonController>().compassLook = false;
                        rb.useGravity = false;
                        stick = true;
                        
                        extended = false;
                    }
                    transform.position = GameObject.Find("CompassPos").transform.position;
                    transform.rotation = GameObject.Find("CompassPos").transform.rotation;
                }
                else if (throwObject && GameObject.Find("GameManager").GetComponent<GameManager>().haveTicket == 1) 
                {
                    player.GetComponent<FirstPersonController>().compassLook = false;
                    GameObject.Find("GameManager").GetComponent<GameManager>().compass = 0;
                    transform.SetParent(null);
                    rb.isKinematic = false;
                    collider.isTrigger = false;
                    rb.AddForce(GameObject.Find("Main Camera").transform.forward * 10f, ForceMode.Impulse);
                    //rb.constraints = 0;
                    //rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = true;
                    stick = false;
                    
                    throwObject = false;
                }
            }

            if (GetComponent<ItemProperties>().id == 11)
            {
                if (inHands) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().motorBike = 1;
                    onMoto = true;
                    collider.isTrigger = true;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0 && !player.GetComponent<EnterZone>().inLift) 
                    {
                        throwObject = true;
                        inHands = false;
                    }
                    
                    rb.useGravity = false;
                    rb.velocity = new Vector3(0,0,0);
                    

                    transform.position = new Vector3(GameObject.Find("MotoPos").transform.position.x, player.transform.position.y, GameObject.Find("MotoPos").transform.position.z);
                    
                }
                else if (throwObject) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().motorBike = 0;
                    onMoto = false;
                    collider.isTrigger = false;
                    transform.SetParent(null);
                    //rb.constraints = 0;
                    //rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = true;
                    throwObject = false;
                }
            }

            if (GetComponent<ItemProperties>().id == 13)
            {
                if (inHands) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().trumpet = 1;
                    trumpet = true;
                    collider.isTrigger = true;
                    rb.useGravity = false;
                    //rb.isKinematic = true;
                    rb.position = GameObject.Find("TrumpetPos00").transform.position;
                    
                    //rb.velocity = new Vector3(0,0,0);
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) && player.GetComponent<FirstPersonController>().canThrow && !player.GetComponent<EnterZone>().inLift) 
                        {
                            throwObject = true;
                            inHands = false;
                        }

                    if (Input.GetButton("Fire1"))
                    {
                        rb.useGravity = false;
                        ItemMovement(GameObject.Find("TrumpetPos00").transform.position, GameObject.Find("TrumpetPos01").transform);
                        //transform.rotation = Quaternion.Euler(GameObject.Find("TrumpetPos00").transform.rotation.x, 0, 0);
                        extended = true;
                    }
                    else
                    {
                        rb.useGravity = false;
                        ItemMovement(GameObject.Find("TrumpetPos01").transform.position, GameObject.Find("TrumpetPos01").transform);
                        //transform.rotation = Quaternion.Euler(GameObject.Find("TrumpetPos01").transform.rotation.x, 0, 0);
                        extended = false;
                    }
                }
                else if (throwObject) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().trumpet = 0;
                    transform.SetParent(null);
                    rb.isKinematic = false;
                    rb.AddForce(GameObject.Find("Main Camera").transform.forward * 10f, ForceMode.Impulse);
                    //rb.constraints = 0;
                    //rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = true;
                    trumpet = false;
                    collider.isTrigger = false;
                    throwObject = false;
                }
            }

            if (GetComponent<ItemProperties>().id == 16)
            {
                if (inHands) 
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().secretfinder = 1;
                    collider.isTrigger = true;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0 && !player.GetComponent<EnterZone>().inLift) 
                    {
                        throwObject = true;
                        inHands = false;
                    }
                    
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    //rb.velocity = new Vector3(0,0,0);
                    

                    transform.rotation = GameObject.Find("SecretFinderPos").transform.rotation;
                    transform.position = GameObject.Find("SecretFinderPos").transform.position;

                    if (Input.GetButton("Fire1") && player.GetComponent<FirstPersonController>().secretTimer < 0 && GameObject.Find("GameManager").GetComponent<GameManager>().secretsFound < 21)
                    {
                        player.GetComponent<FirstPersonController>().initRotationX = player.GetComponent<FirstPersonController>().playerCamera.transform.rotation;
                        player.GetComponent<FirstPersonController>().initRotationY = player.transform.eulerAngles.y;
                    }

                    if (Input.GetButton("Fire1") && GameObject.Find("GameManager").GetComponent<GameManager>().secretsFound < 21)
                    {
                        player.GetComponent<FirstPersonController>().secretLook = true;
                    }
                    else
                    {
                        player.GetComponent<FirstPersonController>().secretLook = false;
                        rb.useGravity = false;
                        stick = true;
                        
                        extended = false;
                    }
                    
                }
                else if (throwObject) 
                {
                    player.GetComponent<FirstPersonController>().secretLook = false;
                    rb.isKinematic = false;
                    GameObject.Find("GameManager").GetComponent<GameManager>().secretfinder = 0;
                    collider.isTrigger = false;
                    transform.SetParent(null);
                    rb.AddForce(GameObject.Find("Main Camera").transform.forward * 5f, ForceMode.Impulse);
                    //rb.constraints = 0;
                    //rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = true;
                    throwObject = false;
                }
            }

            if (inHands)
            {
                if (GameObject.Find("Player").GetComponent<FirstPersonController>().landTimer > 0)
                {
                    //rb.velocity = Vector3.zero;
                }
            }
            

        }

            

        


        
    }
    public bool onMoto;

    void FixedUpdate()
    {
        if (inHands)
        {
            if (tinyobject && (GetComponent<ItemProperties>().id == 0 || tinyobject && GetComponent<ItemProperties>().id == 7)) TiltSway(GameObject.Find("ObjectPos").transform.localRotation);
            if (GetComponent<ItemProperties>().id == 1 && club) TiltSway(GameObject.Find("ObjectPos2").transform.localRotation);
            if (GetComponent<ItemProperties>().id == 13 && trumpet && extended) TiltSway(GameObject.Find("TrumpetPos00").transform.localRotation);
            if (GetComponent<ItemProperties>().id == 13 && trumpet && !extended) TiltSway(GameObject.Find("TrumpetPos01").transform.localRotation);
            if (GetComponent<ItemProperties>().id == 2 && climber) TiltSway(GameObject.Find("Pickaxe01").transform.localRotation);
            if (GetComponent<ItemProperties>().id == 9 && stick) TiltSway(GameObject.Find("StickPos01").transform.localRotation);
            if (GetComponent<ItemProperties>().id == 9 && !stick) transform.rotation = GameObject.Find("StickPos00").transform.rotation;
            if (GetComponent<ItemProperties>().id == 11 && inHands) 
            {
                TiltSway(Quaternion.Euler(0, GameObject.Find("MotoPos").transform.localRotation.y - 90, 0));
            }
        } 
    }


    [Header("Position")]
    public float amount = 0.02f;
    public float maxAmount = 0.06f;
    public float smoothAmount = 6f;

    [Header("Rotation")]
    public float rotationAmount = 4f;
    public float maxRotationAmount = 5f;
    public float smoothRotation = 12f;

    [Space]
    public bool rotationX = true;
    public bool rotationY = true;
    public bool rotationZ = true;



    private float InputX;
    private float InputY;

    public void WeaponSway(Vector3 initialPosition)
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().gamepad)
        {
        InputX = -Input.GetAxis("CameraHorizontal");
        InputY = -Input.GetAxis("CameraVertical");

        float moveX = Mathf.Clamp(InputX * amount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(InputY * amount, -maxAmount, maxAmount);
        
        Vector3 finalPosition = new Vector3(moveX, moveY, 0);

        transform.position = Vector3.Lerp(rb.position, initialPosition, Time.deltaTime * smoothAmount);
        rb.position = Vector3.Lerp(rb.position, initialPosition, Time.fixedDeltaTime * smoothAmount);
        }
        else{
        InputX = -Input.GetAxis("Mouse X");
        InputY = -Input.GetAxis("Mouse Y");

        float moveX = Mathf.Clamp(InputX * amount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(InputY * amount, -maxAmount, maxAmount);
        
        Vector3 finalPosition = new Vector3(moveX, moveY, 0);

        transform.position = Vector3.Lerp(rb.position, initialPosition, Time.deltaTime * smoothAmount);
        rb.position = Vector3.Lerp(rb.position, initialPosition, Time.fixedDeltaTime * smoothAmount);
        }

    }
    
    private float movTimer;

    public void ItemMovement(Vector3 initialPosition, Transform pos)
    {
        movTimer -= Time.deltaTime;
        
        var defaultYPos = initialPosition.y;
        if (player.GetComponent<FirstPersonController>().currentInputRaw != Vector2.zero) {
            transform.position = new Vector3(initialPosition.x, defaultYPos + Mathf.Sin(player.GetComponent<FirstPersonController>().timer) * (player.GetComponent<FirstPersonController>().isCrouching ? player.GetComponent<FirstPersonController>().crouchBobAmount : player.GetComponent<FirstPersonController>().IsSprinting ? player.GetComponent<FirstPersonController>().sprintBobAmount/3 : player.GetComponent<FirstPersonController>().walkBobAmount/2), initialPosition.z);
        }
        else {
            transform.position = initialPosition;
            rb.position = initialPosition;
        }
    }

    public void TiltSway(Quaternion initialRotation)
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().gamepad)
        {
        float tiltY = Mathf.Clamp(-Input.GetAxis("CameraHorizontal") * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(-Input.GetAxis("CameraVertical") * rotationAmount, -maxRotationAmount, maxRotationAmount);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? tiltX : 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, Time.deltaTime * smoothRotation);
        }
        else{
        float tiltY = Mathf.Clamp(-Input.GetAxis("Mouse X") * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(-Input.GetAxis("Mouse Y") * rotationAmount, -maxRotationAmount, maxRotationAmount);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? tiltX : 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, Time.deltaTime * smoothRotation);
        }

    }

    public void TiltSwayGlobal(Quaternion initialRotation)
    {
        float tiltY = Mathf.Clamp(-Input.GetAxis("Mouse X") * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(-Input.GetAxis("Mouse Y") * rotationAmount, -maxRotationAmount, maxRotationAmount);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? tiltX : 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f));

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation * initialRotation, Time.deltaTime * smoothRotation);
    }
    
}
