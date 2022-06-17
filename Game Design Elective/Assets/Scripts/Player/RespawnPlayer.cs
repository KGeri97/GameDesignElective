using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public Transform lastCheckpoint;
    [SerializeField] public GameObject missionFailedTxt;

    private void Update()
    {
        if (transform.position.y < 0)
        {
            missionFailedTxt.SetActive(true);
        }

        if (missionFailedTxt.activeSelf)
        {
            transform.position = lastCheckpoint.position;
        }
    }
}
