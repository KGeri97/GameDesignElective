using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] PlayerInput playerInput;
    InputAction shoot;
    InputAction curve;
    InputAction lookAction;
    Vector2 look;
    bool isGamepad;

    [Header("Gun")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform aimLocation;
    [SerializeField] LayerMask bulletMask;
    [SerializeField] float maxCurveModifier;
    [SerializeField] float fireRate;
    bool canShoot = true;
    float maxCurve;

    //[SerializeField] Transform curveOffsetTransform;

    [Header("Line")]
    [SerializeField] int lineResolution;
    LineRenderer lr;
    Vector3 curveOffset;

    [Header("POV")]
    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;
    bool newLocation = true;
    RaycastHit hit;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();

        mapControls();
    }

    private void Update()
    {
        SwitchController();

        GatherInput();
        Curve();
        Shoot();
    }

    void Shoot()
    {
        if (shoot.IsPressed() && canShoot)
        {
            canShoot = false;
            Invoke("ResetShoot", fireRate);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bScript = bullet.GetComponent<Bullet>();

            if (curve.IsPressed())
            {
                bScript.origin = firePoint.position;
                bScript.curveModifier = curveOffset;
                bScript.endPoint = aimLocation.position;
            }
            else
            {
                Physics.Raycast(cam.position, cam.forward, out hit, 100, bulletMask);
                Vector3 halfPoint = firePoint.position + (hit.point - firePoint.position) / 2;

                bScript.origin = firePoint.position;
                bScript.curveModifier = halfPoint;
                bScript.endPoint = hit.point;
            }
        }
    }

    void ResetShoot()
    {
        canShoot = true;
    }

    void Curve()
    {
        if (curve.IsPressed())
        {
            lr.positionCount = lineResolution;

            if (newLocation)
            {
                newLocation = false;
                Physics.Raycast(cam.position, cam.forward, out hit, 100, bulletMask);
                aimLocation.position = hit.point;
            }

            cam.LookAt(aimLocation);

            curveOffset = GetCurveOffset();
            SetLinePositions(curveOffset);
        }
        else if (!curve.IsPressed() && !newLocation)
            newLocation = true;
        if (!curve.IsPressed())
        {
            lr.positionCount = 0;
        }
    }

    Vector3 GetCurveOffset()
    {
        maxCurve = Vector3.Distance(aimLocation.position, firePoint.position);
        Vector3 halfPoint = firePoint.position + (aimLocation.position - firePoint.position) / 2;

        if (isGamepad)
        {
            halfPoint += (cam.up * look.y + cam.right * look.x) * maxCurve * maxCurveModifier;
        }

        return halfPoint;
    }

    void SetLinePositions(Vector3 offSet)
    {
        for (int i = 0; i < lineResolution; i++)
        {
            lr.SetPosition(i, QuadraticLerp(firePoint.position, offSet, aimLocation.position, 1f / lineResolution * (i+1)));
        }
    }

    void mapControls()
    {
        lookAction = playerInput.actions["Look"];
        shoot = playerInput.actions["Shoot"];
        curve = playerInput.actions["Curve"];
    }

    void GatherInput()
    {
        look = lookAction.ReadValue<Vector2>();
    }

    Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }

    void SwitchController()
    {
        if (playerInput.currentControlScheme == "Gamepad")
            isGamepad = true;
        else
            isGamepad = false;
    }
}
