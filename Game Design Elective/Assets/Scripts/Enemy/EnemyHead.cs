using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    [SerializeField] EnemyHealth healthScript;
    [SerializeField] SlowMotion sloMoScript;

    private void Awake()
    {
        sloMoScript = GameObject.Find("Player").GetComponent<SlowMotion>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            //Debug.Log("Headshot");
            float dmg = other.gameObject.GetComponent<Bullet>().damage;
            healthScript.health -= dmg * healthScript.headDmgModifier;

            sloMoScript.specialAmount += sloMoScript.headshotGain;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            //Debug.Log("Headshot");
            float dmg = other.gameObject.GetComponent<Bullet>().damage;
            healthScript.health -= dmg * healthScript.headDmgModifier;

            sloMoScript.specialAmount += sloMoScript.headshotGain;
        }
    }
}
