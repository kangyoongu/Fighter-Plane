using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaker : MonoBehaviour
{
    public Transform[] points;
    public GameObject enemy;
    public static int enemyCount = 0;
    public static float maxEnemy = 0;
    public static float makeTime = 0;
    public float time = 100;
    private void Start()
    {
        enemyCount = 0;   
    }
    void Update()
    {
        time += Time.deltaTime;
        if(GameManager.Instance.gameOver == false)
        {
            if(enemyCount < maxEnemy)
            {
                if(enemyCount < maxEnemy-1)
                {
                    enemyCount += 1;
                    Instantiate(enemy, points[Random.Range(0, points.Length)].position, Quaternion.Euler(-90, 0, 0));
                }
                else if (time >= makeTime)
                {
                    enemyCount += 1;
                    Instantiate(enemy, points[Random.Range(0, points.Length)].position, Quaternion.Euler(-90, 0, 0));
                    time = 0;
                }
            }
        }
    }
}
