using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiss : MonoBehaviour
{
    private Transform target;
    public float speed = 10;
    public GameObject exp;
    public GameObject mini;
    private void Start()
    {
        Destroy(gameObject, 50f);
        target = FindEnemy.Instance.target;
        Instantiate(mini, Vector3.zero, Quaternion.identity).GetComponent<MiniObjFallow>().target = transform;
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
        if(collision.gameObject.tag != "Enemy")
        {
            Destroy(Instantiate(exp, transform.position, Quaternion.identity), 7);
            Destroy(gameObject);
        }
    }
}
