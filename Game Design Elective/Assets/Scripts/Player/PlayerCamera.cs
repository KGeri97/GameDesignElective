using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform targetPosition;
    [SerializeField] SlowMotion sloMoscript;

    PlayerInput playerInput;
    InputAction curve;
    InputAction lookAction;
    public Vector2 look;

    [Header("Look")]
    [SerializeField] Transform orientation;
    [SerializeField] [Range(0, 1)] float xSensitivity;
    [SerializeField] [Range(0, 1)] float ySensitivity;
    [SerializeField] float gamepadMultiplier;
    float xRotation;
    float yRotation;
    bool isGamepad;

    private void Awake()
    {
        playerInput = orientation.GetComponentInParent<PlayerInput>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mapControls();
    }

    private void Update()
    {
        UpdateCameraPosition();

        LookPlayer();

        SwitchController();
    }

    void UpdateCameraPosition()
    {
        transform.position = targetPosition.position;
    }

    void LookPlayer()
    {
        if (!curve.IsPressed() || sloMoscript.specialAmount < sloMoscript.specialDrainRate * Time.deltaTime)
        {
            GatherInput();
            if (isGamepad)
            {
                xRotation += look.y * ySensitivity * gamepadMultiplier * 10 * Time.deltaTime;
                yRotation += look.x * xSensitivity * gamepadMultiplier * 10 * Time.deltaTime;
            }
            else
            {
                xRotation += look.y * ySensitivity * 10 * Time.deltaTime;
                yRotation += look.x * xSensitivity * 10 * Time.deltaTime;
            }

            xRotation = Mathf.Clamp(xRotation, -90, 90);

            transform.rotation = Quaternion.Euler(-xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        } else if (curve.IsPressed())
        {
            if (transform.eulerAngles.x <= 90)
                xRotation = -transform.eulerAngles.x;
            else
                xRotation = 90 - (transform.eulerAngles.x - 270);

            yRotation = transform.eulerAngles.y;

            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
            
    }

    void mapControls()
    {
        lookAction = playerInput.actions["Look"];
        curve = playerInput.actions["Curve"];
    }

    void GatherInput()
    {
        look = lookAction.ReadValue<Vector2>();
    }

    void SwitchController()
    {
        if (playerInput.currentControlScheme == "Gamepad")
            isGamepad = true;
        else
            isGamepad = false;
    }
}
