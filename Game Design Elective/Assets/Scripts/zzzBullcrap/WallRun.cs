using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallRun : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Transform orientation;

    [Header("Detection")]
    [SerializeField] float wallDistance = 0.6f;
    [SerializeField] float minimumJumpHeight = 1.5f;

    [Header("Wall running")]
    [SerializeField] float wallRunGravity;
    [SerializeField] float wallRunJumpForce;

    bool wallLeft = false;
    bool wallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    Rigidbody rb;

    [Header("Input")]

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    private void Start()
    {
        MapControls();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            if (wallLeft)
            {
                Debug.Log("Run");
                StartWallRun();
            }
            else if (wallRight)
            {
                Debug.Log("Run");
                StartWallRun();
            }
            else
            {
                Debug.Log("Stop");
                StopWallRun();
            }
        }
        else
        {
            Debug.Log("Stop");
            StopWallRun();
        }
    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    void StartWallRun()
    {
        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        if (jumpAction.IsPressed())
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce, ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce, ForceMode.Force);
            }
        }
    }

    void StopWallRun()
    {
        rb.useGravity = true;
    }

    void MapControls()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];
    }
}