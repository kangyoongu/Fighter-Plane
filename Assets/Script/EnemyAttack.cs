using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject bullet;
    public Transform[] bulPoint;//c
    private Transform root;
    bool shot = false;
    private void Awake()
    {
        root = transform.root;
    }
    private void Start()
    {
        StartCoroutine(ShotBullet());
    }
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
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
       // while (true)
        {
            if (shot == true)
            {
                yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
                Instantiate(bullet, bulPoint[0].position, root.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 4000);
                Instantiate(bullet, bulPoint[1].position, root.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 4000);
            }
        }
    }
}
