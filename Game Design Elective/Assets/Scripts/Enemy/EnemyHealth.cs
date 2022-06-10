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
            Destroy(gameObject);
        }
    }
}
