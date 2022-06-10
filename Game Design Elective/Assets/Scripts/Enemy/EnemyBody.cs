using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    [SerializeField] EnemyHealth healthScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Debug.Log("Bodyshot");
            float dmg = other.gameObject.GetComponent<Bullet>().damage;
            healthScript.health -= dmg * healthScript.bodyDmgModifier;
        }
    }
}
