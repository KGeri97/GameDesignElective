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
    [SerializeField] float extraGravity;
    bool canJump;

    [Header("Wallrun")]
    [SerializeField] Transform leftWallCheck;
    [SerializeField] Transform rightWallCheck;
    [SerializeField] float wallRunSpeed;
    [SerializeField][Range(0, 90)] float offWallJumpAngle;
    [SerializeField] float offWallJumpForce;
    [SerializeField] LayerMask wallkMask;
    bool leftWall;
    bool rightWall;
    bool wallRunning;
    bool firstTimeReset = true;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        MapControls();
    }

    private void Update()
    {
        GatherInput();

        IsGrounded();
        ControlDrag();

        Jump();
        ResetJump();
        WallRun();
    }

    void FixedUpdate()
    {
        MovePlayer();
        ControlSpeed();
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * move.y + orientation.right * move.x;
        moveDirection = moveDirection * moveSpeed * 10;

        if (!grounded)
            moveDirection = moveDirection * airMultiplier;

        if (!wallRunning)
        {
            transform.rotation = orientation.rotation;
            rb.AddForce(moveDirection.x, rb.velocity.y, moveDirection.z, ForceMode.Force);
        }
    }

    void Jump()
    {
        if (jump.IsPressed() && canJump && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            canJump = false;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (!grounded && !wallRunning)
        {
            rb.AddForce(Vector3.down * extraGravity, ForceMode.Force);
        }
    }

    void WallRun()
    {
        leftWall = Physics.OverlapSphere(leftWallCheck.position, 0.1f, wallkMask).Length > 0;
        rightWall = Physics.OverlapSphere(rightWallCheck.position, 0.1f, wallkMask).Length > 0;

        if (!grounded && (leftWall || rightWall) && jump.IsPressed() && canJump)
        {
            RaycastHit hit = default;

            if (leftWall)
            {
                Physics.Raycast(leftWallCheck.position, -orientation.right, out hit, 0.1f, wallkMask);
            }
            else if (rightWall)
            {
                Physics.Raycast(rightWallCheck.position, orientation.right, out hit, 0.1f, wallkMask);
            }

            StartWallRun(hit);
        }
        else if (!leftWall && !rightWall)
        {
            StopWallRun();
        }
    }

    void StartWallRun(RaycastHit hit)
    {
        if (firstTimeReset)
        {
            firstTimeReset = false;
            canJump = false;
        }

        if (!wallRunning)
            wallRunning = true;

        Vector3 wallRunDirection = Vector3.ProjectOnPlane(orientation.forward, hit.normal);

        rb.useGravity = false;
        rb.velocity = wallRunDirection.normalized * wallRunSpeed;

        if (canJump && jump.IsPressed())
        {
            canJump = false;
            Vector3 jumpAngle = hit.normal * (90 - offWallJumpAngle) + Vector3.up * offWallJumpAngle;
            rb.AddForce(jumpAngle.normalized * offWallJumpForce, ForceMode.Impulse);
        }
    }

    void StopWallRun()
    {
        firstTimeReset = true;

        if (wallRunning)
            wallRunning = false;

        rb.useGravity = true;
    }

    void IsGrounded()
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
        float tempSpeed = moveSpeed;

        if (wallRunning)
            tempSpeed = wallRunSpeed;

        if (currentVelocity.magnitude > tempSpeed)
        {
            currentVelocity = currentVelocity.normalized * tempSpeed;
            rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);
        }
    }

    void ResetJump()
    {
        if (!jump.IsPressed())
            canJump = true;
    }

    void MapControls()
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
