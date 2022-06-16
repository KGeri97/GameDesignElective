using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float health;
    [SerializeField] public float headDmgModifier;
    [SerializeField] public float bodyDmgModifier;

    private void Update()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
            Invoke("Respawn", 1);
        }
    }

    void Respawn()
    {
        health = 100;
        gameObject.SetActive(true);
    }
}
