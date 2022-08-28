using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool canMove { get; private set; } = true;
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey) && currentInputRaw != new Vector2(0,0);
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
    private bool ShouldCrouch => Input.GetKey(crouchKey) && characterController.isGrounded;

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
    [SerializeField] private float slopeSpeed = 8f;
    [SerializeField] private float boostPower = 8f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 10)] private float upperLookLimit = 90f;
    [SerializeField, Range(1, 10)] private float lowerLookLimit = 90f;
    [SerializeField] private Vector3 offset;

    [Header("Jumping Parameters")]
    [SerializeField] public float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;
    public AudioClip jumpClip;
    public AudioClip landClip;
    private bool landing;
    public float landTimer;
    public float airTime;

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
    private float defaultYPos = 0;
    private float timer;

    [Header("Camera Controller")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float zoomFOV = 30f;
    [SerializeField] private float runFOV = 90f;
    [SerializeField] private float tiltAmount = 7f;
    [SerializeField] private float tiltSpeed = 7f;
    private bool isZooming;
    private float defaultFOV;

    [Header("Footsteps Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
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

    [Header("Slope Parameters")]
    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    private bool OnSlope()
    {
        if (!characterController.isGrounded) return false;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, characterController.height/2 * slopeForceRayLength))
            if (hit.normal != Vector3.up && GetComponent<Slope>().downhill && Vector3.Angle(hit.normal, Vector3.up) > 10)
                return true;
        return false;
    }

    private bool prejump;

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    public Interactable currentInteractable;
    public GameObject currentObject;
    public GameObject dialogueBox;
    public bool dialogueActive;

    [Header("Objects")]
    public GameObject club;
    public GameObject climber;
    public GameObject propeller;
    public GameObject blower;
    public bool inHands;
    public bool canThrow;
    public bool frontRay;
    public bool rightRay;
    public bool leftRay;
    public bool throwRay;
    public bool backRay;
    public bool headRay;
    public bool interactionRay;
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

    [HideInInspector] public Camera playerCamera;
    [HideInInspector] public CharacterController characterController;

    public Vector3 moveDirection;
    private Vector2 currentInput;
    private Vector2 currentInputRaw;

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
    }

    void Start(){
        
    }

    void Update()
    {
        //Pause Menu
        if (pauseMenu.activeSelf == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            firstInterface.SetActive(true);
            library.SetActive(false);
            options.SetActive(false);
            if (pause) 
            {
                
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            firstInterface.SetActive(false);
            options.SetActive(false);
            library.SetActive(true);
            if (pause) 
            {
                
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            
        }
        pause = pauseMenu.activeSelf;


        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit prejumpHit, 1f) && Input.GetKeyDown(jumpKey)) prejump = true;
        frontRay = (Physics.Raycast(playerCamera.transform.position, transform.forward, out RaycastHit ss, 4f));
        leftRay = (Physics.Raycast(playerCamera.transform.position, -transform.right, out RaycastHit ssssss, 2f));
        rightRay = (Physics.Raycast(playerCamera.transform.position, transform.right, out RaycastHit ssssaw, 2f));
        throwRay = (Physics.Raycast(playerCamera.transform.position, transform.forward, out RaycastHit ssss, 1f));
        backRay = (Physics.Raycast(playerCamera.transform.position, -transform.forward, out RaycastHit sww, 0.9f));
        headRay = (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit si, 1f));
        interactionRay = (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit interact, interactionDistance, interactionLayer));
        upRay = (Physics.Raycast(playerCamera.transform.position, transform.up, out RaycastHit so, 1f));
        crouchUpRay = (Physics.Raycast(playerCamera.transform.position, transform.up, out RaycastHit sk, 2f));

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
            airTime = 0.3f;
        }
        landTimer -= Time.deltaTime;
        headbobEndTimer -= Time.deltaTime;
        

        if (climber != null) 
        {

            if (pauseMenu.activeSelf == true) canMove = false;
            else if (climber.GetComponent<Climber>().trigger) canMove = false;
            else if (GetComponent<EnterZone>().inLift && GameObject.Find("GameManager").GetComponent<GameManager>().inActivatedLift) canMove = false;
            else canMove = !dialogueActive;
        }
        if (climber == null) canMove = !dialogueActive;
        
        if (!pause) HandleMouseLook();

        if (canMove)
        {
            HandleMovementInput();
            if (characterController.isGrounded) MovementBoost();
            

            if (canJump) HandleJump();

            if (canCrouch) HandleCrouch();

            if (canUseHeadbob && standing) HandleHeadbob();

            if (useFootsteps) HandleFootsteps();

            if (canInteract)
            {
                HandleInteractionCheck();
                HandleInteractionInput();
            }

            //Adding Force
            HandleAddingForce();

            HandleCameraController();

            

            

            ApplyFinalMovements();

            if (characterController.isGrounded && !GetComponent<EnterZone>().inLift)
            {
                moveDirection.y = -0.5f;
                if (landing && airTime < 0)
                {
                    landTimer = 0.2f;
                    landing = false;
                    if (!prejump) SoundManager.Instance.PlaySound(landClip);
                }
                
            }

            Debug.DrawRay(playerCamera.transform.position, transform.up * 2f, Color.green);
                
        }
    }

    private void MovementBoost()
    {
        if (IsSprinting)
        {
            if (Input.GetKeyDown(KeyCode.D)) AddForce(transform.right, 6f);
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Q)) AddForce(-transform.right, 6f);
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z)) AddForce(transform.forward, 6f);
            if (Input.GetKeyDown(KeyCode.S)) AddForce(-transform.forward, 6f);
        }

        if (Input.GetKey(KeyCode.D))
            if (Input.GetKeyDown(sprintKey) && currentInput != new Vector2(0,0)) AddForce(transform.right, 6f);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
            if (Input.GetKeyDown(sprintKey) && currentInput != new Vector2(0,0)) AddForce(-transform.right, 6f);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z))
            if (Input.GetKeyDown(sprintKey) && currentInput != new Vector2(0,0)) AddForce(transform.forward, 6f);
        if (Input.GetKey(KeyCode.S))
            if (Input.GetKeyDown(sprintKey) && currentInput != new Vector2(0,0)) AddForce(-transform.forward, 6f);
        

        
    }

    

    //Custom addforce physics function
    public bool addingForce;
    public Vector3 forceAdded;
    public Vector3 force;
    public float forceFactor;
    public float initforceFactor;
    public float forceSpeed = 4;
    public float tempforceSpeed;

    public void AddForce(Vector3 tempforce, float tempforceFactor)
    {
        moveDirection = new Vector3(0,0,0);
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
            if (forceFactor < 0.8f) forceFactor = 0.8f;
            if (forceFactor > 0) tempforceSpeed -= Time.deltaTime * initforceFactor / 2;
        }
        else tempforceSpeed = forceSpeed;

        

        forceFactor -= Time.deltaTime * tempforceSpeed;

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
    }

    private float verticalInput;
    private float horizontalInput;

    private float verticalInputRaw;
    private float horizontalInputRaw;

    private void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.D)) horizontalInput = Mathf.Lerp(horizontalInput, 1, 15f * Time.deltaTime);
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q)) horizontalInput = Mathf.Lerp(horizontalInput, -1, 15f * Time.deltaTime);
        else horizontalInput = Mathf.Lerp(horizontalInput, 0, 15f * Time.deltaTime);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z)) verticalInput = Mathf.Lerp(verticalInput, 1, 15f * Time.deltaTime);
        else if (Input.GetKey(KeyCode.S)) verticalInput = Mathf.Lerp(verticalInput, -1, 15f * Time.deltaTime);
        else verticalInput = Mathf.Lerp(verticalInput, 0, 10f * Time.deltaTime);

        if (Input.GetKey(KeyCode.D)) horizontalInputRaw = 1;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q)) horizontalInputRaw = -1;
        else horizontalInputRaw = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z)) verticalInputRaw = 1;
        else if (Input.GetKey(KeyCode.S)) verticalInputRaw = -1;
        else verticalInputRaw = 0;

        currentInput = new Vector2((isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * verticalInput, (isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * horizontalInput);
        currentInputRaw = new Vector2((isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * verticalInputRaw, (isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * horizontalInputRaw);
        
        float  moveDirectionY = moveDirection.y;
        if (club != null && inHands)
        {
            if (blower.GetComponent<Blower>().isActive && characterController.isGrounded) moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y) * 1.7f + forceAdded;
            else if (blower.GetComponent<Blower>().isActive) moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y) + blower.GetComponent<Blower>().blowMovement + forceAdded;
            else if (!propeller.GetComponent<Propeller>().isActive) moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y) + forceAdded;
            else moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y) + forceAdded;
        }
        else moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y) + forceAdded;
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, rotationZ);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);

        if (standing && playerCamera.transform.localPosition.y != defaultYPos && landTimer < 0 && currentInputRaw != new Vector2(0,0))
        {
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, characterController.center + new Vector3(0, characterController.height / 2, 0) + offset, 10f * Time.deltaTime);
            headbobEndTimer = 3f;
        }
        else if (headbobEndTimer < 0 && headbobEndTimer > -0.25f) playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, characterController.center + new Vector3(0, characterController.height / 2, 0) + offset, 8f * Time.deltaTime);
        else if (headbobEndTimer < 0.25f || !standing) playerCamera.transform.localPosition = characterController.center + new Vector3(0, characterController.height / 2, 0) + offset;
    }

    private void HandleJump()
    {
        if (ShouldJump)
        {
            SoundManager.Instance.PlaySound(jumpClip);
            moveDirection.y = jumpForce;
            
            landing = true;
        } 
        if (prejump && characterController.isGrounded) 
        {
            SoundManager.Instance.PlaySound(jumpClip);
            moveDirection.y = jumpForce;
            prejump = false;
            landing = true;
        }
        
    }

    private void HandleCrouch()
    {
        

        if (isCrouching && crouchUpRay);
        else isCrouching = ShouldCrouch;

        if (isCrouching) crouchTimer = 0.35f;
    }

    void LateUpdate()
    {
        crouchTimer -= Time.deltaTime;

        var desiredHeight = isCrouching ? crouchHeight : standHeight;

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
        else isZooming = Input.GetKey(KeyCode.C);

        var desiredFOV = IsSprinting && standing && verticalInputRaw == 1 ? runFOV : isZooming ? zoomFOV : defaultFOV;

        if (playerCamera.fieldOfView != desiredFOV)
        {
            Zoom(desiredFOV);
        }


        //Camera Tilt
        var desiredTilt = IsSprinting && standing && horizontalInputRaw == 1 ? -tiltAmount : IsSprinting && standing && horizontalInputRaw == -1 ? tiltAmount : 0;

        if (rotationZ != desiredTilt)
        {
            SideTilt(desiredTilt);
        }
    }

    private void HandleHeadbob()
    {
        if (!characterController.isGrounded) return;


        if (landTimer > 0)
        {
            timer += Time.deltaTime * sprintBobSpeed;
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * sprintBobAmount * 2, playerCamera.transform.localPosition.z);
        }
        else if (currentInputRaw != new Vector2(0,0))
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount), playerCamera.transform.localPosition.z);
        }
        
    }

    private void HandleInteractionCheck()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            if (hit.collider.gameObject.layer == 7)
            {
                hit.collider.TryGetComponent(out currentInteractable);

                currentObject = hit.collider.gameObject;

                if (currentInteractable)
                    currentInteractable.OnFocus();
            }

            
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit2, interactionDistance * 2f))
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
        if ((Input.GetKeyDown(interactKey) || Input.GetKeyDown(KeyCode.F)) && currentInteractable != null && interactionRay)
        {
            currentInteractable.OnInteract();
        }
    }

    private bool jumpSlope = false;

    private void ApplyFinalMovements()
    {
        if (currentInput.magnitude != 0 && OnSlope())
        {
            moveDirection.y -= gravity * Time.deltaTime;
            moveDirection.y -= slopeForce * Time.deltaTime;
            if (ShouldJump)
            {
                moveDirection.y = 0;
                jumpSlope = true;
            } 
        }
        else if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
            if (jumpSlope)
            {
                jumpSlope = false;
                moveDirection.y = jumpForce;
            }
        }

        if (backRay && forceFactor > 0)
        {
            force.x = 0;
            force.z = 0;
        }
            

        if (upRay && !characterController.isGrounded) AddForce(new Vector3(0, -1, 0), 2f);

        if (WillSlideOnSlopes && IsSliding)
        {
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }
    
    private void HandleFootsteps()
    {
        if (!characterController.isGrounded) return;
        if (currentInputRaw == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3f))
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
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, zoomSpeed * Time.deltaTime);
    }

    private void SideTilt(float temptiltAmount)
    {
        rotationZ = Mathf.Lerp(rotationZ, temptiltAmount, tiltSpeed * Time.deltaTime);
    }



    
    
}
