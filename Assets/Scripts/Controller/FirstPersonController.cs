using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class FirstPersonController : MonoBehaviour
{
    public bool canMove { get; private set; } = true;
    public bool IsSprinting => canSprint && (Input.GetKey(sprintKey) ||Input.GetAxis("Sprint") > 0.4f) && currentInputRaw != new Vector2(0,0) && !motorBike.GetComponent<MotorBike>().isActive;
    private bool ShouldJump => (Input.GetKeyDown(jumpKey) || Input.GetButtonDown("Jump")) && characterController.isGrounded;
    public bool ShouldCrouch => (Input.GetKey(crouchKey) || Input.GetKey(KeyCode.C)  || Input.GetButton("Slide") || Input.GetKey(KeyCode.LeftAlt)) && characterController.isGrounded && !motorBike.GetComponent<MotorBike>().isActive;
    private bool ShouldCrouchInAir => (Input.GetKey(crouchKey) || Input.GetKey(KeyCode.C) || Input.GetButton("Slide")  || Input.GetKey(KeyCode.LeftAlt)) && !characterController.isGrounded && !OnSlope();
    private bool FlatSlide => (Input.GetKeyDown(crouchKey) || Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Slide") || Input.GetKeyDown(KeyCode.LeftAlt)) && characterController.isGrounded && currentInputRaw != Vector2.zero;
    public bool InAirCrouch;   
    public bool preslide;

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool WillSlideOnSlopes = true;
    [SerializeField] private bool useFootsteps = true;
    [SerializeField] private bool canInteract = true;

    [Header("Fall")]
    [SerializeField] private AudioClip bigImpact = default;
    [SerializeField] private AudioClip smallImpact = default;
    [SerializeField] private AudioClip stoneFall = default;
    [SerializeField] private AudioClip concreteFall = default;
    [SerializeField] private AudioClip metalFall = default;
    [SerializeField] private AudioClip tauleFall = default;
    [SerializeField] private AudioClip grassFall = default;
    [SerializeField] private AudioClip dirtFall = default;
    [SerializeField] private AudioClip sandFall = default;
    [SerializeField] private AudioClip waterFall = default;
    [SerializeField] private AudioClip woodFall = default;
    [SerializeField] private AudioSource fallAudio;
    [SerializeField] private GameObject screenshake;
    private float fallTime;
    [SerializeField] private float maxFallTime = 2f;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private KeyCode sprintKey1 = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey1 = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey1 = KeyCode.LeftControl;
    [SerializeField] private KeyCode interactKey1 = KeyCode.E;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float inairCrouchSpeed = 1f;
    [SerializeField] private float slopeSlideSpeed = 7f;
    [SerializeField] private float slopeSpeed = 8f;
    [SerializeField] private float sprintSlideImpulsion = 5f;
    [SerializeField] private float slideImpulsion = 5f;
    [SerializeField] private float boostPower = 8f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] public float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] public float lookSpeedY = 2.0f;
    [SerializeField] private float upperLookLimit = 90f;
    [SerializeField] private float lowerLookLimit = 90f;
    [SerializeField] private Vector3 offset;
    private Quaternion initialRotation;

    [Header("Jumping Parameters")]
    [SerializeField] public float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;
    public AudioClip jumpClip;
    public AudioClip landClip;
    public bool landing;
    public float landTimer;
    public float airTime;
    private float aftervaultjumpTimer;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchingSpeed = 0.3f;
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    public bool isCrouching;
    private float crouchTimer = 0;
    private float headbobEndTimer = 0;
    public bool standing;

    [Header("Headbob Parameters")]
    [SerializeField] public float walkBobSpeed = 14f;
    [SerializeField] public float walkBobAmount = 0.05f;
    [SerializeField] public float sprintBobSpeed = 18f;
    [SerializeField] public float sprintBobAmount = 0.11f;
    [SerializeField] public float crouchBobSpeed = 8f;
    [SerializeField] public float crouchBobAmount = 0.025f;
    [SerializeField] public float fallBobSpeed = 8f;
    [SerializeField] public float fallBobUpSpeed = 2f;
    [SerializeField] public float fallBobAmount = 0.025f;
    private float defaultYPos = 0;
    public float timer;
    public float fallTimer;

    [Header("Camera Controller")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float zoomFOV = 30f;
    [SerializeField] private float runFOV = 90f;
    [SerializeField] private float tiltAmount = 7f;
    [SerializeField] private float tiltSpeed = 7f;
    [SerializeField] private float compassLookSpeed = 12f;
    [SerializeField] private float smoothCamSpeed = 2f;
    [SerializeField] private GameObject targetObject;
    public bool invertX;
    public bool invertY;
    private int invertXFactor;
    private int invertYFactor;
    public bool compassLook;
    public bool secretLook;
    private bool isZooming;
    private float defaultFOV;
    public bool camController = true;
    public bool smoothCam = false;

    [Header("Footsteps Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource playerSource = default;
    [SerializeField] private AudioClip[] stoneClips = default;
    [SerializeField] private AudioClip[] concreteClips = default;
    [SerializeField] private AudioClip[] metalClips = default;
    [SerializeField] private AudioClip[] tauleClips = default;
    [SerializeField] private AudioClip[] grassClips = default;
    [SerializeField] private AudioClip[] dirtClips = default;
    [SerializeField] private AudioClip[] sandClips = default;
    [SerializeField] private AudioClip[] waterClips = default;
    [SerializeField] private AudioClip[] woodClips = default;
    private float footstepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : IsSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;


    // SLIDING PARAMETERS

    private Vector3 hitPointNormal;

    private bool IsSliding
    {
        get
        {
            if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 3f))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }

            else
                return false;
        }

    }

    private bool CrouchSliding
    {
        get
        {
            if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, characterController.height/2 * slopeForceRayLength) && ShouldCrouch)
            {
                return true;
            }

            else
                return false;
        }

    }
    private bool onFlatGround;

    [Header("Slope Parameters")]
    [SerializeField] public float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    private bool OnSlope()
    {
        if (!characterController.isGrounded) return false;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeForceRayLength))
            if (hit.normal != Vector3.up && Vector3.Angle(hit.normal, Vector3.up) > 4)
                return true;
        return false;
    }

    private bool OnCeilingSlope()
    {
        if (characterController.isGrounded) return false;
        
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, Vector3.up, out hit, 1f))
            if (hit.normal != Vector3.up && Vector3.Angle(hit.normal, Vector3.up) > 10)
                return true;
        return false;
    }

    private bool OnSlopeAir()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeForceRayLength))
            if (hit.normal != Vector3.up && Vector3.Angle(hit.normal, Vector3.up) < 65)
                return true;
        return false;
    }

    private bool OnSteepSlope()
    { 
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeForceRayLength))
            if (hit.normal != Vector3.up && GetComponent<Slope>().uphill && Vector3.Angle(hit.normal, Vector3.up) > 40)
                return true;
        return false;
    }

    private bool prejump;
    private float prejumpCancelTimer;
    public bool inJump;

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private float maxInteractionDistance = default;
    [SerializeField] private float sphereRadius = default;
    [SerializeField] private float maxSphereDistance = default;
    [SerializeField] private float currentHitDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    [SerializeField] private LayerMask allinteractLayer = default;
    [SerializeField] private LayerMask nomotoLayer = default;
    [SerializeField] private LayerMask doorLayer = default;
    [SerializeField] private LayerMask inHandsLayer = default;
    [SerializeField] private LayerMask npcLayer = default;
    [SerializeField] private LayerMask motoLayer = default;
    [SerializeField] private LayerMask defaultLayer = default;
    public Interactable currentInteractable;
    public GameObject currentObject;
    public GameObject dialogueBox;
    public bool dialogueActive;

    [Header("Objects")]
    public GameObject club;
    public GameObject climber;
    public GameObject propeller;
    public GameObject blower;
    public GameObject grabber;
    public GameObject motorBike;
    public bool bigball;
    [SerializeField] private float ballWeight = 0.3f;
    private float currentWeight = 1;
    public bool inHands;
    public bool canThrow;
    public bool frontRay;
    public bool shortfrontRay;
    public bool vaulRayDown;
    public bool vaulRayUp;
    public bool rightRay;
    public bool leftRay;
    public bool throwRay;
    public bool backRay;
    public bool downRay;
    public bool headRay;
    public bool interactionRay;
    public bool interactionSphere;
    public bool upRay;
    public bool crouchUpRay;
    public bool clubRay;
    public bool climbRay;

    [Header("System")]
    public bool pause;
    public GameObject pauseMenu;
    public GameObject firstInterface;
    public GameObject library;
    public GameObject map;
    public GameObject options;
    public GameObject confirmExit;
    public GameObject confirmReset;
    public GameObject confirmReturn;
    public GameObject controls;
    public GameObject talkPopup;
    public GameObject grabPopup;
    public GameObject noticketPopup;
    public GameObject jukeboxPopup;
    public GameObject elevatorPopup;
    public GameObject doorPopup;

    public Camera playerCamera;
    [HideInInspector] public CharacterController characterController;

    public Vector3 moveDirection;
    private Vector2 currentInput;
    public Vector2 currentInputRaw;

    public float rotationX = 0;
    public float rotationZ = 0;

    public static FirstPersonController instance;
    [SerializeField] private GameManager gameManager;

    void Awake()
    {
        instance = this;

        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPos = characterController.center.y + characterController.height / 2 + offset.y;
        defaultFOV = playerCamera.fieldOfView;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        club = GameObject.Find("Club");
        climber = GameObject.Find("Climber");
        propeller = GameObject.Find("Propeller");
        blower = GameObject.Find("Blower");
        grabber = GameObject.Find("Grabber");
        motorBike = GameObject.Find("Moto");
        if (GameObject.Find("GameManager") != null) gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        initialRotation = playerCamera.transform.rotation;
    }



    private void FixedUpdate()
    {
        if(KNetworkManager.instance.localPlayerId!=-1)
        {
               
            var msg = new PlayerPositionSyncMessage();
            msg.playerId = KNetworkManager.instance.localPlayerId;
            msg.position = transform.position;
            msg.rotation = transform.eulerAngles;
            KNetworkManager.instance.messenger.SendGlobalMessage(msg);
        }
    }


    void Start(){
        if (GameObject.Find("SavingOptions").GetComponent<SavingOptions>().mouseInvertedX == 1) invertX = true;
        else invertX = false;
        
        if (GameObject.Find("SavingOptions").GetComponent<SavingOptions>().mouseInvertedY == 1) invertY = true;
        else invertY = false;
    }

    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject selectButton;

    void Update()
    {
        if (dialogueActive) talkPopup.SetActive(false);
        if (IsSliding) onFlatGround = false;
        else onFlatGround = true;
        
        //Pause Menu
        if (pause == true || (GameObject.Find("Outro").GetComponent<Outro>().timer < -4f))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start")) && !gameManager.endGame)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            firstInterface.SetActive(true);
            library.SetActive(false);
            options.SetActive(false);
            confirmExit.SetActive(false);
            confirmReset.SetActive(false);
            confirmReturn.SetActive(false);
            controls.SetActive(false);
            map.SetActive(false);

            if (GameObject.Find("Canvas") != null) GameObject.Find("Canvas").GetComponent<MenuInput>().OnPress();

            if (pause) 
            {
                
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (gameManager.gamepad)
            {
                var eventSystem = EventSystem.current;
                eventSystem.SetSelectedGameObject(startButton, new BaseEventData(eventSystem));
            }
            
            
        }
        if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Select")) && !GameObject.Find("GameManager").GetComponent<GameManager>().endGame)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            firstInterface.SetActive(false);
            options.SetActive(false);
            library.SetActive(true);
            confirmExit.SetActive(false);
            confirmReset.SetActive(false);
            confirmReturn.SetActive(false);
            controls.SetActive(false);
            map.SetActive(false);
            if (pause) 
            {
                
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (GameObject.Find("GameManager").GetComponent<GameManager>().gamepad)
            {
                var eventSystem = EventSystem.current;
                eventSystem.SetSelectedGameObject(selectButton, new BaseEventData(eventSystem));
            }
            
        }
        if (Input.GetKeyDown(KeyCode.M) && !GameObject.Find("GameManager").GetComponent<GameManager>().endGame && GameObject.Find("GameManager").GetComponent<GameManager>().haveMap == 1)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            map.SetActive(true);
            firstInterface.SetActive(false);
            options.SetActive(false);
            library.SetActive(false);
            confirmExit.SetActive(false);
            confirmReset.SetActive(false);
            confirmReturn.SetActive(false);
            controls.SetActive(false);
            if (pause) 
            {
                
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            
        }
        if (pauseMenu.activeSelf) pause = true;
        else pause = false;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit prejumpHit, 1.3f) && (Input.GetKeyDown(jumpKey) || Input.GetButtonDown("Jump")) && characterController.velocity.y < 0) 
        {
            prejump = true;
            prejumpCancelTimer = 0.4f;
        }
        frontRay = (Physics.Raycast(playerCamera.transform.position, transform.forward, out RaycastHit sst, 4f));
        shortfrontRay = (Physics.Raycast(playerCamera.transform.position, transform.forward, out RaycastHit swst, 1f));
        vaulRayDown = (Physics.Raycast(transform.position, transform.forward, out RaycastHit ssq, 1.3f, defaultLayer));
        vaulRayUp = (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit ss, 1.3f, defaultLayer));
        leftRay = (Physics.Raycast(transform.position + Vector3.up, -transform.right, out RaycastHit ssssss, 2f));
        rightRay = (Physics.Raycast(transform.position + Vector3.up, transform.right, out RaycastHit ssssaw, 2f));
        throwRay = (Physics.Raycast(playerCamera.transform.position, transform.forward, out RaycastHit ssss, 1f));
        backRay = (Physics.Raycast(transform.position + Vector3.up, -transform.forward, out RaycastHit sww, 0.9f));
        headRay = (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit si, 1f));
        interactionRay = (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit interact, interactionDistance));
        //interactionSphere = (Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out RaycastHit sphereHit, currentHitDistance));
        upRay = (Physics.Raycast(playerCamera.transform.position, transform.up, out RaycastHit so, 1f));
        downRay = (Physics.Raycast(transform.position, -transform.up, out RaycastHit down, 1.2f));
        crouchUpRay = (Physics.Raycast(transform.position + transform.up/5, transform.up, out RaycastHit sk, 1.99f));

        clubRay = (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit st, 4f));
        climbRay = (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit sz, 3f));

        dialogueActive = dialogueBox.activeSelf;

        if (!characterController.isGrounded) 
        {
            landing = true;
            airTime -= Time.deltaTime;


            if (characterController.velocity.y > 0) fallTime = Mathf.Lerp(fallTime, 0, 6 * Time.deltaTime);
            else if (characterController.velocity.y < 0 && airTime < -0.6f && !pause) fallTime += Time.deltaTime;
        }
        else 
        {
            InAirCrouch = false;
            airTime = 0.3f;
            fallTime = Mathf.Lerp(fallTime, 0, 6 * Time.deltaTime);
        }

        //Handle Fall anim and sound
        fallAudio.volume = Mathf.Lerp(0, 1, fallTime / maxFallTime);

        landTimer -= Time.deltaTime;
        headbobEndTimer -= Time.deltaTime;
        aftervaultjumpTimer -= Time.deltaTime;
        prejumpCancelTimer -= Time.deltaTime;
        
        if (gameManager.gameTime < 0) canMove = false;
        else if (gameManager.endGame) canMove = false;
        else if (climber != null) 
        {

            if (pauseMenu.activeSelf == true) canMove = false;
            else if (climber.GetComponent<Climber>().trigger) canMove = false;
            else if (GetComponent<EnterZone>().inLift && gameManager.inActivatedLift) canMove = false;
            else if (gameManager.bridgeTimer > 0) canMove = false;
            else canMove = !dialogueActive;
        }
        if (climber == null) canMove = !dialogueActive;

        if (canInteract)
            {
                HandleInteractionCheck();
                HandleInteractionInput();
            }
        
        if (!pause && (GameObject.Find("Outro").GetComponent<Outro>().timer > -4f))
        {
            HandleCameraController();
            HandleMouseLook();

        } 
        CalculateMovementInput();
        HandleAddingForce();

        if (canMove)
        {
            HandleMovementInput();
            if (characterController.isGrounded && !motorBike.GetComponent<InteractObject>().onMoto) MovementBoost();
            

            if (canJump) HandleJump();

            if (canCrouch) HandleCrouch();

            if (canUseHeadbob && standing && !motorBike.GetComponent<InteractObject>().onMoto) HandleHeadbob();

            if (useFootsteps && !motorBike.GetComponent<InteractObject>().onMoto) HandleFootsteps();

            

            //Adding Force
            

            

            CheckForVault();



            

            

            ApplyFinalMovements();

            if (characterController.isGrounded && !GetComponent<EnterZone>().inLift)
            {
                moveDirection.y = -0.5f;
                if (landing && airTime < 0)
                {
                    landTimer = 0.2f;
                    landing = false;
                    if (fallTime > 1.5f) {
                        SoundManager.Instance.PlaySound(bigImpact);
                        screenshake.GetComponent<Screenshake>().duration = screenshake.GetComponent<Screenshake>().bigFallDuration;
                        screenshake.GetComponent<Screenshake>().strengthCurve = screenshake.GetComponent<Screenshake>().bigCurve;
                        screenshake.GetComponent<Screenshake>().start = true;
                    }
                    else if (fallTime > 0.8f) {
                        SoundManager.Instance.PlaySound(smallImpact);
                        screenshake.GetComponent<Screenshake>().duration = screenshake.GetComponent<Screenshake>().smallFallDuration;
                        screenshake.GetComponent<Screenshake>().strengthCurve = screenshake.GetComponent<Screenshake>().smallCurve;
                        screenshake.GetComponent<Screenshake>().start = true;
                    }
                    else {
                        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 3f))
                        {
                            //fallTimer = 0;
                            switch(hit.collider.tag)
                            {
                                case "Footsteps/Metal":
                                    SoundManager.Instance.PlaySound(metalFall);
                                    break;
                                case "Footsteps/Taule":
                                    SoundManager.Instance.PlaySound(tauleClips[Random.Range(0, tauleClips.Length - 1)]);
                                    break;
                                case "Footsteps/Wood":
                                    SoundManager.Instance.PlaySound(woodFall);
                                    break;
                                case "Footsteps/Concrete":
                                    SoundManager.Instance.PlaySound(concreteFall);
                                    break;
                                case "Footsteps/Stone":
                                    SoundManager.Instance.PlaySound(stoneFall);
                                    break;
                                case "Footsteps/Grass":
                                    SoundManager.Instance.PlaySound(grassFall);
                                    break;
                                case "Footsteps/Dirt":
                                    SoundManager.Instance.PlaySound(dirtFall);
                                    break;
                                case "Footsteps/Sand":
                                    SoundManager.Instance.PlaySound(sandFall);
                                    break;
                                case "Footsteps/Water":
                                    SoundManager.Instance.PlaySound(waterFall);
                                    break;
                                default:
                                    SoundManager.Instance.PlaySound(stoneFall);
                                    break;
                            }
                        }
                    }
                    if (slideTimer > 0 && (!Input.GetKey(crouchKey) && !Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetButton("Slide"))) forceFactor = 0;
                }
                
            }

            Debug.DrawRay(transform.position + transform.up, transform.up * 1.99f, Color.green);


            if (characterController.isGrounded) inJump = false;

            if (bigball) currentWeight = ballWeight;
            else currentWeight = 1;
                
        }
        jumpTimer -= Time.deltaTime;
        if ((Input.GetKeyDown(jumpKey) || Input.GetButtonDown("Jump")) && characterController.isGrounded) jumpTimer = 0.3f;
    }
    public float jumpTimer = 0;

    private void MovementBoost()
    {
        /*if (IsSprinting)
        {
            if (Input.GetKeyDown(KeyCode.D)) AddHorizontalForce(transform.right, 6f);
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Q)) AddHorizontalForce(-transform.right, 6f);
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z)) AddHorizontalForce(transform.forward, 6f);
            if (Input.GetKeyDown(KeyCode.S)) AddHorizontalForce(-transform.forward, 6f);
        }

        if (Input.GetKey(KeyCode.D))
            if (Input.GetKeyDown(sprintKey) && currentInput != new Vector2(0,0)) AddHorizontalForce(transform.right, 6f);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
            if (Input.GetKeyDown(sprintKey) && currentInput != new Vector2(0,0)) AddHorizontalForce(-transform.right, 6f);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z))
            if (Input.GetKeyDown(sprintKey) && currentInput != new Vector2(0,0)) AddHorizontalForce(transform.forward, 6f);
        if (Input.GetKey(KeyCode.S))
            if (Input.GetKeyDown(sprintKey) && currentInput != new Vector2(0,0)) AddHorizontalForce(-transform.forward, 6f);
        */

        
    }


    public bool vault;

    private void CheckForVault()
    {
        if (!shortfrontRay && !characterController.isGrounded && !downRay && vaulRayDown && !vaulRayUp && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z) || Input.GetAxis("Vertical") > 0.15f))
        {
            vault = true;
            moveDirection.y = 5f;
            aftervaultjumpTimer = 0.5f;
        }
        else if (aftervaultjumpTimer > 0 && downRay && !characterController.isGrounded)
        {
            //aftervaultjumpTimer = 0f;
            moveDirection.y = -1.5f;
            vault = false;
        }
        else vault = false;
        
            
    }

    

    //Custom addforce physics function
    public bool addingForce;
    public Vector3 forceAdded;
    public Vector3 force;
    public float forceFactor;
    public float initforceFactor;
    public float forceSpeed = 4;
    public float tempforceSpeed;
    [Range(0f, 100.0f)] public float deceleration = 1;

    public void AddForce(Vector3 tempforce, float tempforceFactor)
    {
        deceleration = 1;
        moveDirection = new Vector3(0,0,0);
        forceFactor = tempforceFactor;
        force = tempforce;
        moveDirection.y = force.y * forceFactor;
        tempforceSpeed = forceSpeed;
        initforceFactor = tempforceFactor;
    } 

    public void AddAccumulatedForce(Vector3 tempforce, float tempforceFactor)
    {
        moveDirection = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);
        forceFactor = tempforceFactor;
        force = tempforce;
        moveDirection.y = force.y * forceFactor;
        tempforceSpeed = forceSpeed;
        initforceFactor = tempforceFactor;
    }  

    public void AddHorizontalForce(Vector3 tempforce, float tempforceFactor)
    {
        deceleration = 1;
        moveDirection = new Vector3(0, moveDirection.y, 0);
        forceFactor = tempforceFactor;
        force = tempforce;
        tempforceSpeed = forceSpeed;
        initforceFactor = tempforceFactor;
    }
    
    public void AddVerticalForce(Vector3 tempforce, float tempforceFactor)
    {
        deceleration = 1;
        moveDirection = new Vector3(moveDirection.x, -1, moveDirection.z);
        forceFactor = tempforceFactor;
        force = tempforce;
        moveDirection.y = force.y * forceFactor;
        tempforceSpeed = forceSpeed;
        initforceFactor = tempforceFactor;
    }

    private void HandleAddingForce()
    {
        if (initforceFactor > 20f && tempforceSpeed < 10f) tempforceSpeed = 10f;
        else if (tempforceSpeed < 2f) tempforceSpeed = 2f;
        if (!characterController.isGrounded)
        {
            if (forceFactor < 0f) forceFactor = 0f;
            if (forceFactor > 0) tempforceSpeed -= Time.deltaTime * initforceFactor / 2;
        }
        else tempforceSpeed = forceSpeed;

        

        forceFactor -= Time.deltaTime * (tempforceSpeed * deceleration);

        if (forceFactor > 0)
        {
            addingForce = true;
            forceAdded = force * forceFactor;
        }
        else
        {
            addingForce = false;
            forceAdded = new Vector3(0,0,0);
            forceFactor = 0;
            force = new Vector3(0,0,0);
        }

        if (forceFactor <= 1) deceleration = 1;
    }

    private float verticalInput;
    private float horizontalInput;

    private float verticalInputRaw;
    private float horizontalInputRaw;

    private void CalculateMovementInput()
    {
        //keyboard input
        if (Input.GetKey(KeyCode.D) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))) horizontalInput = 0;
        else if (Input.GetKey(KeyCode.D)) horizontalInput = Mathf.Lerp(horizontalInput, 1, 15f * Time.deltaTime);
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q)) horizontalInput = Mathf.Lerp(horizontalInput, -1, 15f * Time.deltaTime);
        else if (Input.GetAxis("horizontalArrows") != 0) horizontalInput = Input.GetAxis("horizontalArrows");
        else if (Input.GetAxis("Horizontal") != 0) horizontalInput = Input.GetAxis("Horizontal");
        else horizontalInput = Mathf.Lerp(horizontalInput, 0, 15f * Time.deltaTime);

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z)) && Input.GetKey(KeyCode.S)) verticalInput = verticalInput = 0;
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z)) verticalInput = Mathf.Lerp(verticalInput, 1, 15f * Time.deltaTime);
        else if (Input.GetKey(KeyCode.S)) verticalInput = Mathf.Lerp(verticalInput, -1, 15f * Time.deltaTime);
        else if (Input.GetAxis("verticalArrows") != 0) verticalInput = Input.GetAxis("verticalArrows");
        else if (Input.GetAxis("Vertical") != 0) verticalInput = Input.GetAxis("Vertical");
        else verticalInput = Mathf.Lerp(verticalInput, 0, 10f * Time.deltaTime);

        if (Input.GetKey(KeyCode.D) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))) horizontalInputRaw = 0;
        else if (Input.GetKey(KeyCode.D)) horizontalInputRaw = 1;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q)) horizontalInputRaw = -1;
        else if (Input.GetAxisRaw("horizontalArrows") != 0) horizontalInputRaw = Input.GetAxisRaw("horizontalArrows");
        else if (Input.GetAxisRaw("Horizontal") != 0) horizontalInputRaw = Input.GetAxisRaw("Horizontal");
        else horizontalInputRaw = 0;

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z)) && Input.GetKey(KeyCode.S)) verticalInputRaw = 0;
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z)) verticalInputRaw = 1;
        else if (Input.GetKey(KeyCode.S)) verticalInputRaw = -1;
        else if (Input.GetAxisRaw("verticalArrows") != 0) verticalInputRaw = Input.GetAxisRaw("verticalArrows");
        else if (Input.GetAxisRaw("Vertical") != 0) verticalInputRaw = Input.GetAxisRaw("Vertical");
        else verticalInputRaw = 0;

        


        if (motorBike.GetComponent<InteractObject>().onMoto) 
        {
            currentInput = Vector2.zero;
            currentInputRaw = Vector2.zero;
        }
        else
        {
            currentInput = new Vector2((InAirCrouch && inJump ? inairCrouchSpeed : isCrouching && force == Vector3.zero ? crouchSpeed : IsSprinting && (slideTimer < 0 || (!Input.GetKey(crouchKey) && !Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetButton("Slide"))) ? sprintSpeed : walkSpeed) * verticalInput, (InAirCrouch ? inairCrouchSpeed : isCrouching && force == Vector3.zero ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * horizontalInput);
            currentInputRaw = new Vector2((InAirCrouch && inJump ? inairCrouchSpeed : isCrouching && force == Vector3.zero ? crouchSpeed : IsSprinting && (slideTimer < 0 || (!Input.GetKey(crouchKey) && !Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetButton("Slide"))) ? sprintSpeed : walkSpeed) * verticalInputRaw, (InAirCrouch ? inairCrouchSpeed : isCrouching && force == Vector3.zero ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * horizontalInputRaw);
        }


        
    }

    private void HandleMovementInput()
    {
        

        
        
        
        float  moveDirectionY = moveDirection.y;

        if (isCrouching && forceAdded != Vector3.zero && characterController.isGrounded)
        {
            moveDirection = forceAdded;
        }
        else if (club != null && inHands)
        {
            if (blower.GetComponent<Blower>().isActive) moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y) + blower.GetComponent<Blower>().blowMovement + forceAdded + motorBike.GetComponent<MotorBike>().motorMovement;
            else if (!propeller.GetComponent<Propeller>().isActive) moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y) + forceAdded + forceAdded + propeller.GetComponent<Propeller>().propellerMovement + motorBike.GetComponent<MotorBike>().motorMovement;
            else moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y) + forceAdded + propeller.GetComponent<Propeller>().propellerMovement + motorBike.GetComponent<MotorBike>().motorMovement;
        }
        else moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y) + forceAdded;
        moveDirection.y = moveDirectionY;
    }


    [Header("Camera mouse incidence")]
    public float rotationAmount = 4f;
    public float maxRotationAmount = 5f;
    public float smoothRotation = 12f;

    public float compassTimer;
    public float secretTimer;
    public Quaternion initRotationX;
    public float initRotationY;

    private void HandleMouseLook()
    {
        compassTimer -= Time.deltaTime;
        secretTimer -= Time.deltaTime;

        if (invertX) invertXFactor = -1;
        else invertXFactor = 1;
        if (invertY) invertYFactor = -1;
        else invertYFactor = 1;

        var targetRotation = Quaternion.LookRotation(targetObject.transform.position - playerCamera.transform.position);
        var secretTargetRotation = Quaternion.LookRotation((GetComponent<SecretFinder>().target != null ? GetComponent<SecretFinder>().target.transform.position - playerCamera.transform.position : GameObject.Find("ObjectPos").transform.position - playerCamera.transform.position));

        if (secretLook) {
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, Quaternion.Euler(secretTargetRotation.eulerAngles.x, transform.eulerAngles.y, 0), compassLookSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z), Quaternion.Euler(0, secretTargetRotation.eulerAngles.y, 0), compassLookSpeed * 0.8f * Time.deltaTime);
            secretTimer = 0.2f;
        }
        else if(secretTimer >= 0)
        {
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, Quaternion.Euler(initRotationX.eulerAngles.x, transform.eulerAngles.y, 0), compassLookSpeed * 1.5f * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, initRotationY, 0), compassLookSpeed * Time.deltaTime);
            rotationZ = Mathf.Lerp(transform.localRotation.z, 0, Time.deltaTime * smoothRotation);
        }
        else if (compassLook) {
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, Quaternion.Euler(targetRotation.eulerAngles.x, transform.eulerAngles.y, 0), compassLookSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z), Quaternion.Euler(0, targetRotation.eulerAngles.y, 0), compassLookSpeed * 0.8f * Time.deltaTime);
            compassTimer = 0.2f;
        }
        else if(compassTimer >= 0)
        {
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, Quaternion.Euler(initRotationX.eulerAngles.x, transform.eulerAngles.y, 0), compassLookSpeed * 1.5f * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, initRotationY, 0), compassLookSpeed * Time.deltaTime);
            rotationZ = Mathf.Lerp(transform.localRotation.z, 0, Time.deltaTime * smoothRotation);
        }
        else
        {
            /*if (smoothCam)
            {
                //Controller look
                rotationX = Mathf.Lerp(rotationX, rotationX - Input.GetAxis("CameraHorizontal") * lookSpeedY, smoothCamSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation*Quaternion.Euler(0, Input.GetAxis("CameraVertical") * lookSpeedX, 0), smoothCamSpeed * Time.deltaTime);

                //Keyboard look
                rotationX -= Mathf.Lerp(rotationX, rotationX - Input.GetAxis("Mouse Y") * lookSpeedY, smoothCamSpeed * Time.deltaTime);
                rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, rotationZ);

                transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation*Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0), smoothCamSpeed * Time.deltaTime);
            }
            else {*/
                //Controller look
                rotationX -= Input.GetAxis("CameraHorizontal") * lookSpeedY * invertYFactor;
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("CameraVertical") * lookSpeedX * invertXFactor, 0);

                //Keyboard look
                rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY * invertYFactor;
                rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, rotationZ);

                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX * invertXFactor, 0);
            
            

            if (standing && playerCamera.transform.localPosition.y != defaultYPos && landTimer < 0 ) //&& currentInputRaw == Vector2.zero)
            {
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, characterController.center + new Vector3(0, characterController.height / 2, 0) + offset, 5f * Time.deltaTime);
                headbobEndTimer = 3f;
            }
            else if (headbobEndTimer < 0 && headbobEndTimer > -0.25f) playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, characterController.center + new Vector3(0, characterController.height / 2, 0) + offset, 8f * Time.deltaTime);
            else if (headbobEndTimer < 0.25f || !standing) playerCamera.transform.localPosition = characterController.center + new Vector3(0, characterController.height / 2, 0) + offset;
        }

        
    }

    private void HandleJump()
    {           
        if (jumpSlope)
        {
            jumpTimer = 0;
            jumpSlope = false;
            SoundManager.Instance.PlaySound(jumpClip);
            moveDirection.y = jumpForce;
            inJump = true;
            landing = true;
        }
        else if (aftervaultjumpTimer > 0 && (Input.GetKeyDown(jumpKey) || Input.GetButtonDown("Jump")))
        {
            SoundManager.Instance.PlaySound(jumpClip);
            moveDirection.y = jumpForce;
            prejump = false;
            landing = true;
            aftervaultjumpTimer = 0;
            inJump = true;
        }
        else if (prejump && characterController.isGrounded && prejumpCancelTimer > 0) 
        {
            SoundManager.Instance.PlaySound(jumpClip);
            moveDirection.y = jumpForce;
            prejump = false;
            landing = true;
            inJump = true;
        }
        else if ((Input.GetKeyDown(jumpKey) || Input.GetButtonDown("Jump")) && characterController.isGrounded && airTime > 0.1f || (airTime > 0.1f && (Input.GetKeyDown(jumpKey) || Input.GetButtonDown("Jump")) && !inJump))
        {

            SoundManager.Instance.PlaySound(jumpClip);
            moveDirection.y = jumpForce;
            inJump = true;
            landing = true;
            
            
        }

    }
    
    public float slideTimer;
    private float tempslideCountdown;
    public float slideCountdown = 1;
    public float keyUpTimer;

    private void HandleCrouch()
    {
        if (!characterController.isGrounded && (Input.GetKeyDown(crouchKey) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetButtonDown("Slide") )) preslide = true;
        if (!characterController.isGrounded && (Input.GetKeyUp(crouchKey) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetButtonUp("Slide") )) preslide = false;


        slideTimer -= Time.deltaTime;
        keyUpTimer -= Time.deltaTime;

        if (standing && characterController.isGrounded) tempslideCountdown -= Time.deltaTime;
        if (isCrouching && crouchUpRay);
        else 
        {
            isCrouching = ShouldCrouch;
            if ((Input.GetKey(crouchKey) || Input.GetKey(KeyCode.C) || Input.GetButton("Slide") || Input.GetKey(KeyCode.LeftAlt)) && !characterController.isGrounded && !OnSlope()) InAirCrouch = true;
            else InAirCrouch = false;
        }

        if (isCrouching) crouchTimer = 0.35f;

        if ((FlatSlide || (preslide && characterController.isGrounded)) && tempslideCountdown < 0 && verticalInputRaw > 0 && ((Input.GetKey(crouchKey) || Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.LeftAlt) || Input.GetButton("Slide") ))) 
        {
            preslide = false;
            AddHorizontalForce(transform.forward / 12, IsSprinting ? sprintSlideImpulsion : slideImpulsion);
            deceleration = IsSprinting ? 2f : 3f;
            slideTimer = 3f;
            tempslideCountdown = slideCountdown;
            playerSource.Play();
        }
        else if (!characterController.isGrounded && slideTimer > 0)
        {
            playerSource.Stop();
            deceleration = 9;
        } 
        else if ((Input.GetKeyUp(crouchKey) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetButtonUp("Slide") ) && characterController.isGrounded) 
        {
            playerSource.Stop();
            slideTimer = 0.1f;
            forceFactor = 0;
        }

        if (((Input.GetKeyUp(crouchKey) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetButtonUp("Slide") ) && !characterController.isGrounded)) keyUpTimer = 0.5f;

        
    }

    void LateUpdate()
    {
        crouchTimer -= Time.deltaTime;

        var desiredHeight = isCrouching || InAirCrouch ? crouchHeight : standHeight;

        if (characterController.height != desiredHeight)
        {
            AdjustHeight(desiredHeight);
        }
        if (crouchTimer < 0 && !ShouldCrouch) standing = true;
        else standing = false;
    }

    private void HandleCameraController()
    {
        if (IsSprinting) isZooming = false;
        else isZooming = Input.GetButton("Fire2");

        var desiredFOV = dialogueActive ? 70 : IsSprinting && standing && verticalInputRaw == 1 ? runFOV : isZooming ? zoomFOV : defaultFOV;

        if (playerCamera.fieldOfView != desiredFOV && !compassLook && compassTimer < 0 && !secretLook && secretTimer < 0)
        {
            Zoom(desiredFOV);
        }

        


        //Camera Tilt
        var desiredTilt = InAirCrouch && !characterController.isGrounded ? 0 : IsSprinting && standing && horizontalInputRaw == 1 ? -tiltAmount : IsSprinting && standing && horizontalInputRaw == -1 ? tiltAmount : 0;
        if (camController) SideTilt(desiredTilt);

        if (rotationZ != desiredTilt)
        {
            
        }
    }

    private void HandleHeadbob()
    {

        if (!characterController.isGrounded && (landTimer > -0.06f || keyUpTimer > 0))
        {
            fallTimer = 0;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, characterController.center + new Vector3(0, characterController.height / 2, 0) + offset, 8f * Time.deltaTime);
        }
        else if (landTimer > 0 && !ShouldCrouch && !InAirCrouch && keyUpTimer < 0)
        {
            if (currentInputRaw != Vector2.zero ? fallTimer < 0.3f : fallTimer < 0.5f) fallTimer += Time.deltaTime * fallBobSpeed;
            else fallTimer += Time.deltaTime * fallBobUpSpeed;
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, defaultYPos + Mathf.Sin(fallTimer) * fallBobAmount * 2, playerCamera.transform.localPosition.z);
        }
        else if (currentInputRaw != new Vector2(0,0) && landTimer < -0.4f && characterController.isGrounded && camController)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount), playerCamera.transform.localPosition.z);
        }
        else fallTimer = 0;

        
    }

    public GameObject currentHitObject;

    private void HandleInteractionCheck()
    {
        //Spherecast size
        if (Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out RaycastHit cacouHit, maxSphereDistance, nomotoLayer) && motorBike.GetComponent<InteractObject>().onMoto)
        {
            currentHitObject = cacouHit.transform.gameObject;
            currentHitDistance = cacouHit.distance + sphereRadius;
        }
        else if (Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out RaycastHit joji, maxSphereDistance, inHandsLayer) && inHands)
        {
            currentHitObject = joji.transform.gameObject;
            currentHitDistance = joji.distance + sphereRadius;
        }
        else if (Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out RaycastHit sphereHit, maxSphereDistance))
        {
            currentHitObject = sphereHit.transform.gameObject;
            currentHitDistance = sphereHit.distance + sphereRadius;
        }
        else{
            currentHitDistance = maxSphereDistance;
            currentHitObject = null;
        }

        //Raycastsize
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit cast, maxInteractionDistance))
        {
            if (cast.transform.gameObject.layer == 7) currentHitObject = cast.transform.gameObject;
            interactionDistance = cast.distance;
        }
        else{
            interactionDistance = maxInteractionDistance;
        }

        //Raycast
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit frontCast0, interactionDistance, motoLayer) && !motorBike.GetComponent<InteractObject>().onMoto && !dialogueActive)
        {
            interactionSphere = true;

                frontCast0.collider.TryGetComponent(out currentInteractable);

                currentObject = frontCast0.collider.gameObject;

                if (currentInteractable)
                    currentInteractable.OnFocus();

     
        }
        else if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit frontCast1, interactionDistance, interactionLayer) && !dialogueActive)
        {
            interactionSphere = true;

                frontCast1.collider.TryGetComponent(out currentInteractable);

                currentObject = frontCast1.collider.gameObject;

                if (currentInteractable)
                    currentInteractable.OnFocus();

                talkPopup.SetActive(false);
                jukeboxPopup.SetActive(false);
                doorPopup.SetActive(false);
            
        }
        else if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit frontCast2, interactionDistance, doorLayer) && !dialogueActive)
        {
            interactionSphere = true;

                frontCast2.collider.TryGetComponent(out currentInteractable);

                currentObject = frontCast2.collider.gameObject;

                if (currentInteractable)
                    currentInteractable.OnFocus();

                talkPopup.SetActive(false);
                grabPopup.SetActive(false);
      
        }
        else if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit frontCast3, interactionDistance, npcLayer) && !dialogueActive)
        {
            interactionSphere = true;

                frontCast3.collider.TryGetComponent(out currentInteractable);

                currentObject = frontCast3.collider.gameObject;

                if (currentInteractable)
                    currentInteractable.OnFocus();

                grabPopup.SetActive(false);
                doorPopup.SetActive(false);
                jukeboxPopup.SetActive(false);
                
        }
        //Spherecast
        else if (Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out RaycastHit spherehit0, currentHitDistance,motoLayer) && !motorBike.GetComponent<InteractObject>().onMoto && !dialogueActive)
        {
            interactionSphere = true;

                spherehit0.collider.TryGetComponent(out currentInteractable);

                currentObject = spherehit0.collider.gameObject;

                if (currentInteractable)
                    currentInteractable.OnFocus();

     
        }
        else if (Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out RaycastHit spherehit, currentHitDistance,interactionLayer) && !dialogueActive)
        {
            interactionSphere = true;

                spherehit.collider.TryGetComponent(out currentInteractable);

                currentObject = spherehit.collider.gameObject;

                if (currentInteractable)
                    currentInteractable.OnFocus();

                talkPopup.SetActive(false);
                jukeboxPopup.SetActive(false);
                doorPopup.SetActive(false);
            
        }
        else if (Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out RaycastHit spherehit1, currentHitDistance,doorLayer) && !dialogueActive)
        {
            interactionSphere = true;

                spherehit1.collider.TryGetComponent(out currentInteractable);

                currentObject = spherehit1.collider.gameObject;

                if (currentInteractable)
                    currentInteractable.OnFocus();

                talkPopup.SetActive(false);
                grabPopup.SetActive(false);
      
        }
        else if (Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out RaycastHit spherehit2, currentHitDistance,npcLayer) && !dialogueActive)
        {
            interactionSphere = true;

                spherehit2.collider.TryGetComponent(out currentInteractable);

                currentObject = spherehit2.collider.gameObject;

                if (currentInteractable)
                    currentInteractable.OnFocus();

                grabPopup.SetActive(false);
                doorPopup.SetActive(false);
                jukeboxPopup.SetActive(false);
                
        }
        else if (currentInteractable)
        {
            interactionSphere = false;
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
            currentObject = null;
            noticketPopup.SetActive(false);
            grabPopup.SetActive(false);
            talkPopup.SetActive(false);
            jukeboxPopup.SetActive(false);
            elevatorPopup.SetActive(false);
            doorPopup.SetActive(false);
        }

        if (currentInteractable == null)
        {
            interactionSphere = false;
            noticketPopup.SetActive(false);
            grabPopup.SetActive(false);
            talkPopup.SetActive(false);
            jukeboxPopup.SetActive(false);
            elevatorPopup.SetActive(false);
            doorPopup.SetActive(false);
        }

        if (dialogueActive) canThrow = false;
        else if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit dontThrowCast, interactionDistance, allinteractLayer))
        {
            
            if (dontThrowCast.collider.tag == "DontThrow")
            {
                canThrow = false;
            }
            else if ((isCrouching && headRay) || throwRay) canThrow = false;
            else canThrow = true;
        }
        else if (Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out RaycastHit hit2, currentHitDistance, allinteractLayer))
        {
            
            if (hit2.collider.tag == "DontThrow")
            {
                canThrow = false;
            }
            else if ((isCrouching && headRay) || throwRay) canThrow = false;
            else canThrow = true;
        }
        else if ((isCrouching && headRay) || throwRay)
        {
            canThrow = false;
        }
        else canThrow = true;
    }

    private void HandleInteractionInput()
    {
        if ((Input.GetKeyDown(interactKey) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) && currentInteractable != null && interactionSphere && !dialogueActive)
        {
            currentInteractable.OnInteract();
        }
    }

    public bool jumpSlope = false;
    private bool leaveSlope;

    private void ApplyFinalMovements()
    {
        if ((currentInputRaw.magnitude != 0 || motorBike.GetComponent<InteractObject>().onMoto) && OnSlope() && (GameObject.Find("GameManager").GetComponent<GameManager>().gamepad ? !Input.GetButton("Jump") : !Input.GetKey(jumpKey)))
        {
            if (Input.GetKeyDown(jumpKey)  || Input.GetButtonDown("Jump"))
            {
                moveDirection.y = 0;
                jumpSlope = true;
            } 
            else jumpSlope = false;
            if (!jumpSlope)
            {
                moveDirection.y -= gravity * slopeForce * Time.deltaTime;
            }
            leaveSlope = true;
        }
        else if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;

        }

        

        

        if (backRay && characterController.velocity.magnitude > 30)
        {
            moveDirection.x /= 4;
            moveDirection.z /= 4;
            force.x = 0;
            force.z = 0;
        }
            

        if (upRay && !characterController.isGrounded) AddVerticalForce(new Vector3(0, -1, 0), 2f);
        if (OnCeilingSlope()){
            AddVerticalForce(new Vector3(0, -1, 0), IsSprinting ? -8f : -5f);
            moveDirection.y = IsSprinting ? -8f : -5f;
        }

        if (WillSlideOnSlopes && CrouchSliding && GetComponent<Slope>().surfaceAngle >= 12 && (!Input.GetKey(jumpKey) && !Input.GetButton("Jump")))
        {
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * (slopeSlideSpeed) * hitPointNormal.magnitude;
            moveDirection.y -= 800 * Time.deltaTime;
        }
        

        characterController.Move(moveDirection * Time.deltaTime * currentWeight);
    }
    
    private void HandleFootsteps()
    {
        if (!characterController.isGrounded) return;
        if (currentInputRaw == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0 && !isCrouching && currentInputRaw != Vector2.zero)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 3f))
            {
                switch(hit.collider.tag)
                {
                    case "Footsteps/Metal":
                        SoundManager.Instance.PlaySound(metalClips[Random.Range(0, metalClips.Length - 1)]);
                        break;
                    case "Footsteps/Taule":
                        SoundManager.Instance.PlaySound(tauleClips[Random.Range(0, tauleClips.Length - 1)]);
                        break;
                    case "Footsteps/Wood":
                        SoundManager.Instance.PlaySound(woodClips[Random.Range(0, woodClips.Length - 1)]);
                        break;
                    case "Footsteps/Concrete":
                        SoundManager.Instance.PlaySound(concreteClips[Random.Range(0, concreteClips.Length - 1)]);
                        break;
                    case "Footsteps/Stone":
                        SoundManager.Instance.PlaySound(stoneClips[Random.Range(0, stoneClips.Length - 1)]);
                        break;
                    case "Footsteps/Grass":
                        SoundManager.Instance.PlaySound(grassClips[Random.Range(0, grassClips.Length - 1)]);
                        break;
                    case "Footsteps/Dirt":
                        SoundManager.Instance.PlaySound(dirtClips[Random.Range(0, dirtClips.Length - 1)]);
                        break;
                    case "Footsteps/Sand":
                        SoundManager.Instance.PlaySound(sandClips[Random.Range(0, sandClips.Length - 1)]);
                        break;
                    case "Footsteps/Water":
                        SoundManager.Instance.PlaySound(waterClips[Random.Range(0, waterClips.Length - 1)]);
                        break;
                    default:
                        SoundManager.Instance.PlaySound(stoneClips[Random.Range(0, stoneClips.Length - 1)]);
                        break;
                }
            }
            
            footstepTimer = GetCurrentOffset;
        }
    }

    private void AdjustHeight(float height)
    {
        float center = height / 2;

        characterController.height = Mathf.Lerp(characterController.height, height, crouchingSpeed * Time.deltaTime);
        characterController.center = Vector3.Lerp(characterController.center, new Vector3(0, center, 0), crouchingSpeed * Time.deltaTime);
    }

    private void Zoom(float fov)
    {
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, zoomSpeed * Time.deltaTime );
    }

    private void SideTilt(float temptiltAmount)
    {
        //Mouse Movement Incidence
        float tiltY = Mathf.Clamp(-Input.GetAxis("Mouse X") * rotationAmount, -maxRotationAmount, maxRotationAmount);

        rotationZ = Mathf.Lerp(rotationZ, temptiltAmount, tiltSpeed * Time.deltaTime) + Mathf.Lerp(transform.localRotation.z, tiltY, Time.deltaTime * smoothRotation);
    }


    private void OnDrawGizmos()
    
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * currentHitDistance);
        Gizmos.DrawWireSphere(playerCamera.transform.position + playerCamera.transform.forward * currentHitDistance, sphereRadius);
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionDistance, Color.green);
    }
    
    
}
