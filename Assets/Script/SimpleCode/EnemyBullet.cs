using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 15);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
