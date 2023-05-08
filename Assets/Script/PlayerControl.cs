
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public enum State : short
{
    GROUND = 0,
    FLY = 1,
    DIE = 2
}
public class PlayerControl : MonoBehaviour
{
    public float speed = 2;
    private Rigidbody rigid;
    float velocity = 0;
    int enter = 0;
    public VisualEffect[] jet;

    public float camSpeed = 9.0f; // 화면이 움직이는 속도 변수
    float pitch = 0;
    public Transform child;//c
    public Transform dir;//c
    public GameObject model;//c
    public Animator tire;//c
    public Animator miss;//c
    public Transform misPos;//c
    float angle = 0;

    public GameObject bullet;
    public Transform[] bulPoint;//c
    [SerializeField]
    private GameObject dieParticel;
    public State playState = State.GROUND;
    bool canDie = false;
    public GameObject mis;
    float mistime = 0;
    private GameObject g = null;
    public GameObject smoke;
    float power = 1;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        velocity = Mathf.Sqrt(rigid.velocity.x * rigid.velocity.x + rigid.velocity.y * rigid.velocity.y + rigid.velocity.z * rigid.velocity.z);
        if ((short)playState < 2)
        {
            if (canDie == false)
            {
                DieCheck();
            }

            if(transform.position.y < 20)
                tire.SetBool("IsSky", false);

            if (enter == 0)
                Sky();
            else
                Ground();

            CamTurn();
            StartCoroutine(ShotBullet());
            mistime += Time.deltaTime;
            if(playState == State.FLY)
            {
                if (mistime >= 6)
                {
                    if (FindEnemy.Instance.canShot == true)
                    {
                        miss.SetBool("Shot", true);
                        mistime = 0;
                    }
                }
            }
            else if(playState == State.GROUND)
            {
                tire.SetBool("IsSky", false);
            }
        }
    }
    void DieCheck()
    {
        if (transform.position.y >= 20)
        {
            canDie = true;
            playState = State.FLY;
            tire.SetBool("IsSky", true);
        }
    }
    void CamTurn()
    {
        if (rigid.useGravity == false)
        {
            pitch = -camSpeed * Input.GetAxis("Mouse Y"); // 마우스y값을 지속적으로 받을 변수
            pitch = Mathf.Clamp(pitch, -220, 220);
            rigid.AddTorque(dir.right * pitch);
        }
    }
    IEnumerator ShotBullet()
    {
        if (Input.GetMouseButton(0))
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            Instantiate(bullet, bulPoint[0].position, child.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 4000);
            Instantiate(bullet, bulPoint[1].position, child.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 4000);
        }
        yield return null;
    }

    private void Sky()
    {
        if(!(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)))
        {
            rigid.AddForce(Vector3.down * 300);
        }
        if (Input.GetKey(KeyCode.W))
        {
            rigid.AddRelativeForce(Vector3.forward * speed * Time.deltaTime * 90 * power);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            rigid.AddTorque(dir.right * -30000);
        }
        if (Input.GetKey(KeyCode.D))
        {
            angle = Mathf.Lerp(angle, -100, Time.deltaTime * 5);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            angle = Mathf.Lerp(angle, 100, Time.deltaTime * 5);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            angle = Mathf.Lerp(angle, 700, Time.deltaTime * 5);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            angle = Mathf.Lerp(angle, -700, Time.deltaTime * 5);
        }
        else
        {
            angle = Mathf.Lerp(angle, 0, Time.deltaTime * 5);
        }
        Shift();
        if (angle != 0)
        {
            SetAngle();
        }
    }

    private void Ground()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed * 0.03f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * -speed * 0.03f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * -speed * Time.deltaTime * 0.07f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * speed * Time.deltaTime * 0.07f);
        }
        Shift();
    }
    void Shift()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            rigid.AddRelativeForce(Vector3.forward * speed * Time.deltaTime * 80 * power);
            jet[0].Play();
            jet[1].Play();
        }
        else
        {
            jet[0].Stop();
            jet[1].Stop();
        }
        if (velocity >= 50)
        {
            rigid.useGravity = false;
        }
        else
        {
            rigid.useGravity = true;
        }
    }
    void SetAngle()
    {
        child.localRotation = Quaternion.Euler(0, 0, angle * Time.deltaTime  + child.localEulerAngles.z);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "EnemyBullet")
        {
            Transform t = Instantiate(smoke, collision.contacts[0].point, Quaternion.identity).transform;
            t.parent = transform;
            t.localScale = new Vector3(5, 5, 5);
            if(power >= 0)
            {
                power -= 0.03f;
            }

        }
        if (velocity > 50 && canDie == true && collision.gameObject.tag != "EnemyBullet")
        {
            StartCoroutine(Die());
        }
        else if(velocity <= 50)
        {
            canDie = false;
            playState = State.GROUND;
        }
        enter++;
        
    }
    public void OnCollisionExit(Collision collision)
    {
        enter--;
    }
    IEnumerator Die()
    {
        if (g == null)
        {
            playState = State.DIE;
            g = Instantiate(dieParticel, transform.position, Quaternion.identity);
            g.transform.position = transform.position;
            g.transform.rotation = transform.rotation;
            Destroy(g, 6);
            g.GetComponentInChildren<Rigidbody>().velocity = rigid.velocity;
            rigid.isKinematic = true;
            model.SetActive(false);
            yield return new WaitForSecondsRealtime(3);
        }
        else
        {
            yield return null;
        }
    }
    public void ShotMis()
    {
        Instantiate(mis, misPos.position, dir.rotation * Quaternion.Euler(0, -90, 0)).GetComponent<Rigidbody>().velocity = rigid.velocity;

    }
}
