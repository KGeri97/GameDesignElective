using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CPBeginning : MonoBehaviour
{
    [SerializeField] PlayerInput pInput;
    [SerializeField] PlayerMovement pMove;
    [SerializeField] PlayerCamera pCam;
    
    [Header("Texts")]
    [SerializeField] GameObject txt11;
    [SerializeField] GameObject txt12;
    [SerializeField] GameObject txt13;
    [SerializeField] GameObject txt14;
    [SerializeField] GameObject txt15;
    [SerializeField] GameObject txt21;
    [SerializeField] GameObject txt22;
    [SerializeField] GameObject txt31;
    [SerializeField] GameObject txt41;
    [SerializeField] GameObject txt51;
    [SerializeField] GameObject txt52;

    public bool tutLook = false;
    public bool tutMove = false;
    public bool tutJumpReached = false;
    public bool tutSlideReached = false;

    float counter;

    bool isGamepad;

    float jumpForce;

    private void Start()
    {
        getBasicValues();
    }

    private void Update()
    {
        counter += Time.deltaTime;

        tut11();
        tut12();

        SwitchController();
    }

    void getBasicValues()
    {
        jumpForce = pMove.jumpForce;
    }

    void SwitchController()
    {
        if (pInput.currentControlScheme == "Gamepad")
            isGamepad = true;
        else
            isGamepad = false;
    }

    void tut11()
    {
        if (!tutLook && !txt11.activeSelf)
        {
            txt11.SetActive(true);
            counter = 0;
        }

        if (pCam.look != Vector2.zero && counter > 3)
        {
            tutLook = true;
            txt11.SetActive(false);
        }
    }

    void tut12()
    {

        if (!tutMove && tutLook && !txt12.activeSelf)
        {
            txt12.SetActive(true);
            counter = 0;
        }

        if (pMove.moveDirection != Vector3.zero && counter > 3)
        {
            tutMove = true;
            txt12.SetActive(false);
        }
    }

    void tut13()
    {

        if (tutMove && tutJumpReached && tutLook && !txt13.activeSelf)
        {
            txt13.SetActive(true);
            counter = 0;
        }

        if (pMove.moveDirection != Vector3.zero && counter > 3)
        {
            tutMove = true;
            txt12.SetActive(false);
        }
    }
}
