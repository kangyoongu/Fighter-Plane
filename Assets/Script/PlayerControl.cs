
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;
public enum State2 : short
{
    GROUND = 0,
    FLY = 1,
    DIE = 2
}
public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance;
    public float speed = 2;
    private Rigidbody rigid;
    float velocity = 0;
    public VisualEffect[] jet;
    int damage = 0;

    public float camSpeed = 9.0f; // ȭ���� �����̴� �ӵ� ����
    float pitch = 0;
    float turn = 0;
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
    public State2 playState = State2.GROUND;
    bool canDie = false;
    public GameObject mis;
    float misTime = 0;
    private GameObject g = null;
    public GameObject smoke;
    float power = 1;
    float sTime = 0;
    public ParticleSystem[] flares;//c
    float flaresTime = 0;
    int bulCount = 0;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        bulCount = 0;
        flaresTime = 0;
        power = 1;
        angle = 0;
        damage = 0;
    }
    private void Update()
    {
        Debug.Log(bulCount);
        if (GameManager.Instance.gameOver == false)
        {
            velocity = Mathf.Sqrt(rigid.velocity.x * rigid.velocity.x + rigid.velocity.y * rigid.velocity.y + rigid.velocity.z * rigid.velocity.z);
            if ((short)playState < 2)
            {
                if (canDie == false)
                {
                    DieCheck();
                }

                if (transform.position.y < 20)//���� ����
                    tire.SetBool("IsSky", false);

                CamTurn();//ī�޶� ȸ��
                misTime += Time.deltaTime;
                sTime += Time.deltaTime;
                flaresTime += Time.deltaTime;
                States(); // ���� �Ǵ�
            }
        }
    }
    private void States()//�� ���¿� ���� �� ��;
    {
        if (playState == State2.FLY)
        {
            Sky();
            StartCoroutine("ShotBullet");
            if (misTime >= 4)
            {
                if (FindEnemy.Instance.canShot == true)
                {
                    miss.SetBool("Shot", true);
                    misTime = 0;
                }
            }
            if(flaresTime > 6)
            {
                ShotFlares();
            }
        }
        else if (playState == State2.GROUND)
        {
            Ground();
            tire.SetBool("IsSky", false);
        }
    }
    private void ShotFlares()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flares[0].Play();
            flares[1].Play();
            flaresTime = 0;
        }
    }
    private void DieCheck()//���� �� �ִ� �������� �Ǵ�
    {
        if (transform.position.y >= 20)
        {
            canDie = true;
            playState = State2.FLY;
            tire.SetBool("IsSky", true);
        }
    }
    private void CamTurn()
    {
        if (velocity >= 40)
        {
            pitch = -camSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime * 60; // ���콺y���� ���������� ���� ����
            pitch = Mathf.Clamp(pitch, -900, 900);
            rigid.AddTorque(dir.right * pitch);
            turn = -camSpeed * Input.GetAxis("Mouse X") * Time.deltaTime * 60; // ���콺�� ��������
            turn = Mathf.Clamp(turn, -900, 900);
            rigid.AddTorque(dir.forward * turn);//��
        }
    }
    private IEnumerator ShotBullet()//�� ��
    {
        if(bulCount >= 150)
        {
            yield return new WaitForSeconds(7);
            bulCount = 0;
            StopCoroutine("ShotBullet");
        }
        else if (Input.GetMouseButton(0))
        {
            yield return new WaitForSeconds(Random.Range(0.2f, 0.3f));
            Instantiate(bullet, bulPoint[0].position, child.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 3500);
            Instantiate(bullet, bulPoint[1].position, child.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 3500);
            bulCount++;
        }
        yield return null;
    }

    private void Sky()
    {
        if(!(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))) // �ν��� �Ⱦ���������
        {
            rigid.AddForce(Vector3.down * 1500);//������
        }
        if (Input.GetKey(KeyCode.W))
        {
            rigid.AddRelativeForce(Vector3.forward * speed * Time.deltaTime * 90 * power);
        }
        if (Input.GetKeyDown(KeyCode.S)) // �޻��
        {
            if (sTime >= 4)
            {
                rigid.AddTorque(dir.right * -90000);
                sTime = 0;
            }
        }
        /*if (Input.GetKey(KeyCode.D))//ȸ��
        {
            angle = Mathf.Lerp(angle, -100, Time.deltaTime * 5);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            angle = Mathf.Lerp(angle, 100, Time.deltaTime * 5);
        }*/
        if (Input.GetKey(KeyCode.Q))
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
        if (velocity >= 60)
        {
            rigid.useGravity = false;
        }
        else
        {
            rigid.useGravity = true;
        }
    }

    private void Ground()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed * 0.01f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * -speed * 0.01f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * -speed * Time.deltaTime * 0.015f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * speed * Time.deltaTime * 0.015f);
        }
        Shift();
    }
    private void Shift()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            rigid.AddRelativeForce(Vector3.forward * speed * Time.deltaTime * 70 * power);
            jet[0].Play();
            jet[1].Play();
        }
        else
        {
            jet[0].Stop();
            jet[1].Stop();
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
            if(damage >= 20)
            {
                Die();
            }
            Transform t = Instantiate(smoke, collision.contacts[0].point, Quaternion.identity).transform;
            t.parent = transform.GetChild(0);
            t.localScale = new Vector3(5, 5, 5);
            power -= 0.04f;
            damage++;
        }
        if (canDie == true && collision.gameObject.tag != "EnemyBullet" && collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "EnemyMis")
        {
            Die();
        }
    }
    public void Die()//�׾��� ��
    {
        if (g == null)//�ѹ��� �׵���
        {
            playState = State2.DIE;
            g = Instantiate(dieParticel, transform.position, Quaternion.identity);
            g.transform.position = transform.position;
            g.transform.rotation = transform.rotation;
            Destroy(g, 6);
            g.GetComponentInChildren<Rigidbody>().velocity = rigid.velocity;
            rigid.isKinematic = true;
            model.SetActive(false);
            GameManager.Instance.gameOver = true;
            StartCoroutine(GameManager.Instance.ReStart());
        }
    }
    public void ShotMis()// �̻��� �߻�
    {
        Instantiate(mis, misPos.position, dir.rotation * Quaternion.Euler(0, -90, 0)).GetComponent<Rigidbody>().velocity = rigid.velocity;
    }
}
