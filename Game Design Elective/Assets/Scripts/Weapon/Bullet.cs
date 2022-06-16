using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float curveSpeed;
    [SerializeField] public float damage;
    [SerializeField] public Vector3 origin;
    [SerializeField] public Vector3 curveModifier;
    [SerializeField] public Vector3 endPoint;
    [SerializeField] LayerMask bulletMask;
    [SerializeField] float maxLifeTime;
    public Vector3 direction;
    float interpolateAmount = 0;
    float counter;
    float distance;

    private void Start()
    {
        distance = Vector3.Distance(origin, endPoint);
        curveSpeed = 1 / distance * 50;
    }

    void Update()
    {
        //https://www.youtube.com/watch?v=7j_BNf9s0jM&ab_channel=CodeMonkey
        interpolateAmount += Time.deltaTime * curveSpeed;
        //interpolateAmount = (interpolateAmount + Time.deltaTime) % 1f;
        counter += Time.deltaTime;

        if (direction != default)
        {
            if (counter > maxLifeTime)
            {
                Destroy(gameObject);
            }
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            if (interpolateAmount > 1)
            {
                Destroy(gameObject);
            }
            transform.position = QuadraticLerp(origin, curveModifier, endPoint, interpolateAmount);
        }
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
