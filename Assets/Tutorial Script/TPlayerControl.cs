
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;
public enum TState : short
{
    GROUND = 0,
    FLY = 1,
    DIE = 2
}
public class TPlayerControl : MonoBehaviour
{
    public static TPlayerControl Instance;
    public float speed = 2;
    private Rigidbody rigid;
    float velocity = 0;
    public VisualEffect[] jet;
    int damage = 0;

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
    public TState playState = TState.GROUND;
    bool canDie = false;
    public GameObject mis;
    float misTime = 0;
    private GameObject g = null;
    public GameObject smoke;
    float power = 1;
    float sTime = 0;
    public ParticleSystem[] flares;//c
    float flaresTime = 0;
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        velocity = Mathf.Sqrt(rigid.velocity.x * rigid.velocity.x + rigid.velocity.y * rigid.velocity.y + rigid.velocity.z * rigid.velocity.z);
        if(TutorialManager.Instance.level == 2)
        {
            if(velocity >= 50)
            {
                StartCoroutine(TutorialManager.Instance.ThreeSet());
            }
        }
        if ((short)playState < 2)
        {
            if (canDie == false)
            {
                DieCheck();
            }

            if(transform.position.y < 20)//바퀴 관리
                tire.SetBool("IsSky", false);

            CamTurn();//카메라 회전
            if (TutorialManager.Instance.level >= 4)
            {
                StartCoroutine(ShotBullet());
            }
            misTime += Time.deltaTime; 
            sTime += Time.deltaTime;
            flaresTime += Time.deltaTime;
            States(); // 상태 판단
        }
    }
    private void States()//각 상태에 따라 할 일;
    {
        if (playState == TState.FLY)
        {
            if (TutorialManager.Instance.level >= 3)
            {
                if (TutorialManager.Instance.level == 3)
                {
                    StartCoroutine(TutorialManager.Instance.FourSet());
                }
                Sky();
                if (misTime >= 4)
                {
                    if (FindEnemy.Instance != null && FindEnemy.Instance.canShot == true)
                    {
                        miss.SetBool("Shot", true);
                        misTime = 0;
                    }
                }
                if (flaresTime > 6)
                {
                    ShotFlares();
                }
            }
        }
        else if (playState == TState.GROUND)
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
    private void DieCheck()//죽을 수 있는 상태인지 판단
    {
        if (transform.position.y >= 20)
        {
            canDie = true;
            playState = TState.FLY;
            tire.SetBool("IsSky", true);
        }
    }
    private void CamTurn()
    {
        if (rigid.useGravity == false)
        {
            pitch = -camSpeed * Input.GetAxis("Mouse Y"); // 마우스y값을 지속적으로 받을 변수
            pitch = Mathf.Clamp(pitch, -420, 420);
            rigid.AddTorque(dir.right * pitch);
        }
    }
    private IEnumerator ShotBullet()//총 쏨
    {
        if (Input.GetMouseButton(0))
        {
            yield return new WaitForSeconds(Random.Range(0.03f, 0.05f));
            Instantiate(bullet, bulPoint[0].position, child.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 4000);
            Instantiate(bullet, bulPoint[1].position, child.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 4000);
        }
        yield return null;
    }

    private void Sky()
    {
        if (TutorialManager.Instance.isStop == false)
        {
            if (!(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))) // 부스터 안쓰고있으면
            {
                rigid.AddForce(Vector3.down * 300);//떨어짐
            }
            if (Input.GetKey(KeyCode.W))
            {
                rigid.AddRelativeForce(Vector3.forward * speed * Time.deltaTime * 90 * power);
            }
            if (Input.GetKeyDown(KeyCode.S)) // 급상승
            {
                if (sTime >= 4)
                {
                    rigid.AddTorque(dir.right * -28000);
                    sTime = 0;
                }
            }
            if (Input.GetKey(KeyCode.D))//회전
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
    }

    private void Ground()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed * 0.06f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * -speed * 0.06f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up * -speed * Time.deltaTime * 0.1f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * speed * Time.deltaTime * 0.1f);
        }
        Shift();
    }
    private void Shift()
    {
        if (TutorialManager.Instance.level >= 2)
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
                StartCoroutine(Die());
            }
            Transform t = Instantiate(smoke, collision.contacts[0].point, Quaternion.identity).transform;
            t.parent = transform.GetChild(0);
            t.localScale = new Vector3(5, 5, 5);
            power -= 0.04f;
            damage++;
            Destroy(collision.gameObject);
        }
        if (velocity > 50 && canDie == true && collision.gameObject.tag != "EnemyBullet" && collision.gameObject.tag != "Enemy")
        {
            StartCoroutine(Die());
        }
        /*else if(velocity <= 50)
        {
            canDie = false;
            playState = State.GROUND;
        }*/
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "p1")
        {
            TutorialManager.Instance.level = 2;
            other.gameObject.SetActive(false);
            TutorialManager.Instance.TwoSet();
        }
        if (other.gameObject.name == "p2")
        {
            other.gameObject.SetActive(false);
            StopCoroutine(TutorialManager.Instance.FourSet());
            StartCoroutine(TutorialManager.Instance.FiveSet());
        }
    }
    public IEnumerator Die()//죽었을 때
    {
        if (g == null)//한번만 죽도록
        {
            playState = TState.DIE;
            g = Instantiate(dieParticel, transform.position, Quaternion.identity);
            g.transform.position = transform.position;
            g.transform.rotation = transform.rotation;
            Destroy(g, 3);
            g.GetComponentInChildren<Rigidbody>().velocity = rigid.velocity;
            rigid.isKinematic = true;
            model.SetActive(false);
            yield return new WaitForSecondsRealtime(3);
            if (TutorialManager.Instance.level < 6)
            {
                canDie = false;
                TutorialManager.Instance.Restart();
                model.SetActive(true);
                StopAllCoroutines();
                model.transform.localRotation = Quaternion.Euler(0, 0, 0);
                rigid.isKinematic = false;
            }
        }
        else
        {
            yield return null;
        }
    }
    public void ShotMis()//히히 미사일 발사
    {
        Instantiate(mis, misPos.position, dir.rotation * Quaternion.Euler(0, -90, 0)).GetComponent<Rigidbody>().velocity = rigid.velocity;
    }
}
