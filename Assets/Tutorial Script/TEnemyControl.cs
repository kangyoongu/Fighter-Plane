using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TEnemyControl : MonoBehaviour
{
    int damage = 0;
    public GameObject smoke;
    [SerializeField]
    private GameObject dieParticel;
    public GameObject exp;
    private GameObject g = null;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mis")
        {
            Destroy(Instantiate(exp, collision.transform.position, Quaternion.identity), 7);
            TutorialManager.Instance.SixSet();
            FindEnemy.Instance.canShot = false;
            Destroy(collision.gameObject);
            StartCoroutine(Die());
        }
        else if (collision.gameObject.tag == "Bullet")
        {
            if (damage >= 20)
            {
                StartCoroutine(Die());
            }
            Transform t = Instantiate(smoke, collision.contacts[0].point, Quaternion.identity).transform;
            t.parent = transform;
            t.localScale = new Vector3(5, 5, 5);
            damage++;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(TPlayerControl.Instance.Die());
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
            Destroy(gameObject);
        }
        yield return null;
    }
}