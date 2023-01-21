using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;
    private Rigidbody playerRigidbody;
    private BoxCollider playerBoxCollider;

    [SerializeField]
    private float jumpForce = 5.0f;
    private float turnSpeed = 100.0f;
    public float movementSpeed = 5.0f;
    public float horizontalInput;
    public float forwardInput;

    public bool isWalking;

    // Start is called before the first frame update
    void Start()
    {
        isWalking = false;
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerBoxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ChangeMovementMode();
        }
        HandleMovement();
        HandleJump();
        HandleSound();
    }

    private void HandleMovement()
    {
        float currentSpeed;
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        currentSpeed = movementSpeed;
        if (forwardInput < 0 && !isWalking)
        {
            currentSpeed = movementSpeed / 2;
        }
        if (isWalking)
        {
            playerAnimator.SetFloat("moveSpeed", forwardInput / 2);
        }
        else
        {
            playerAnimator.SetFloat("moveSpeed", forwardInput);
        }

        playerAnimator.SetFloat("turn", horizontalInput);
        // moves the player based on the vertical input
        transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed * forwardInput);


        // rotate player
        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * horizontalInput);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround())
            {
                playerAnimator.SetTrigger("doJump");
                playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void HandleSound()
    {
        // handle walking sound
    }

    private bool isOnGround()
    {
        return transform.Find("GroundCheck").GetComponent<GroundCheck>().isGrounded;
    }

    private void ChangeMovementMode()
    {
        if (isWalking)
        {
            movementSpeed *= 2;
        }
        else
        {
            movementSpeed /= 2;
        }
        isWalking = !isWalking;

    }
}
