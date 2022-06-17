using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    float curveSpeed;
    [SerializeField] public float damage;
    [SerializeField] public Vector3 origin;
    [SerializeField] public Vector3 curveModifier;
    [SerializeField] public Vector3 endPoint;
    [SerializeField] LayerMask bulletMask;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] float maxLifeTime;
    public Transform marker;
    public Vector3 direction;
    float interpolateAmount = 0;
    float counter;
    float distance;
    float radius;
    RaycastHit hit;

    private void Start()
    {
        distance = Vector3.Distance(origin, endPoint);
        curveSpeed = 1 / distance * 80;
        radius = GetComponent<SphereCollider>().radius * transform.localScale.x;
    }

    void FixedUpdate()
    {
        //https://www.youtube.com/watch?v=7j_BNf9s0jM&ab_channel=CodeMonkey
        interpolateAmount += Time.fixedDeltaTime * curveSpeed;
        if (interpolateAmount > 1)
            interpolateAmount = 1;

        counter += Time.fixedDeltaTime;

        if (direction != default)
        {
            if (counter > maxLifeTime)
            {
                Destroy(gameObject);
            }

            if (Physics.SphereCast(transform.position, radius, direction, out hit, Vector3.Distance(transform.position, transform.position + direction * speed * Time.fixedDeltaTime), enemyMask + bulletMask))
            {
                transform.position = hit.point;
            }
            else
                transform.position += direction * speed * Time.fixedDeltaTime;
        }
        else
        {
            endPoint = marker.position;
            if (interpolateAmount >= 1)
                Invoke("asd", 0.05f);

            Vector3 newPlace = QuadraticLerp(origin, curveModifier, endPoint, interpolateAmount);
            if (Physics.SphereCast(transform.position, radius, newPlace - transform.position, out hit, Vector3.Distance(transform.position, newPlace), enemyMask + bulletMask))
            {
                transform.position = hit.point;
            }
            else
                transform.position = newPlace;
            
        }
    }

    void asd()
    {
        Destroy(gameObject);
    }

    Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((bulletMask.value & (1 << other.gameObject.layer)) > 0)
        {
            Destroy(gameObject);
        }
    }
}
