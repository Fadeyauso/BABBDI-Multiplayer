using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool canMove { get; private set; } = true;
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey) && currentInputRaw != new Vector2(0,0) && !motorBike.GetComponent<MotorBike>().isActive;
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
    private bool ShouldCrouch => (Input.GetKey(crouchKey) || Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.LeftAlt)) && characterController.isGrounded && !motorBike.GetComponent<MotorBike>().isActive;
    private bool ShouldCrouchInAir => (Input.GetKey(crouchKey) || Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.LeftAlt)) && !characterController.isGrounded && !OnSlope();
    private bool FlatSlide => (Input.GetKeyDown(crouchKey) || Input.GetKeyDown(KeyCode.C) || Input.GetKey(KeyCode.LeftAlt)) && characterController.isGrounded && currentInputRaw != Vector2.zero;
    private bool InAirCrouch;   
    public bool preslide;

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool WillSlideOnSlopes = true;
    [SerializeField] private bool useFootsteps = true;
    [SerializeField] private bool canInteract = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

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
    private bool landing;
    public float landTimer;
    public float airTime;
    private float aftervaultjumpTimer;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchingSpeed = 0.3f;
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    private bool isCrouching;
    private float crouchTimer = 0;
    private float headbobEndTimer = 0;
    public bool standing;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.11f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;
    [SerializeField] private float fallBobSpeed = 8f;
    [SerializeField] private float fallBobUpSpeed = 2f;
    [SerializeField] private float fallBobAmount = 0.025f;
    private float defaultYPos = 0;
    private float timer;
    private float fallTimer;

    [Header("Camera Controller")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float zoomFOV = 30f;
    [SerializeField] private float runFOV = 90f;
    [SerializeField] private float tiltAmount = 7f;
    [SerializeField] private float tiltSpeed = 7f;
    [SerializeField] private float compassLookSpeed = 12f;
    [SerializeField] private GameObject targetObject;
    public bool compassLook;
    private bool isZooming;
    private float defaultFOV;

    [Header("Footsteps Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource playerSource = default;
    [SerializeField] private AudioClip[] stoneClips = default;
    [SerializeField] private AudioClip[] woodClips = default;
    [SerializeField] private AudioClip[] grassClips = default;
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
    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    private bool OnSlope()
    {
        if (!characterController.isGrounded) return false;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeForceRayLength))
            if (hit.normal != Vector3.up && Vector3.Angle(hit.normal, Vector3.up) > 15)
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
    private bool inJump;

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
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

    [HideInInspector] public Camera playerCamera;
    [HideInInspector] public CharacterController characterController;

    public Vector3 moveDirection;
    private Vector2 currentInput;
    public Vector2 currentInputRaw;

    public float rotationX = 0;
    public float rotationZ = 0;

    public static FirstPersonController instance;

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

        initialRotation = playerCamera.transform.rotation;
    }


    void Start(){
        
    }

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
        if (Input.GetKeyDown(KeyCode.Escape) && !GameObject.Find("GameManager").GetComponent<GameManager>().endGame)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            firstInterface.SetActive(true);
            library.SetActive(false);
            options.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.Tab) && !GameObject.Find("GameManager").GetComponent<GameManager>().endGame)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            firstInterface.SetActive(false);
            options.SetActive(false);
            library.SetActive(true);
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

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit prejumpHit, 1.3f) && Input.GetKeyDown(jumpKey) && characterController.velocity.y < 0) 
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
        downRay = (Physics.Raycast(transform.position, -transform.up, out RaycastHit down, 0.6f));
        crouchUpRay = (Physics.Raycast(transform.position + transform.up/5, transform.up, out RaycastHit sk, 1.99f));

        clubRay = (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit st, 4f));
        climbRay = (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit sz, 3f));

        dialogueActive = dialogueBox.activeSelf;

        if (!characterController.isGrounded) 
        {
            landing = true;
            airTime -= Time.deltaTime;
        }
        else 
        {
            InAirCrouch = false;
            airTime = 0.3f;
        }
        landTimer -= Time.deltaTime;
        headbobEndTimer -= Time.deltaTime;
        aftervaultjumpTimer -= Time.deltaTime;
        prejumpCancelTimer -= Time.deltaTime;
        
        if (GameObject.Find("GameManager").GetComponent<GameManager>().endGame) canMove = false;
        else if (climber != null) 
        {

            if (pauseMenu.activeSelf == true) canMove = false;
            else if (climber.GetComponent<Climber>().trigger) canMove = false;
            else if (GetComponent<EnterZone>().inLift && GameObject.Find("GameManager").GetComponent<GameManager>().inActivatedLift) canMove = false;
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
                    if (!prejump) SoundManager.Instance.PlaySound(landClip);
                    if (slideTimer > 0 && (!Input.GetKey(crouchKey) && !Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.LeftAlt))) forceFactor = 0;
                }
                
            }

            Debug.DrawRay(transform.position + transform.up, transform.up * 1.99f, Color.green);


            if (characterController.isGrounded) inJump = false;

            if (bigball) currentWeight = ballWeight;
            else currentWeight = 1;
                
        }
        jumpTimer -= Time.deltaTime;
        if (Input.GetKeyDown(jumpKey) && characterController.isGrounded) jumpTimer = 0.3f;
    }
    private float jumpTimer = 0;

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
        if (!shortfrontRay && !characterController.isGrounded && !downRay && vaulRayDown && !vaulRayUp && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.W)))
        {
            vault = true;
            moveDirection.y = 5f;
            aftervaultjumpTimer = 0.3f;
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
        if (Input.GetKey(KeyCode.D) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))) horizontalInput = 0;
        else if (Input.GetKey(KeyCode.D)) horizontalInput = Mathf.Lerp(horizontalInput, 1, 15f * Time.deltaTime);
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q)) horizontalInput = Mathf.Lerp(horizontalInput, -1, 15f * Time.deltaTime);
        else horizontalInput = Mathf.Lerp(horizontalInput, 0, 15f * Time.deltaTime);

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z)) && Input.GetKey(KeyCode.S)) verticalInput = verticalInput = 0;
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z)) verticalInput = Mathf.Lerp(verticalInput, 1, 15f * Time.deltaTime);
        else if (Input.GetKey(KeyCode.S)) verticalInput = Mathf.Lerp(verticalInput, -1, 15f * Time.deltaTime);
        else verticalInput = Mathf.Lerp(verticalInput, 0, 10f * Time.deltaTime);

        if (Input.GetKey(KeyCode.D) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))) horizontalInputRaw = 0;
        else if (Input.GetKey(KeyCode.D)) horizontalInputRaw = 1;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q)) horizontalInputRaw = -1;
        else horizontalInputRaw = 0;

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z)) && Input.GetKey(KeyCode.S)) verticalInputRaw = 0;
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z)) verticalInputRaw = 1;
        else if (Input.GetKey(KeyCode.S)) verticalInputRaw = -1;
        else verticalInputRaw = 0;

        if (motorBike.GetComponent<InteractObject>().onMoto) 
        {
            currentInput = Vector2.zero;
            currentInputRaw = Vector2.zero;
        }
        else
        {
            currentInput = new Vector2((InAirCrouch && inJump ? inairCrouchSpeed : isCrouching && force == Vector3.zero ? crouchSpeed : IsSprinting && (slideTimer < 0 || (!Input.GetKey(crouchKey) && !Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.LeftAlt))) ? sprintSpeed : walkSpeed) * verticalInput, (InAirCrouch ? inairCrouchSpeed : isCrouching && force == Vector3.zero ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * horizontalInput);
            currentInputRaw = new Vector2((InAirCrouch && inJump ? inairCrouchSpeed : isCrouching && force == Vector3.zero ? crouchSpeed : IsSprinting && (slideTimer < 0 || (!Input.GetKey(crouchKey) && !Input.GetKey(KeyCode.C) && !Input.GetKey(KeyCode.LeftAlt))) ? sprintSpeed : walkSpeed) * verticalInputRaw, (InAirCrouch ? inairCrouchSpeed : isCrouching && force == Vector3.zero ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * horizontalInputRaw);
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
            if (blower.GetComponent<Blower>().isActive && characterController.isGrounded) moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y) * 1.7f + forceAdded + motorBike.GetComponent<MotorBike>().motorMovement;
            else if (blower.GetComponent<Blower>().isActive) moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y) + blower.GetComponent<Blower>().blowMovement + forceAdded + motorBike.GetComponent<MotorBike>().motorMovement;
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
    public Quaternion initRotationX;
    public float initRotationY;

    private void HandleMouseLook()
    {
        compassTimer -= Time.deltaTime;

        var targetRotation = Quaternion.LookRotation(targetObject.transform.position - playerCamera.transform.position);

        if (compassLook) {
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
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
            rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, rotationZ);

            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);

            
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
        else if (aftervaultjumpTimer > 0 && Input.GetKeyDown(jumpKey))
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
        else if (Input.GetKeyDown(jumpKey) && characterController.isGrounded && airTime > 0.1f || (airTime > 0.1f && Input.GetKeyDown(jumpKey) && !inJump))
        {
            SoundManager.Instance.PlaySound(jumpClip);
            moveDirection.y = jumpForce;
            inJump = true;
            landing = true;
        } 

    }
    
    private float slideTimer;
    private float tempslideCountdown;
    public float slideCountdown = 1;
    private float keyUpTimer;

    private void HandleCrouch()
    {
        if (!characterController.isGrounded && (Input.GetKeyDown(crouchKey) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftAlt))) preslide = true;
        if (!characterController.isGrounded && (Input.GetKeyUp(crouchKey) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.LeftAlt))) preslide = false;


        slideTimer -= Time.deltaTime;
        keyUpTimer -= Time.deltaTime;

        if (standing && characterController.isGrounded) tempslideCountdown -= Time.deltaTime;
        if (isCrouching && crouchUpRay);
        else 
        {
            isCrouching = ShouldCrouch;
            if (IsSprinting) InAirCrouch = ShouldCrouchInAir;
        }

        if (isCrouching) crouchTimer = 0.35f;

        if ((FlatSlide || (preslide && characterController.isGrounded)) && tempslideCountdown < 0 && verticalInputRaw > 0) 
        {
            preslide = false;
            AddHorizontalForce(transform.forward / 12, IsSprinting ? sprintSlideImpulsion : slideImpulsion);
            deceleration = IsSprinting ? 2f : 3f;
            slideTimer = 3f;
            Debug.Log("cacaca");
            tempslideCountdown = slideCountdown;
            playerSource.Play();
        }
        else if (!characterController.isGrounded && slideTimer > 0)
        {
            playerSource.Stop();
            deceleration = 9;
        } 
        else if ((Input.GetKeyUp(crouchKey) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.LeftAlt)) && characterController.isGrounded) 
        {
            playerSource.Stop();
            slideTimer = 0.1f;
            forceFactor = 0;
            Debug.Log("caca");
        }

        if (((Input.GetKeyUp(crouchKey) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.LeftAlt)) && !characterController.isGrounded)) keyUpTimer = 0.5f;

        
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

        if (playerCamera.fieldOfView != desiredFOV)
        {
            Zoom(desiredFOV);
        }

        


        //Camera Tilt
        var desiredTilt = InAirCrouch && !characterController.isGrounded ? 0 : IsSprinting && standing && horizontalInputRaw == 1 ? -tiltAmount : IsSprinting && standing && horizontalInputRaw == -1 ? tiltAmount : 0;
        SideTilt(desiredTilt);

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
        else if ((currentInputRaw != Vector2.zero ? landTimer > -0.06f : landTimer > 0) && !ShouldCrouch && !InAirCrouch && keyUpTimer < 0)
        {
            
            if (fallTimer < 0.5f) fallTimer += Time.deltaTime * fallBobSpeed;
            else fallTimer += Time.deltaTime * (currentInputRaw != Vector2.zero ? fallBobUpSpeed / 4 : fallBobUpSpeed);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, defaultYPos + Mathf.Sin(fallTimer) * fallBobAmount * 2, playerCamera.transform.localPosition.z);
        }
        else if (currentInputRaw != new Vector2(0,0) && landTimer < -0.25f && characterController.isGrounded)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount), playerCamera.transform.localPosition.z);
        }
        else fallTimer = 0;
        
    }

    public GameObject currentHitObject;

    private void HandleInteractionCheck()
    {

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

        
        if (Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out RaycastHit spherehit0, currentHitDistance,motoLayer) && !motorBike.GetComponent<InteractObject>().onMoto && !dialogueActive)
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
        if ((Input.GetKeyDown(interactKey) || Input.GetKeyDown(KeyCode.F)) && currentInteractable != null && interactionSphere && !dialogueActive)
        {
            currentInteractable.OnInteract();
        }
    }

    private bool jumpSlope = false;
    private bool leaveSlope;

    private void ApplyFinalMovements()
    {
        if ((currentInputRaw.magnitude != 0 || motorBike.GetComponent<InteractObject>().onMoto) && OnSlope() && !Input.GetKey(jumpKey))
        {
            if (Input.GetKeyDown(jumpKey))
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

        if (WillSlideOnSlopes && CrouchSliding && GetComponent<Slope>().surfaceAngle >= 12 && !Input.GetKey(jumpKey))
        {
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * (GetComponent<Slope>().surfaceAngle < 20 ? slopeSlideSpeed * 4 : slopeSlideSpeed) * hitPointNormal.magnitude;
        }
        else if (WillSlideOnSlopes && IsSliding)
        {
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
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
                    case "Footsteps/Wood":
                        SoundManager.Instance.PlaySound(woodClips[Random.Range(0, woodClips.Length - 1)]);
                        break;
                    case "Footsteps/Stone":
                        SoundManager.Instance.PlaySound(stoneClips[Random.Range(0, stoneClips.Length - 1)]);
                        break;
                    case "Footsteps/Grass":
                        SoundManager.Instance.PlaySound(grassClips[Random.Range(0, grassClips.Length - 1)]);
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
    }
    
    
}
