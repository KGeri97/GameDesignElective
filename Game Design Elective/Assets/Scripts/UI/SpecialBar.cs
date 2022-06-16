using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBar : MonoBehaviour
{
    //[SerializeField] Transform bar;
    [SerializeField] SlowMotion script;
    float special;
    float cap;
    float maxWidth;

    private void Start()
    {
        maxWidth = transform.localScale.x;
        cap = script.specialCap;
    }

    private void Update()
    {
        special = script.specialAmount;
        transform.localScale = new Vector3(maxWidth * (special / cap), transform.localScale.y, transform.localScale.z);
    }
}
