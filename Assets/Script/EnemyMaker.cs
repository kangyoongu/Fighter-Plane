using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaker : MonoBehaviour
{
    float time = 40;
    public Transform[] points;
    public GameObject enemy;
    void Update()
    {
        if(GameManager.Instance.gameOver == false)
        {
            time += Time.deltaTime;
            if(time >= 4)
            {
                Instantiate(enemy, points[Random.Range(0, points.Length)].position, Quaternion.Euler(-90, 0, 0));
                time = 0;
            }
        }
    }
}
