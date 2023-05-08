using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEnemy : MonoBehaviour
{
    public static FindEnemy Instance;
    public bool canShot = false;
    public Transform target;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            canShot = true;
            target = other.transform;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            canShot = false;
        }
    }
}
