using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 10);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            Destroy(transform.GetChild(0).gameObject, 6);
            transform.GetChild(0).parent = null;
            Destroy(gameObject);
        }
    }
}
