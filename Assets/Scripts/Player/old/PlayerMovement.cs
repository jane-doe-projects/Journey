using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    private float playerSpeed = 10.0f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 playerVelocity;
    private float gravityValue = -19.81f;
    private bool groundedPlayer;
    private float jumpHeight = 1.0f;
    float turnSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = GameObject.Find("Player/GroundCheck").transform;
    }

    // Update is called once per frame
    void Update()
    {
      HandleMovement();
    }

    private void HandleMovement()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        // rotate the player on horizontal input
        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * hInput);

        // move player on vertical input
        Vector3 move = Vector3.forward * vInput;

        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // rotate player
        //transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * hInput);

    }

    bool CheckForGround()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }
}
