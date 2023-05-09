using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniObjFallow : MonoBehaviour
{
    public Transform target;
    void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, (10000 + target.position.y*0.01f), target.position.z);
            transform.rotation = Quaternion.Euler(0, target.eulerAngles.y, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
