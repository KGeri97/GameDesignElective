using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlowMotion : MonoBehaviour
{

    [Header("Input")]
    [SerializeField] PlayerInput playerInput;
    InputAction slowMo;

    [Header("Slow-Mo")]
    [SerializeField] [Range(0, 1)] float minTimeScale;
    [SerializeField] float slowMoFadeTime;
    [SerializeField] public float specialCap;
    [SerializeField] public float specialDrainRate;
    [SerializeField] public float headshotGain;
    public float specialAmount;

    private void Start()
    {
        mapControls();
        specialAmount = specialCap;
    }

    private void Update()
    {
        if (specialAmount > specialCap)
            specialAmount = specialCap;

        SlowMo();

        if (specialAmount < 0)
            specialAmount = 0;
    }

    void SlowMo()
    {
        if (slowMo.IsPressed())
        {
            specialAmount -= specialDrainRate * Time.deltaTime;
        }

        if (slowMo.IsPressed() && Time.timeScale > minTimeScale && specialAmount > specialDrainRate * Time.deltaTime)
        {
            Time.timeScale -= slowMoFadeTime * Time.deltaTime;
            if (Time.timeScale < minTimeScale)
                Time.timeScale = minTimeScale;
        }

        if (!slowMo.IsPressed() && Time.timeScale < 1 || specialAmount < specialDrainRate * Time.deltaTime)
        {
            Time.timeScale += slowMoFadeTime * 4 * Time.deltaTime;
            if (Time.timeScale > 1)
                Time.timeScale = 1;
        }
    }


    void mapControls()
    {
        slowMo = playerInput.actions["SlowMo"];
    }
}
