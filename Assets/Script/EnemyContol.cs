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
    int damage = 0;
    public GameObject smoke;
    [SerializeField]
    private GameObject dieParticel;
    float power = 1;
    public GameObject exp;
    private GameObject g = null;
    float startTime = 0;
    public GameObject enemini;
    private Transform mymini;
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
            if (startTime < 7)
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
        float dis = Vector3.Distance(transform.position, target.position);
        if (dis >= 1500)
        {
            rigid.AddRelativeForce(Vector3.forward * Time.deltaTime * speeds * 110 * power);
            jet[0].Play();
            jet[1].Play();
        }
        else if (dis <= 800)
        {
            rigid.AddRelativeForce(Vector3.forward * Time.deltaTime * speeds * 50 * power);
            jet[0].Stop();
            jet[1].Stop();
        }
        else
        {
            rigid.AddRelativeForce(Vector3.forward * Time.deltaTime * speeds * 80 * power);
            jet[0].Play();
            jet[1].Play();
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Mis")
        {
            Destroy(Instantiate(exp, collision.transform.position, Quaternion.identity), 7);
            Destroy(collision.gameObject);
            FindEnemy.Instance.canShot = false;
            ScoreManager.Instance.Score += 100;
            StartCoroutine(Die());
        }
        else if(collision.gameObject.tag == "Bullet")
        {
            Transform t = Instantiate(smoke, collision.contacts[0].point, Quaternion.identity).transform;
            t.parent = transform;
            t.localScale = new Vector3(5, 5, 5);
            power -= 0.04f;
            damage++;
            ScoreManager.Instance.Score += 250;
            if (damage >= 20)
            {
                StartCoroutine(Die());
            }
        }
        else if(collision.gameObject.tag == "Player")
        {
            PlayerControl.Instance.Die();
            StartCoroutine(Die());
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