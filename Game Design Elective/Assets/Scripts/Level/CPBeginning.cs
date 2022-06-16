using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CPBeginning : MonoBehaviour
{
    [SerializeField] PlayerInput pInput;
    [SerializeField] PlayerMovement pMove;

    public bool tutLook = false;
    public bool tutMove = false;
    public bool tutSlideReached = false;
    public bool tutJumpReached = false;

    float jumpForce;

    private void Start()
    {

    }

    private void Update()
    {
        
    }

    void getBasicValues()
    {
        jumpForce = pMove.jumpForce;
    }
}
