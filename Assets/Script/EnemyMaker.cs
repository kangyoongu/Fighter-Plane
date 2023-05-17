using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaker : MonoBehaviour
{
    public Transform[] points;
    public GameObject enemy;
    public static int enemyCount = 0;
    float time = 100;
    void Update()
    {
        time += Time.deltaTime;
        if(GameManager.Instance.gameOver == false)
        {
            if(enemyCount < 5)
            {
                if(enemyCount < 4)
                {
                    enemyCount += 1;
                    Instantiate(enemy, points[Random.Range(0, points.Length)].position, Quaternion.Euler(-90, 0, 0));
                }
                else if (time >= 30)
                {
                    enemyCount += 1;
                    Instantiate(enemy, points[Random.Range(0, points.Length)].position, Quaternion.Euler(-90, 0, 0));
                    time = 0;
                }
            }
        }
    }
}
