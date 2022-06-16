using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] List<Transform> patrolPoints;
    [SerializeField] float speed;
    int counter = 0;
    int direction = 1;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[counter].position, speed * Time.deltaTime);
        
        if (transform.position == patrolPoints[counter].position)
        {
            counter += direction;
        }

        if (counter < 0)
        {
            direction = 1;
            counter += direction * 2;
        }
        else if (counter > patrolPoints.Count - 1)
        {
            direction = -1;
            counter += direction * 2;
        }
    }
}
