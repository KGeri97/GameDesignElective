using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionFailed : MonoBehaviour
{
    [SerializeField] GameObject text;
    [SerializeField] List<GameObject> npc;
    float counter = 0;

    void Update()
    {
        if (text.activeSelf)
        {
            counter += Time.deltaTime;
        }
        else
        {
            counter = 0;
        }

        if (counter > 5)
        {
            text.SetActive(false);

            foreach (GameObject n in npc)
            {
                n.SetActive(true);
                n.GetComponent<EnemyHealth>().CancelInvoke();
                n.GetComponent<EnemyHealth>().Invoke("Respawn", 2);
            }
        }
    }
}
