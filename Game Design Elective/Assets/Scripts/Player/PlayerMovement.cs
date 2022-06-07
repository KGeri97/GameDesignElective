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
    InputAction jump;
    InputAction slide;
    InputAction dash;

    [Header("Run")]
    [SerializeField] float moveSpeed;
    [SerializeField] float groundDrag;
    [SerializeField] float maxSpeedReductionRate;
    [SerializeField] Transform orientation;
    Vector3 moveDirection;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float extraGravity;
    [SerializeField] float airMultiplier;
    bool canJump;

    [Header("Slide")]
    [SerializeField] float slideMinDrag;
    [SerializeField] float slideMaxDrag;
    [SerializeField] float slideDragChangeSpeed;
    [SerializeField] float slideHeightMultiplier;
    [SerializeField] float slideSpeedLowThreshold;
    bool sliding = false;
    CapsuleCollider coll;

    [Header("Dash")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;
    [SerializeField] bool directionalDash;
    bool canDash = true;
    bool dashing = false;

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

    [Header("Ground")]
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask groundMask;
    bool grounded;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();

        MapControls();
    }

    private void Update()
    {
        GatherInput();

        IsGrounded();
        ControlDrag();

        Jump();
        ResetJump();
        Slide();
        Dash();
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
            rb.AddForce(Vector3.down * extraGravity * Time.deltaTime, ForceMode.Force);
        }
    }
    
    void Slide()
    {
        if (slide.IsPressed() && grounded && !sliding && !dashing && rb.velocity.magnitude > slideSpeedLowThreshold)
        {
            rb.drag = slideMinDrag;
            sliding = true;
            coll.height *= slideHeightMultiplier;
        }
        else if (sliding && (rb.velocity.magnitude < slideSpeedLowThreshold || jump.IsPressed()))
        {
            sliding = false;
            coll.height /= slideHeightMultiplier;
        }
    }

    void Dash()
    {
        if (dash.IsPressed() && canDash && !wallRunning && !sliding)
        {
            dashing = true;
            canDash = false;

            if (!directionalDash)
                rb.velocity = orientation.forward.normalized * dashSpeed;
            else
                rb.velocity = Vector3.Normalize(orientation.forward * move.y + orientation.right * move.x) * dashSpeed;

            Invoke("StopDash", dashDuration);
            Invoke("ResetDash", dashDuration + dashCooldown);
        }
    }

    void StopDash()
    {
        dashing = false;
        rb.velocity = rb.velocity.normalized * moveSpeed;
    }

    void ResetDash()
    {
        canDash = true;
    }

    void WallRun()
    {
        leftWall = Physics.OverlapSphere(leftWallCheck.position, 0.1f, wallkMask).Length > 0;
        rightWall = Physics.OverlapSphere(rightWallCheck.position, 0.1f, wallkMask).Length > 0;

        if (!grounded && (leftWall || rightWall) && !dashing && jump.IsPressed() && canJump)
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
        if (sliding && rb.drag < slideMaxDrag)
            rb.drag += slideDragChangeSpeed * Time.deltaTime;
        else if (grounded)
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

        if (!sliding && !dashing && currentVelocity.magnitude > tempSpeed)
        {
            currentVelocity -= currentVelocity * maxSpeedReductionRate * Time.deltaTime;
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
        jump = playerInput.actions["Jump"];
        dash = playerInput.actions["Dash"];
        slide = playerInput.actions["Slide"];
    }

    void GatherInput()
    {
        move = moveAction.ReadValue<Vector2>();
    }
}
