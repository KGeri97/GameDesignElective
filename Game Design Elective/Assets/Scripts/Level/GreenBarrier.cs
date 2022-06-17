using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBarrier : MonoBehaviour
{
    public bool triggered;
    public bool active = false;
    [SerializeField] Gun pGun;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            triggered = true;
            pGun.canShoot = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            triggered = true;
            pGun.canShoot = false;
        }
    }
}
