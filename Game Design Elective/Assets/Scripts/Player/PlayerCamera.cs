using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform targetPosition;

    PlayerInput playerInput;
    InputAction lookAction;
    Vector2 look;

    [Header("Look")]
    [SerializeField] Transform orientation;
    [SerializeField] [Range(0, 10)] float xSensitivity;
    [SerializeField] [Range(0, 10)] float ySensitivity;
    float xRotation;
    float yRotation;

    private void Awake()
    {
        playerInput = orientation.GetComponentInParent<PlayerInput>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        mapControls();
    }

    private void Update()
    {
        GatherInput();
        UpdateCameraPosition();
        LookPlayer();
    }

    void UpdateCameraPosition()
    {
        transform.position = targetPosition.position;
    }

    void LookPlayer()
    {
        xRotation += look.y * ySensitivity * Time.deltaTime;
        yRotation += look.x * xSensitivity * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.rotation = Quaternion.Euler(-xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void mapControls()
    {
        lookAction = playerInput.actions["Look"];
    }

    void GatherInput()
    {
        look = lookAction.ReadValue<Vector2>();
    }
}
