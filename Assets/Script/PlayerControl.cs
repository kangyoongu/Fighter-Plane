
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

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
    float angle = 0;

    public GameObject bullet;
    public Transform[] bulPoint;//c
    [SerializeField]
    private GameObject dieParticel;

    bool canDie = false;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (canDie == false)
            DieCheck();
        velocity = Mathf.Sqrt(rigid.velocity.x * rigid.velocity.x + rigid.velocity.y * rigid.velocity.y + rigid.velocity.z * rigid.velocity.z);
        Debug.Log(velocity);
        if (enter == 0)
        {
            Sky();
        }
        else
        {
            Ground();
        }
        CamTurn();
        StartCoroutine(ShotBullet());
    }
    void DieCheck()
    {
        if (transform.position.y >= 20)
        {
            canDie = true;
        }
    }
    void CamTurn()
    {
        if (rigid.useGravity == false)
        {
            pitch = -camSpeed * Input.GetAxis("Mouse Y"); // 마우스y값을 지속적으로 받을 변수
            rigid.AddTorque(dir.right * pitch);
        }
    }
    IEnumerator ShotBullet()
    {
        if (Input.GetMouseButton(0))
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            Instantiate(bullet, bulPoint[0].position, child.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 5000);
            Instantiate(bullet, bulPoint[1].position, child.rotation * Quaternion.Euler(90, 0, 0)).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * 5000);
        }
        yield return null;
    }

    private void Sky()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigid.AddRelativeForce(Vector3.forward * speed * Time.deltaTime * 90);
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
            rigid.AddRelativeForce(Vector3.forward * speed * Time.deltaTime * 70);
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
        if (velocity > 50 && canDie == true)
        {
            StartCoroutine(Die());
        }
        enter++;
        
    }
    public void OnCollisionExit(Collision collision)
    {
        enter--;
    }
    IEnumerator Die()
    {
        GameObject g = Instantiate(dieParticel, transform.position, Quaternion.identity);
        Destroy(g, 6);
        foreach(Rigidbody par in g.GetComponentsInChildren<Rigidbody>())
        {
            par.velocity = rigid.velocity;
        }
        rigid.isKinematic = true;
        model.SetActive(false);
        yield return new WaitForSecondsRealtime(3);
    }
}
