using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public Transform lastCheckpoint;

    private void Update()
    {
        if (transform.position.y < 0)
        {
            transform.position = lastCheckpoint.position;
        }
    }
}
