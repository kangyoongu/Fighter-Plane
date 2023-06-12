using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject bullet;
    public Transform[] bulPoint;//c
    private Transform root;
    bool shot = false;
    public Transform target;
    public GameObject mis;
    float misTime = 0;
    public float misCool = 0;
    public AudioSource b;
    private void Awake()
    {
        root = transform.root;
    }
    private void Start()
    {
        StartCoroutine(ShotBullet());
    }
    private void Update()
    {
        misTime += Time.deltaTime;
        if(misTime >= misCool)
        {
            if(shot == true)
            {
                misTime = 0;
                ShotMiss();
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            target = other.transform;
            shot = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            shot = false;
        }
    }
    IEnumerator ShotBullet()
    {
        while (true)
        {
            if (shot == true)
            {
                yield return new WaitForSeconds(Random.Range(0.2f, 0.3f));
                /*ObjectPool.instance.GetObject(bulPoint[0].position, root.rotation * Quaternion.Euler(90, 0, 0));
                ObjectPool.instance.GetObject(bulPoint[1].position, root.rotation * Quaternion.Euler(90, 0, 0));*/
                b.Play();
                Instantiate(bullet, bulPoint[0].position, root.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 4000);
                Instantiate(bullet, bulPoint[1].position, root.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 4000);
            }
            yield return null;
        }
    }
    public void ShotMiss()
    {
        if (shot == true)
        {
            GameObject m = Instantiate(mis, transform.root.position, transform.root.rotation * Quaternion.Euler(0, -90, 0));
            m.GetComponent<Rigidbody>().velocity = transform.root.GetComponent<Rigidbody>().velocity;
            m.GetComponent<EnemyMiss>().target = target;
        }
    }
}
