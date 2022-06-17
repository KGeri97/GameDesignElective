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

    private void Update()
    {
        if (health <= 0)
        {
            if (tag != "Enemy")
            {
                gameObject.SetActive(false);
                Invoke("Respawn", respawnTimer);
            }
            else
            {
                missionFailedTxt.SetActive(true);
            }
        }
    }

    void Respawn()
    {
        health = 100;
        gameObject.SetActive(true);
    }
}
