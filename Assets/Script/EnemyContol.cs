using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContol : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float speeds;
    private Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Start()
    {

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
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.right), Time.deltaTime * speed);
        }
        float dis = Vector3.Distance(transform.position, target.position);
        if (dis >= 300)
        {
            rigid.AddRelativeForce(Vector3.forward * Time.deltaTime * speeds * 20);
        }
        else if (dis <= 160)
        {
            rigid.AddRelativeForce(Vector3.forward * Time.deltaTime * speeds * 2);
        }
        else
        {
            rigid.AddRelativeForce(Vector3.forward * Time.deltaTime * speeds * 7);
        }
    }
}