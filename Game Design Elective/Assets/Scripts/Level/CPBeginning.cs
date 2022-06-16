using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CPBeginning : MonoBehaviour
{
    [SerializeField] PlayerInput pInput;
    [SerializeField] PlayerMovement pMove;
    [SerializeField] PlayerCamera pCam;
    [SerializeField] Gun pGun;
    
    [Header("Texts Gamepad")]
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
    [SerializeField] GameObject txt6;

    [Header("Barriers")]
    [SerializeField] Barrier jumpBarrier;
    [SerializeField] Barrier slideBarrier;
    [SerializeField] Barrier dashBarrier;
    [SerializeField] Barrier wallrunBarrier;
    [SerializeField] Barrier advancedWallrunBarrier;
    [SerializeField] Barrier enemyBarrier;
    [SerializeField] Barrier advancedEnemyBarrier;
    [SerializeField] GameObject greenBarrier;
    [SerializeField] GameObject wall;

    [Header("Enemies")]
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] GameObject enemy3;
    [SerializeField] GameObject enemy4;

    [Header("Markers")]
    public bool tutLook = false;
    public bool tutMove = false;
    public bool tutJumpReached = false;
    public bool tutSlideReached = false;
    public bool tutDashReached = false;
    public bool tutWallrunReached = false;
    public bool tutAdvancedWallrunReached = false;
    public bool tutEnemyReached = false;
    public bool tutAdvancedEnemyReached = false;
    [SerializeField] bool noTut22 = true;

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

        Barrier();

        tut11();
        tut12();
        tut13();
        tut14();
        tut15();
        tut21();
        tut22();
        tut31();
        tut41();
        tut51();
        tut52();

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

    void Barrier()
    {
        if (jumpBarrier.triggered)
            tutJumpReached = true;

        if (slideBarrier.triggered)
            tutSlideReached = true;

        if (dashBarrier.triggered)
            tutDashReached = true;

        if (wallrunBarrier.triggered)
            tutWallrunReached = true;

        if (advancedWallrunBarrier.triggered)
            tutAdvancedWallrunReached = true;

        if (enemyBarrier.triggered)
            tutEnemyReached = true;

        if (advancedEnemyBarrier.triggered)
            tutAdvancedEnemyReached = true;
    }

    void tut11()
    {
        if (!tutLook && !txt11.activeSelf)
        {
            txt11.SetActive(true);
            counter = 0;
        }

        if (pCam.look != Vector2.zero && counter > 1)
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

        if (tutLook && pMove.moveDirection != Vector3.zero && counter > 2)
        {
            tutMove = true;
            txt12.SetActive(false);
        }
    }

    void tut13()
    {
        if (tutMove && tutJumpReached && !txt13.activeSelf)
        {
            txt13.SetActive(true);
            counter = 0;
        }

        if (tutSlideReached && counter > 2)
        {
            jumpBarrier.triggered = false;
            tutJumpReached = false;
            txt13.SetActive(false);
        }
    }

    void tut14()
    {
        if (tutSlideReached && !txt14.activeSelf && !txt13.activeSelf)
        {
            txt14.SetActive(true);
            counter = 0;
        }

        if (tutDashReached && counter > 2)
        {
            slideBarrier.triggered = false;
            txt14.SetActive(false);
            tutSlideReached = false;
        }
    }

    void tut15()
    {
        if (tutDashReached && !txt15.activeSelf && !txt14.activeSelf)
        {
            txt15.SetActive(true);
            counter = 0;
        }

        if (tutWallrunReached && counter > 5)
        {
            dashBarrier.triggered = false;
            txt15.SetActive(false);
            tutDashReached = false;
        }
    }

    void tut21()
    {
        if (tutWallrunReached && !txt21.activeSelf && !txt15.activeSelf)
        {
            txt21.SetActive(true);
            counter = 0;
        }

        if (tutWallrunReached && pMove.wallRunning && counter > 5)
        {
            wallrunBarrier.triggered = false;
            txt21.SetActive(false);
            tutWallrunReached = false;
            noTut22 = false;
        }
    }

    void tut22()
    {
        if (!noTut22 && pMove.wallRunning && !txt22.activeSelf && !txt21.activeSelf)
        {
            txt22.SetActive(true);
            counter = 0;
        }

        if (tutAdvancedWallrunReached && counter > 5)
        {
            txt22.SetActive(false);
            noTut22 = true;
        }
    }

    void tut31()
    {
        if (tutAdvancedWallrunReached && !txt31.activeSelf && !txt22.activeSelf)
        {
            txt31.SetActive(true);
            counter = 0;
            noTut22 = true;
        }

        if (tutEnemyReached && counter > 5)
        {
            advancedWallrunBarrier.triggered = false;
            txt31.SetActive(false);
            tutAdvancedWallrunReached = false;
        }
    }

    void tut41()
    {
        if (tutEnemyReached && !txt41.activeSelf && !txt31.activeSelf)
        {
            txt41.SetActive(true);
            counter = 0;
        }

        if (!enemy1.activeSelf && !enemy2.activeSelf & !enemy3.activeSelf && counter > 5)
        {
            enemyBarrier.triggered = false;
            txt41.SetActive(false);
            tutEnemyReached = false;
            wall.SetActive(false);
        }
    }

    void tut51()
    {
        if (tutAdvancedEnemyReached && !txt41.activeSelf && !txt51.activeSelf)
        {
            txt51.SetActive(true);
            txt52.SetActive(false);
            counter = 0;
            pMove.jumpForce = 0;
            pGun.canShoot = false;
        }

        if (txt51.activeSelf && counter > 5)
        {
            greenBarrier.SetActive(true);
            advancedEnemyBarrier.triggered = false;
            txt51.SetActive(false);
            tutAdvancedEnemyReached = false;
            counter = 0;
            txt52.SetActive(true);
        }
    }

    void tut52()
    {
        if (!enemy4.activeSelf)
        {
            txt52.SetActive(false);
            pMove.jumpForce = jumpForce;
            greenBarrier.SetActive(false);
            pGun.canShoot = true;
            txt6.SetActive(true);
        }
    }
}
