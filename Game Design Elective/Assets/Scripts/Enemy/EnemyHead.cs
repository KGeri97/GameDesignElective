using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    [SerializeField] EnemyHealth healthScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            //Debug.Log("Headshot");
            float dmg = other.gameObject.GetComponent<Bullet>().damage;
            healthScript.health -= dmg * healthScript.headDmgModifier;
        }
    }
}
