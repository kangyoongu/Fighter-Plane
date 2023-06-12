using System.Collections;
using UnityEngine;

public class TEnemyControl : MonoBehaviour
{
    int damage = 0;
    public GameObject smoke;
    [SerializeField]
    private GameObject dieParticel;
    public GameObject exp;
    private GameObject g = null;
    float dis = 0;
    bool cut = false;
    IEnumerator sh(int x, float y)
    {
        ShakeManager.Instance.Shake(x, y);
        yield return null;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Mis")
        {
            if (cut == false)
            {
                cut = true;
                Destroy(Instantiate(exp, collision.transform.position, Quaternion.identity), 7);
                Destroy(collision.gameObject);
                TFindEnemy.Instance.canShot = false;
                HitEnemy.Instance.hitEnemy();
                TutorialManager.Instance.SixSet();
                if (((1000 - dis) * 0.005f) >= 2)
                {
                    StartCoroutine(sh(4, (1200 - dis) * 0.005f));
                }
                StartCoroutine(Die());
            }
        }
        else if (collision.gameObject.tag == "Bullet")
        {
            Transform t = Instantiate(smoke, collision.contacts[0].point, Quaternion.identity).transform;
            t.parent = transform;
            t.localScale = new Vector3(5, 5, 5);
            Destroy(collision.gameObject);
            HitEnemy.Instance.hitEnemy();
            if (damage >= 20)
            {
                StartCoroutine(Die());
            }
        }
        else if (collision.gameObject.tag == "Player")
        {
            TPlayerControl.Instance.Die();
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