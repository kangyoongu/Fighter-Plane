using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class EnemyMiss : MonoBehaviour
{
    public Transform target;
    public float speed;
    public GameObject exp;
    public GameObject mini;
    float time = 0;
    ConstantForce cf;
    bool isEnd = false;
    bool cut = false;
    private void Start()
    {
        Instantiate(mini, Vector3.zero, Quaternion.identity).GetComponent<MiniObjFallow>().target = transform;
        cf = GetComponent<ConstantForce>();
    }
    void Update()
    {
        if (isEnd == false)
        {
            FollowTarget();
            time += Time.deltaTime;
        }
        if (time >= 20)
        {
            if (isEnd == false)
            {
                isEnd = true;
                cf.relativeForce = Vector3.zero;
                cf.force = new Vector3(0, -100, 0);
                gameObject.GetComponent<Rigidbody>().drag = 0.5f;
                transform.GetChild(1).GetChild(0).GetComponent<VisualEffect>().Stop();
            }
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
    IEnumerator sh(int x, float y)
    {
        ShakeManager.Instance.Shake(x, y);
        yield return null;
    }
    public void OnParticleCollision(GameObject other)
    {
        if (cut == false)
        {
            ScoreManager.Instance.Score += 5;
            HitEnemy.Instance.hitEnemy();
            cut = true;
            StartCoroutine(sh(3, 6));
            DieMis();
        }
    }
    public void DieMis()
    {
        Destroy(Instantiate(exp, transform.position, Quaternion.identity), 7);
        Destroy(gameObject);
    }
}
