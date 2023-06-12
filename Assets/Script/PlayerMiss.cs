using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
public class PlayerMiss : MonoBehaviour
{
    private Transform target;
    public float speed = 10;
    public GameObject exp;
    public GameObject mini;
    float time = 0;
    ConstantForce cf;
    bool isEnd = false;
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Playing")
        {
            target = FindEnemy.Instance.target;
        }
        else
        {
            target = TFindEnemy.Instance.target;
        }
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
        if (time >= 30)
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
        if(collision.gameObject.tag != "Enemy")
        {
            Destroy(Instantiate(exp, transform.position, Quaternion.identity), 7);
            Destroy(gameObject);
        }
    }
}
