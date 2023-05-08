using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyContol : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float speeds;
    private Rigidbody rigid;
    public VisualEffect[] jet;
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
            jet[0].Play();
            jet[1].Play();
        }
        else if (dis <= 160)
        {
            rigid.AddRelativeForce(Vector3.forward * Time.deltaTime * speeds * 2);
            jet[0].Stop();
            jet[1].Stop();
        }
        else
        {
            rigid.AddRelativeForce(Vector3.forward * Time.deltaTime * speeds * 7);
            jet[0].Stop();
            jet[1].Stop();
        }
    }
}