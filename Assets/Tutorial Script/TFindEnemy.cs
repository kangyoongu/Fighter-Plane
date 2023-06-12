using UnityEngine;

public class TFindEnemy : MonoBehaviour
{
    public static TFindEnemy Instance;
    public bool canShot = false;
    public Transform target;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * 100);
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
