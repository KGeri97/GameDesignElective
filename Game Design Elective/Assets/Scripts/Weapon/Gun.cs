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
    InputAction slowMo;
    Vector2 look;
    bool isGamepad;

    [Header("Gun")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform aimLocation;
    [SerializeField] LayerMask bulletMask;
    [SerializeField] float maxCurveModifier;
    [SerializeField] float fireRate;
    [SerializeField] float range;
    bool canShoot = true;
    bool validTarget;
    float maxCurve;

    [Header("Enemy")]
    [SerializeField] LayerMask enemyMask;
    [SerializeField] float behindWallDistance;

    [Header("Slow-Mo")]
    [SerializeField][Range(0, 1)] float minTimeScale;
    [SerializeField] float slowMoFadeTime;

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
        SlowMo();
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
                bScript.direction = cam.forward;
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
                if (Physics.Raycast(cam.position, cam.forward, out hit, range, bulletMask))
                {
                    validTarget = true;
                    aimLocation.position = hit.point;
                    if (hit.collider.gameObject.layer != enemyMask)
                    {
                        if (Physics.Raycast(cam.position, cam.forward, out hit, range + behindWallDistance, enemyMask))
                        {
                            aimLocation.position = hit.point;
                        }
                    }
                }
                else
                    validTarget = false;

            }

            if (!validTarget)
                return;

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

    void SlowMo()
    {
        if (slowMo.IsPressed() && Time.timeScale > minTimeScale)
        {
            Time.timeScale -= slowMoFadeTime * Time.deltaTime;
            if (Time.timeScale < minTimeScale)
                Time.timeScale = minTimeScale;
        }

        if (!slowMo.IsPressed() && Time.timeScale < 1)
        {
            Time.timeScale += slowMoFadeTime * 4 * Time.deltaTime;
            if (Time.timeScale > 1)
                Time.timeScale = 1;
        }
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
        slowMo = playerInput.actions["SlowMo"];
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
