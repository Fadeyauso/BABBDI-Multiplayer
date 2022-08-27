using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public bool canMove { get; private set; } = true;
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
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
    public bool throwRay;
    public bool backRay;
    public bool headRay;
    public bool interactionRay;
    public bool upRay;
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

    private float rotationX = 0;

    public static FirstPersonController instance;

    void Awake()
    {
        instance = this;

        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPos = characterController.center.y + characterController.height / 2+ offset.y;
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


        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit prejumpHit, 2f) && Input.GetKeyDown(jumpKey)) prejump = true;
        frontRay = (Physics.Raycast(playerCamera.transform.position, transform.forward, out RaycastHit ss, 4f));
        throwRay = (Physics.Raycast(playerCamera.transform.position, transform.forward, out RaycastHit ssss, 1f));
        backRay = (Physics.Raycast(playerCamera.transform.position, -transform.forward, out RaycastHit sww, 3f));
        headRay = (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit si, 1f));
        interactionRay = (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit interact, interactionDistance, interactionLayer));
        upRay = (Physics.Raycast(playerCamera.transform.position, transform.up, out RaycastHit so, 1.2f));

        clubRay = (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit st, 2.3f));
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
            

            if (canJump) HandleJump();

            if (canCrouch) HandleCrouch();

            if (canUseHeadbob && standing) HandleHeadbob();

            if (useFootsteps) HandleFootsteps();

            if (canInteract)
            {
                HandleInteractionCheck();
                HandleInteractionInput();
            }

            

            

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
                
        }
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2((isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));
        
        float  moveDirectionY = moveDirection.y;
        if (club != null && inHands)
        {
            if (blower.GetComponent<Blower>().isActive && characterController.isGrounded) moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y) * 1.7f;
            else if (blower.GetComponent<Blower>().isActive) moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y) + blower.GetComponent<Blower>().blowMovement;
            else if (!club.GetComponent<Club>().trigger && !propeller.GetComponent<Propeller>().isActive) moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        }
        else moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);

        playerCamera.transform.localPosition = characterController.center + new Vector3(0, characterController.height / 2, 0) + offset;
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
        

        if (isCrouching && upRay);
        else isCrouching = ShouldCrouch;

        if (ShouldCrouch) crouchTimer = 0.3f;
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

    private void HandleHeadbob()
    {
        if (!characterController.isGrounded) return;


        if (landTimer > 0)
        {
            timer += Time.deltaTime * sprintBobSpeed;
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * sprintBobAmount * 2, playerCamera.transform.localPosition.z);
        }
        else if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount), playerCamera.transform.localPosition.z);
        }

        
    }
// 
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

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit2, interactionDistance * 2f, interactionLayer))
        {
            if (hit2.collider.tag == "DontThrow" || throwRay || (isCrouching && headRay))
            {
                canThrow = false;
            }
            else canThrow = true;
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
            moveDirection.y -= gravity * slopeForce * Time.deltaTime;
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
            

        if (upRay) moveDirection.y -= moveDirection.y * Time.deltaTime * 7;

        if (WillSlideOnSlopes && IsSliding)
        {
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }
    
    private void HandleFootsteps()
    {
        if (!characterController.isGrounded) return;
        if (currentInput == Vector2.zero) return;

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



    
    
}
