using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float movementMultiplier;
    [SerializeField] float airMultiplier;
    [SerializeField] Transform orientation;

    Vector2 movement;
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    RaycastHit slopeHit;

    [Header("Jump")]
    [SerializeField] float jumpForce;

    [Header("Ground Detection")]
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    float groundDistance = 0.4f;

    [Header("Drag")]
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;

    [Header("Input")]

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    Rigidbody rb;
    float playerHeight = 2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        MapControls();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, groundMask);
        GatherInput();
        ControlDrag();

        if (jumpAction.IsPressed() && isGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void GatherInput()
    {
        movement = moveAction.ReadValue<Vector2>();

        moveDirection = orientation.forward * movement.y + orientation.right * movement.x;
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    void MapControls()
    {
        playerInput = GetComponent<PlayerInput>();
            
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }
}
