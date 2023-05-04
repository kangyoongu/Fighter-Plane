using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class PlayerControl : MonoBehaviour
{
    public float speed = 2;
    private Rigidbody rigid;
    float velocity = 0;
    int enter = 0;
    public VisualEffect[] jet;

    public float camSpeed = 9.0f; // 화면이 움직이는 속도 변수
    float pitch = 0;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if(enter == 0)
        {
            Sky();
        }
        else
        {
            Ground();
        }
        CamTurn();
    }
    void CamTurn()
    {
        if (rigid.useGravity == false)
        {
            pitch = -camSpeed * Input.GetAxis("Mouse Y"); // 마우스y값을 지속적으로 받을 변수
            rigid.AddRelativeTorque(Vector3.right * pitch);
        }
    }

    private void Sky()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigid.AddRelativeForce(Vector3.forward * speed * Time.deltaTime * 90);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetAngle(-90);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            SetAngle(0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetAngle(90);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            SetAngle(0);
        }
        Shift();
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
        velocity = Mathf.Sqrt(rigid.velocity.x * rigid.velocity.x + rigid.velocity.y * rigid.velocity.y + rigid.velocity.z * rigid.velocity.z);
        if (velocity >= 50)
        {
            rigid.useGravity = false;
        }
        else
        {
            rigid.useGravity = true;
        }
    }
    void SetAngle(int angle)
    {
        transform.DOLocalRotate(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle), 2).SetEase(Ease.OutQuad);
        transform.rotation = Quaternion.
    }
    public void OnCollisionEnter(Collision collision)
    {
        enter++;
    }
    public void OnCollisionExit(Collision collision)
    {
        enter--;
    }
}
