using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMiss : MonoBehaviour
{
    public Transform target;
    public float speed = 10;
    public GameObject exp;
    public GameObject mini;
    float time = 0;
    private void Start()
    {
        Instantiate(mini, Vector3.zero, Quaternion.identity).GetComponent<MiniObjFallow>().target = transform;
    }
    void Update()
    {
        FollowTarget();
        time += Time.deltaTime;
        if(time >= 20)
        {
            DieMis();
        }
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
        if (collision.gameObject.tag == "Player")
        {
            PlayerControl.Instance.Die();
            DieMis();
        }
        else
        {
            DieMis();
        }
    }
    public void OnParticleCollision(GameObject other)
    {
        ScoreManager.Instance.Score += 70;
        DieMis();
    }
    private void DieMis()
    {
        Destroy(Instantiate(exp, transform.position, Quaternion.identity), 7);
        Destroy(gameObject);
    }
}
