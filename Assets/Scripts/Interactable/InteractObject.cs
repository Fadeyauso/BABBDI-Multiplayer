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

    public GameObject secret;

    float timer = 0;

    private bool club;
    private bool climber;
    private bool tinyobject;
    private bool stick;
    private bool moto;


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
            if (GetComponent<ItemProperties>().id == 0 || GetComponent<ItemProperties>().id == 1 || GetComponent<ItemProperties>().id == 2 || GetComponent<ItemProperties>().id == 4|| GetComponent<ItemProperties>().id == 5 || GetComponent<ItemProperties>().id == 6 || GetComponent<ItemProperties>().id == 7 || GetComponent<ItemProperties>().id == 8 || GetComponent<ItemProperties>().id == 9 || GetComponent<ItemProperties>().id == 10 || GetComponent<ItemProperties>().id == 11) 
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
                Destroy(this.gameObject);
            }
        }
        
        
    }

    public override void OnLoseFocus()
    {
        foreach (Material mat in materials)
        {
            if (!inHands) mat.SetFloat("_OutlineWidth", 0);

        }
        GameObject.Find("Player").GetComponent<FirstPersonController>().grabPopup.SetActive(false);

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

        if (inHands) player.GetComponent<FirstPersonController>().inHands = true;
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
                    collider.isTrigger = true;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire1")) && player.GetComponent<FirstPersonController>().canThrow && !player.GetComponent<EnterZone>().inLift) 
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
                    collider.isTrigger = true;
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    rb.velocity = new Vector3(0,0,0);
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)) && player.GetComponent<FirstPersonController>().canThrow && !player.GetComponent<EnterZone>().inLift) 
                        {
                            throwObject = true;
                            inHands = false;
                        }

                    if (Input.GetButton("Fire1"))
                    {
                        transform.position = GameObject.Find("ObjectPos3").transform.position;
                        transform.rotation = GameObject.Find("ObjectPos3").transform.rotation;
                        extended = true;
                        club = false;
                    }
                    else
                    {
                        club = true;
                        WeaponSway(GameObject.Find("ObjectPos2").transform.position);
                        extended = false;
                    }
                }
                else if (throwObject) 
                {
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
            

            if (GetComponent<ItemProperties>().id == 2)
            {
                if (inHands) 
                {
                    collider.isTrigger = true;
                    if (!extended && timer < 0)
                    {
                        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)) && player.GetComponent<FirstPersonController>().canThrow && GetComponent<Climber>().trigger == false && !player.GetComponent<EnterZone>().inLift) 
                        {
                            throwObject = true;
                            inHands = false;
                        }
                    }
                    rb.useGravity = false;
                    rb.isKinematic = true;
                    rb.velocity = new Vector3(0,0,0);
                    
                    if (extended && GameObject.Find("Climber(Clone)").GetComponent<Climber>().trigger)
                    {
                        transform.position = GameObject.Find("Climber(Clone)").GetComponent<Climber>().pickPos;
                        transform.rotation = GameObject.Find("Climber(Clone)").GetComponent<Climber>().pickRot;
                        climber = false;
                    }
                    else if (Input.GetButton("Fire1") && GameObject.Find("Climber(Clone)").GetComponent<Climber>().pick)
                    {
                        transform.position = GameObject.Find("Pickaxe00").transform.position;
                        transform.rotation = GameObject.Find("Pickaxe00").transform.rotation;
                        extended = true;
                        climber = false;
                    }
                    else
                    {
                        climber = true;
                        WeaponSway(GameObject.Find("Pickaxe01").transform.position);
                        extended = false;
                    }
                }
                else if (throwObject) 
                {
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
                    collider.isTrigger = true;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0 && !player.GetComponent<EnterZone>().inLift) 
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
                    collider.isTrigger = true;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0 && !player.GetComponent<EnterZone>().inLift) 
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
                    collider.isTrigger = true;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0 && !player.GetComponent<EnterZone>().inLift) 
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
                    collider.isTrigger = true;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0 && !player.GetComponent<EnterZone>().inLift) 
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
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)) && player.GetComponent<FirstPersonController>().canThrow && !player.GetComponent<EnterZone>().inLift) 
                        {
                            throwObject = true;
                            inHands = false;
                        }

                    if (Input.GetButton("Fire1"))
                    {
                        rb.useGravity = false;
                        transform.position = GameObject.Find("StickPos00").transform.position;
                        extended = true;
                        stick = false;
                    }
                    else
                    {
                        rb.useGravity = false;
                        stick = true;
                        transform.position = GameObject.Find("StickPos01").transform.position;
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
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0 && !player.GetComponent<EnterZone>().inLift) 
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
            

        }

            if (GetComponent<ItemProperties>().id == 11)
            {
                if (inHands) 
                {
                    onMoto = true;
                    collider.isTrigger = true;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F)) && player.GetComponent<FirstPersonController>().canThrow  && timer < 0 && !player.GetComponent<EnterZone>().inLift) 
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
                    onMoto = false;
                    collider.isTrigger = false;
                    transform.SetParent(null);
                    //rb.constraints = 0;
                    //rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.useGravity = true;
                    throwObject = false;
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
            if (GetComponent<ItemProperties>().id == 2 && climber) TiltSway(GameObject.Find("Pickaxe01").transform.localRotation);
            if (GetComponent<ItemProperties>().id == 9 && stick) TiltSway(GameObject.Find("StickPos01").transform.localRotation);
            if (GetComponent<ItemProperties>().id == 9 && !stick) TiltSway(GameObject.Find("StickPos00").transform.localRotation);
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
        InputX = -Input.GetAxis("Mouse X");
        InputY = -Input.GetAxis("Mouse Y");

        float moveX = Mathf.Clamp(InputX * amount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(InputY * amount, -maxAmount, maxAmount);
        
        Vector3 finalPosition = new Vector3(moveX, moveY, 0);

        transform.position = Vector3.Lerp(rb.position, initialPosition, Time.deltaTime * smoothAmount);
        rb.position = Vector3.Lerp(rb.position, initialPosition, Time.deltaTime * smoothAmount);
    }

    public void TiltSway(Quaternion initialRotation)
    {
        float tiltY = Mathf.Clamp(-Input.GetAxis("Mouse X") * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(-Input.GetAxis("Mouse Y") * rotationAmount, -maxRotationAmount, maxRotationAmount);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? tiltX : 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, Time.deltaTime * smoothRotation);
    }

    public void TiltSwayGlobal(Quaternion initialRotation)
    {
        float tiltY = Mathf.Clamp(-Input.GetAxis("Mouse X") * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(-Input.GetAxis("Mouse Y") * rotationAmount, -maxRotationAmount, maxRotationAmount);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? tiltX : 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f));

        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation * initialRotation, Time.deltaTime * smoothRotation);
    }
    
}
