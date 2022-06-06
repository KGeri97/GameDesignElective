using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    //Input
    PlayerInput playerInput;
    InputAction moveAction;
    Vector2 move;
    InputAction lookAction;
    Vector2 look;
    InputAction jump;
    InputAction dash;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float airMultiplier;
    [SerializeField] Transform orientation;
    Vector3 moveDirection;

    [Header("Ground")]
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] float groundDrag;
    [SerializeField] LayerMask groundMask;
    bool grounded;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    bool canJump;

    [Header("Wallrun")]
    [SerializeField] Transform leftWallCheck;
    [SerializeField] Transform rightWallCheck;
    [SerializeField] float wallRunSpeed;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        mapControls();
    }

    private void Update()
    {
        GatherInput();

        isGrounded();
        ControlDrag();

        Jump();
    }

    void FixedUpdate()
    {
        MovePlayer();
        ControlSpeed();
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * move.y + orientation.right * move.x;
        transform.rotation = orientation.rotation;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10 * airMultiplier, ForceMode.Force);
    }

    void Jump()
    {
        if (jump.IsPressed() && canJump && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.y);
            canJump = false;

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
        else if (!jump.IsPressed())
            canJump = true;
    }

    void isGrounded()
    {
        grounded = Physics.OverlapSphere(groundCheckTransform.position, 0.1f, groundMask).Length > 0;
    }

    void ControlDrag()
    {
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    void ControlSpeed()
    {
        Vector3 currentVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (currentVelocity.magnitude > moveSpeed)
        {
            currentVelocity = currentVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);
        }
    }

    void mapControls()
    {
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jump = playerInput.actions["Jump"];
        dash = playerInput.actions["Dash"];
    }

    void GatherInput()
    {
        move = moveAction.ReadValue<Vector2>();
        look = lookAction.ReadValue<Vector2>();
    }
}
