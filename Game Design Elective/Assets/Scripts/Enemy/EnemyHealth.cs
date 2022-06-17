using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float health;
    [SerializeField] public float headDmgModifier;
    [SerializeField] public float bodyDmgModifier;
    [SerializeField] float respawnTimer;
    [SerializeField] GameObject missionFailedTxt;
    RespawnPlayer pRes;

    private void Start()
    {
        pRes = GameObject.Find("Player").GetComponentInChildren<RespawnPlayer>();
        missionFailedTxt = pRes.missionFailedTxt;
    }

    private void Update()
    {
        if (health <= 0)
        {
            if (tag == "Enemy")
            {
                gameObject.SetActive(false);
                Invoke("Respawn", respawnTimer);
            }
            else
            {
                gameObject.SetActive(false);
                missionFailedTxt.SetActive(true);
            }
        }
    }

     public void Respawn()
    {
        health = 100;
        gameObject.SetActive(true);
    }
}
