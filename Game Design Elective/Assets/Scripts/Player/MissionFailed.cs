using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionFailed : MonoBehaviour
{
    [SerializeField] GameObject text;
    float counter = 0;

    void Update()
    {
        if (text.activeSelf)
        {
            counter += Time.deltaTime;
        }
        else
        {
            counter = 0;
        }

        if (counter > 5)
        {
            text.SetActive(false);
        }
    }
}
