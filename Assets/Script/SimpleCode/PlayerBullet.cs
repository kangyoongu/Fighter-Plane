using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public GameObject effect;
    private void Start()
    {
        Destroy(gameObject, 8);
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
