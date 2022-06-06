using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float sensitivityX;
    [SerializeField] float sensitivityY;

    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;

    private PlayerInput playerInput;
    private InputAction lookAction;

    Vector2 look;

    float multiplier = 0.01f;
    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MapConrtols();
    }

    private void Update()
    {
        GatherInput();

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void GatherInput()
    {
        look = lookAction.ReadValue<Vector2>();

        yRotation += look.x * sensitivityX * multiplier;
        xRotation -= look.y * sensitivityY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    void MapConrtols()
    {
        playerInput = GetComponent<PlayerInput>();

        lookAction = playerInput.actions["Look"];
    }
}
