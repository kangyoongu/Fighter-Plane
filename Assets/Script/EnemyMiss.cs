using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMiss : MonoBehaviour
{
    public Transform target;
    public float speed = 10;
    public GameObject exp;
    private void Start()
    {
        Destroy(gameObject, 50f);
    }
    void Update()
    {
        FollowTarget();
    }
    void FollowTarget()
    {
        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir) * Quaternion.Euler(0, -90, 0), Time.deltaTime * speed);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(Instantiate(exp, transform.position, Quaternion.identity), 7);
            Destroy(gameObject);
        }
    }
}
