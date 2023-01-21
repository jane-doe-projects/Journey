using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

/*
 * Based on these videos https://www.youtube.com/watch?v=pZBjIyttA4Q .
 * slope slide https://www.youtube.com/watch?v=jIsHe9ARE70
 * https://answers.unity.com/questions/1358491/character-controller-slide-down-slope.html
 * */

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public PlayerInventoryController playerInventoryController;

    public PlayerState state;

    public static CurrentItem currentItem;
    public ActionBarController actionBarController;

    public Transform itemSpawn;

    public Controls controls;
    CharacterController controller;
    PlayerAnimationController playerAnimCont;

    public GearControl gearControl;

    public Vector3 moveDir;

    [SerializeField]
    bool isWalking;
    [SerializeField]
    float jumpRate = 1f;
    float nextJump = 0.0f;

    float currentSpeed;
    public float baseSpeed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 2f;

    public Vector3 velocity;
    public float gravity = -16f;

    // slope handling
    float slopeVelocityFix = -2f;

    // slope sliding
    public bool SlideOnSlopes = true;
    public float slopeSlideSpeed = 8f;

    Vector3 hitPointNormal;


    // tool variables
    bool isSwinging = false;
    float swingTime = 1.5f;


    private bool IsSliding
    {
        get
        {
            if (controller.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > controller.slopeLimit;
            } else
            {
                return false;
            }
        }
    }

    // jumping
    public bool isJumping;
    public float jumpSpeed;
    public float jumpHeight;

    Transform mainCamera;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public bool isGroundedYo;
    Vector3 directionBeforeJump;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerInventoryController = GetComponent<PlayerInventoryController>();
        state = GetComponent<PlayerState>();

        currentItem = new CurrentItem();
    }

    void Start()
    {
        if (actionBarController == null)
        {
            Debug.LogError("ActionBarController not set in PlayerContoller.");
            //UnityEditor.EditorApplication.isPlaying = false;
        }
        mainCamera = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        playerAnimCont = GetComponent<PlayerAnimationController>();

        gearControl = GetComponent<GearControl>();
        currentSpeed = runSpeed;
        isWalking = false;        
    }

    void Update()
    {
        /*
        if (EventSystem.current.IsPointerOverGameObject())
            return; */

        HandleSpeedB();
        HandleMovementB();
        HandleToolUsage();
        isGroundedYo = controller.isGrounded;

    }

    void HandleMovementB()
    {
        // toggle walking on/off
        if (Input.GetKeyDown(controls.toggleWalk))
        {
            isWalking = !isWalking;
            playerAnimCont.WalkingStateSwitch(isWalking);
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (!isGroundedYo && isJumping)
        {
            direction = directionBeforeJump;
        }


        //if (Input.GetKeyDown(controls.jump) && Time.time > nextJump && controller.isGrounded)
        if (Input.GetKeyDown(controls.jump) && Time.time > nextJump && !isJumping)
        {
            directionBeforeJump = direction;
            nextJump = Time.time + jumpRate;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            controller.Move(velocity * Time.deltaTime);
            isJumping = true;
            playerAnimCont.TriggerJump();
            playerAnimCont.SwitchIsInAir(true);
        }

        if (direction.magnitude > 0.1)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // only change direction if grounded (maybe add rotation also)
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            if (isGroundedYo)
            {
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }

            /* slope sliding TODO
            if (SlideOnSlopes && IsSliding)
            {
                moveDir += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSlideSpeed;
            } */

            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        } else
        {
            moveDir = Vector3.zero;
        }

        // add gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // reset grounding velocity
        if (controller.isGrounded)
        {
            if (isJumping)
                isJumping = false;

            velocity.y = slopeVelocityFix; // reset velocity if grounded - set to -2f (slopeVelocityFix) to smoothly go down slopes
        }
    }

 
    void HandleSpeedB()
    {
        if (controller.isGrounded)
        {
            if (isWalking)
            {
                currentSpeed = baseSpeed * PlayerController.Instance.state.speedBuffMultiplier;
            }
            else
            {
                currentSpeed = runSpeed * PlayerController.Instance.state.speedBuffMultiplier;
            }
        }
    }

    void HandleToolUsage()
    {
        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.Locked)
        {
            // change player direction
            
            // swing tool
            SwingTool();
        }

    }

    void SwingTool()
    {
        if (isSwinging)
            return;
        if (gearControl.HasTool())
        {
            // swing tool
            //ToolObject currentTool = gearControl.GetTool();
            isSwinging = true;
            gearControl.Swing();
            StartCoroutine("SwingReset");
            
            
        }
    }


    private void OnTriggerExit(Collider other)
    {
        // close dialog on leaving currentdiloagtarget radius sphere
        if (other.gameObject.CompareTag("QuestGiver"))
            GameManager.Instance.uiState.dialogControl.CloseDialog();

        // close chest ad player ui if player leaves range
        if (other.gameObject.CompareTag("GlobalChest"))
        {
            other.gameObject.GetComponent<Chest>().CloseChest();
            GameManager.Instance.uiState.ChangePlayerUIDisplayState();
        }

    }

    IEnumerator SwingReset()
    {
        new WaitForSeconds(swingTime);
        isSwinging = false;
        yield return null;
    }
}

public class CurrentItem
{
    public static ItemObject item;
}

public class OnActionBar
{
    public static ActionBarSlot slot;
}
