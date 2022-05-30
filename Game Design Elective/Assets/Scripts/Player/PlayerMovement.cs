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

    Vector2 movement;
    Vector3 moveDirection;

    [Header("Jump")]

    [SerializeField] float jumpForce;
    bool isGrounded;

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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.1f);
        GatherInput();
        ControlDrag();

        if (jumpAction.IsPressed() && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void GatherInput()
    {
        movement = moveAction.ReadValue<Vector2>();

        moveDirection = transform.forward * movement.y + transform.right * movement.x;
    }

    void MovePlayer()
    {
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
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
