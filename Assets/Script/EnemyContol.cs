using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContol : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float speeds;
    private Rigidbody rigid;
    int damage = 0;
    public GameObject smoke;
    [SerializeField]
    private GameObject dieParticel;
    float power = 1;
    public GameObject exp;
    private GameObject g = null;
    float startTime = 0;
    public GameObject enemini;
    float dis = 0;
    bool cut = false;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Start()
    {
        speed += Random.Range(-0.5f, 0.5f);
        speeds += Random.Range(-1000, 1000);
        startTime = 0;
        damage = 0;
        power = 1;
        Instantiate(enemini, Vector3.zero, Quaternion.identity).GetComponent<MiniObjFallow>().target = transform;
    }

    void Update()
    {
        if (gameObject.name != "Enemy Variant")
        {
            startTime += Time.deltaTime;
            if (startTime < 6)
            {
                rigid.AddForce(Vector3.up * Time.deltaTime * 700000);
            }
            else
            {
                FollowTarget();
            }
        }
    }
    void FollowTarget()
    {
        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.right), Time.deltaTime * speed);
        }
        dis = Vector3.Distance(transform.position, target.position);
        if (dis >= 1500)
        {
            rigid.AddRelativeForce(Vector3.forward * Time.deltaTime * speeds * 180 * power);
        }
        else if (dis <= 400)
        {
            rigid.AddRelativeForce(Vector3.forward * Time.deltaTime * speeds * 75 * power);
        }
        else
        {
            rigid.AddRelativeForce(Vector3.forward * Time.deltaTime * speeds * 100 * power);
        }
    }
    IEnumerator sh(int x, float y)
    {
        ShakeManager.Instance.Shake(x, y);
        yield return null;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Mis")
        {
            if (cut == false)
            {
                cut = true;
                Destroy(Instantiate(exp, collision.transform.position, Quaternion.identity), 7);
                Destroy(collision.gameObject);
                FindEnemy.Instance.canShot = false;
                ScoreManager.Instance.Score += 13;
                HitEnemy.Instance.hitEnemy();
                if (((1000 - dis) * 0.005f) >= 2)
                {
                    StartCoroutine(sh(4, (1200 - dis) * 0.01f));
                }
                StartCoroutine(Die());
            }
        }
        else if(collision.gameObject.tag == "Bullet")
        {
            Transform t = Instantiate(smoke, collision.contacts[0].point, Quaternion.identity).transform;
            t.parent = transform;
            t.localScale = new Vector3(5, 5, 5);
            power -= 0.04f;
            damage++;
            Destroy(collision.gameObject);
            HitEnemy.Instance.hitEnemy();
            ScoreManager.Instance.Score += 7;
            if (damage >= 20)
            {
                StartCoroutine(Die());
            }
        }
        else if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(Die());
            PlayerControl.Instance.Die();
        }
        else
        {
            StartCoroutine(Die());
        }
    }
    IEnumerator Die()//Á×¾úÀ» ¶§
    {
        if (g == null)
        {
            g = Instantiate(dieParticel, transform.position, Quaternion.identity);
            g.transform.position = transform.position;
            g.transform.rotation = transform.rotation;
            g.GetComponentInChildren<Rigidbody>().velocity = rigid.velocity;
            EnemyMaker.enemyCount -= 1;
            Destroy(gameObject);
        }
        yield return null;
    }
}