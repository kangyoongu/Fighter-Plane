using UnityEngine;

public class MiniObjFallow : MonoBehaviour
{
    public Transform target;
    void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, (8000 + target.position.y*0.01f), target.position.z);
            transform.rotation = Quaternion.Euler(0, target.eulerAngles.y, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
