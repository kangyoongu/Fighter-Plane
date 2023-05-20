using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool gameOver = true;
    float playTime = 0;
    private TierManager tierManager;
    public int num = 0;
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        Application.targetFrameRate = 60;
        tierManager = FindAnyObjectByType<TierManager>();
    }
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (!PlayerPrefs.HasKey("Tutorial") && SceneManager.GetActiveScene().name == "Playing")
        {
            SceneManager.LoadScene(1);
        }
        EnemyMaker.maxEnemy = 5 + tierManager.tierNum * 2;
        EnemyMaker.makeTime = 30 - tierManager.tierNum * 4;

    }
    private void Update()
    {
        tierManager.Tier = num;
        if(gameOver == false)
        {
            playTime += Time.deltaTime;
            Debug.Log(playTime);
        }
    }
    public void OnClickStart()
    {
        gameOver = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        CameraManager.Instance.MakePlayingView();
        ScoreManager.Instance.Score = 0;
        UIManager.Instance.StartPlay();
    }
    public IEnumerator ReStart()
    {
        yield return new WaitForSeconds(3);
        FindEnemy.Instance.canShot = false;
        SceneManager.LoadScene(0);
    }
}
