
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.VFX;

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
    public int turndir = 1;

    public float camSpeed = 9.0f; // 화면이 움직이는 속도 변수
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
    public VisualEffect sonic; // c
    public AudioSource shotBulletAudio;//c
    public AudioSource[] reload;//c
    public AudioSource sonicsound;//c
    public AudioSource boost;//c
    public AudioSource flaresound;
    float flaresTime = 0;
    int bulCount = 0;
    public Volume damagePost;
    public Image background;
    bool isDie = false;
    float sonicCool = 0;
    bool startsonic = false;
    bool over140 = false;
    bool doboost = false;
    private Tweener tweener;
    public GameObject esc;
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
        StartCoroutine("ShotBullet");
    }
    private void Update()
    {
        if (GameManager.Instance.gameOver == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                esc.SetActive(true);
            }
            velocity = Mathf.Sqrt(rigid.velocity.x * rigid.velocity.x + rigid.velocity.y * rigid.velocity.y + rigid.velocity.z * rigid.velocity.z);
            if(startsonic == false)
            {
                sonicCool += Time.deltaTime;
                if(sonicCool > 7)
                {
                    startsonic = true;
                    sonicCool = 0;
                }
            }
            else
            {
                if(velocity >= 151 && over140 == false)
                {
                    over140 = true;
                    sonicsound.Play();
                }
                if(over140 == true)
                {
                    if(velocity >= 151f)
                    {
                        sonic.Play();
                        StartCoroutine(StartTween());
                    }
                    else if(velocity >= 138f)
                    {
                        sonic.Stop();
                    }
                    else
                    {
                        sonic.Stop();
                        StartCoroutine(DelayStop());
                        over140 = false;
                        startsonic = false;
                        sonicCool = 0;
                    }
                }
            }
            if ((short)playState < 2)
            {
                if (canDie == false)
                {
                    DieCheck();
                }

                if (transform.position.y < 20)//바퀴 관리
                    tire.SetBool("IsSky", false);
                else
                    tire.SetBool("IsSky", true);


                CamTurn();//카메라 회전
                misTime += Time.deltaTime;
                sTime += Time.deltaTime;
                flaresTime += Time.deltaTime;
                States(); // 상태 판단
            }
        }
        if(isDie == true)
        {
            background.color = new Color(0, 0, 0, background.color.a + Time.deltaTime * 0.33f);
        }
    }
    IEnumerator DelayStop()
    {
        yield return new WaitForSeconds(0.1f);
        sonicsound.Stop();
    }
    private IEnumerator StartTween()
    {
        ShakeManager.Instance.Shake(1, 3);
        yield return null;
    }
    private void States()//각 상태에 따라 할 일;
    {
        if (playState == State2.FLY)
        {
            Sky();
            if (misTime >= 4)
            {
                if (FindEnemy.Instance.canShot == true)
                {
                    miss.SetBool("Shot", true);
                    misTime = 0;
                }
            }
            if(flaresTime > 5)
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
            flaresound.Play();
            flaresTime = 0;
        }
    }
    private void DieCheck()//죽을 수 있는 상태인지 판단
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
            pitch = -camSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime * 60; // 마우스y값을 지속적으로 받을 변수
            pitch = Mathf.Clamp(pitch, -1000, 1000);
            rigid.AddTorque(dir.right * pitch * turndir);
            if (PlayerPrefs.GetInt("control") == 0) {
                turn = -camSpeed * Input.GetAxis("Mouse X") * Time.deltaTime * 60; // 마우스로 방향조정
                turn = Mathf.Clamp(turn, -1000, 1000);
                rigid.AddTorque(dir.forward * turn);//끝
            }
        }
    }
    private IEnumerator ShotBullet()//총 쏨
    {
        while (true)
        {
            if (bulCount >= 150)
            {
                yield return new WaitForSeconds(7);
                bulCount = 0;
                reload[0].Stop();
            }
            else if (Input.GetMouseButton(0) && playState == State2.FLY)
            {
                shotBulletAudio.Play();
                Instantiate(bullet, bulPoint[0].position, child.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 3500);
                Instantiate(bullet, bulPoint[1].position, child.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 3500);
                bulCount++;
                if (bulCount >= 150)
                {
                    reload[0].Play();
                    reload[1].Play();
                }
                ShakeManager.Instance.Shake(0, 4);
                yield return new WaitForSeconds(Random.Range(0.02f, 0.05f));
            }
            yield return null;
        }
    }

    private void Sky()
    {
        if(!(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))) // 부스터 안쓰고있으면
        {
            rigid.AddForce(Vector3.down * 2500 * Time.deltaTime * 60);//떨어짐
        }
        if (Input.GetKey(KeyCode.W))
        {
            rigid.AddRelativeForce(Vector3.forward * speed * Time.deltaTime * 90 * power);
        }
        if (Input.GetKeyDown(KeyCode.C)) // 급상승
        {
            if (sTime >= 4)
            {
                rigid.AddTorque(dir.right * -90000);
                sTime = 0;
            }
        }
        if (PlayerPrefs.GetInt("control") == 1) {
            if (Input.GetKey(KeyCode.D))//회전
            {
                angle = Mathf.Lerp(angle, -400, Time.deltaTime * 5);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                angle = Mathf.Lerp(angle, 400, Time.deltaTime * 5);
            }
        }
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
        if (velocity >= 55)
        {
            rigid.useGravity = false;
        }
        else
        {
            rigid.useGravity = true;
        }
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
            if (doboost == false)
            {
                doboost = true;
                jet[0].Play();
                jet[1].Play();
            }
            if (tweener != null && tweener.IsActive())
            {
                tweener.Kill();
            }
            boost.volume = 0.13f;
        }
        else
        {
            if (doboost == true)
            {
                float currentValue = 0.13f;
                tweener = DOTween.To(() => currentValue, x => currentValue = x, 0, 0.3f).SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    boost.volume = currentValue;
                    
                });
            }
            doboost = false;
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
            Transform t = Instantiate(smoke, collision.contacts[0].point, Quaternion.identity).transform;
            t.parent = transform.GetChild(0);
            t.localScale = new Vector3(5, 5, 5);
            power -= 0.02f;
            damage++;
            damagePost.weight += (20-damage) * 0.00476f;
            Destroy(collision.gameObject);
            if (damage >= 20)
            {
                Die();
            }
        }
        if (canDie == true && collision.gameObject.tag != "EnemyBullet" && collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "EnemyMis")
        {
            Die();
        }
    }
    public void Die()//죽었을 때
    {
        if (g == null)//한번만 죽도록
        {
            playState = State2.DIE;
            g = Instantiate(dieParticel, transform.position, Quaternion.identity);
            g.transform.position = transform.position;
            g.transform.rotation = transform.rotation;
            Destroy(g, 6);
            g.GetComponentInChildren<Rigidbody>().velocity = rigid.velocity;
            rigid.isKinematic = true;
            model.SetActive(false);
            isDie = true;
            GameManager.Instance.gameOver = true;
            StartCoroutine(GameManager.Instance.ReStart());
            ShakeManager.Instance.ExpMe();
        }
    }
    public void ShotMis()// 미사일 발사
    {
        Instantiate(mis, misPos.position, dir.rotation * Quaternion.Euler(0, -90, 0)).GetComponent<Rigidbody>().velocity = rigid.velocity;
    }
    public void OnClickReturn()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        esc.SetActive(false);
    }
    public void OnClickDone()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        esc.SetActive(false);
        Die();
    }
}
