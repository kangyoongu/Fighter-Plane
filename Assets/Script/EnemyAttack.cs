using System.Collections;
using System.Collections.Generic;
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
        if(misTime >= 16)
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
            shot = true;
            target = other.transform;
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
