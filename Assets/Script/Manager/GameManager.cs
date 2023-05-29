using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool gameOver = true;
    float playTime = 0;
    private TierManager tierManager;
    public int num = 0;
    public ConstantForce enemyForce;
    public EnemyAttack enemyAttack;
    float[] harder = { 0, 0, 0, 0 };
    public GameObject anim;
    public Sprite[] tier;
    public AudioSource aud;
    public void Awake()
    {
        PlayerPrefs.DeleteAll();
        if(Instance == null)
        {
            Instance = this;
        }
        Application.targetFrameRate = 160;
        tierManager = FindAnyObjectByType<TierManager>();
    }
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if(PlayerPrefs.GetInt("isUp") == 1)
        {
            anim.SetActive(true);
            anim.transform.GetChild(0).GetComponent<Image>().sprite = tier[tierManager.tierNum-1];
            anim.transform.GetChild(1).GetComponent<Image>().sprite = tier[tierManager.tierNum];
            PlayerPrefs.SetInt("isUp", 0);
        }
        if (!PlayerPrefs.HasKey("Tutorial") && SceneManager.GetActiveScene().name == "Playing")
        {
            SceneManager.LoadScene(1);
        }
        harder[0] = 5 + tierManager.tierNum * 2;
        harder[1] = 30 - tierManager.tierNum * 4;
        harder[2] = 20 - tierManager.tierNum;
        harder[3] = 4000 + tierManager.tierNum * 200;
    }
    private void Update()
    {
        print(tierManager.Tier);
        //tierManager.Tier = num;
        if(gameOver == false)
        {
            playTime = playTime < 300 ? playTime + Time.deltaTime : 300;
            EnemyMaker.maxEnemy = harder[0] * (1 + playTime * 0.0034f) - 1;
            EnemyMaker.makeTime = harder[1] * (1 - playTime * 0.00167f);
            enemyAttack.misCool = harder[2] * (1 - playTime * 0.0007f);
            enemyForce.relativeForce = new Vector3(harder[3] * (1 + playTime * 0.0007f), 0, 0);
        }
    }
    public void OnClickStart()
    {
        gameOver = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        CameraManager.Instance.MakePlayingView();
        ScoreManager.Instance.Score = 0;
        aud.Play();
        UIManager.Instance.StartPlay();
    }
    public IEnumerator ReStart()
    {
        yield return new WaitForSeconds(3);
        int t = tierManager.tierNum;
        switch (tierManager.tierNum)
        {
            case 0:
                if (tierManager.Tier >= 20)
                {
                    tierManager.Tier += Mathf.Clamp(ScoreManager.Instance.Score - 100, -20, 30);
                }
                else
                {
                    tierManager.Tier += Mathf.Clamp(ScoreManager.Instance.Score - 100, 0, 30);
                }
                break;
            case 1:
                tierManager.Tier += Mathf.Clamp(ScoreManager.Instance.Score - 120, -20, 30);
                break;
            case 2:
                tierManager.Tier += Mathf.Clamp(ScoreManager.Instance.Score - 200, -20, 30);
                break;
            case 3:
                tierManager.Tier += Mathf.Clamp(ScoreManager.Instance.Score - 320, -20, 30);
                break;
            case 4:
                tierManager.Tier += Mathf.Clamp(ScoreManager.Instance.Score - 500, -20, 30);
                break;
            case 5:
                tierManager.Tier += Mathf.Clamp(ScoreManager.Instance.Score - 500, -20, 30);
                break;
        }
        if(t < tierManager.tierNum)
        {
            PlayerPrefs.SetInt("isUp", 1);
        }
        FindEnemy.Instance.canShot = false;
        SceneManager.LoadScene(0);
    }
}
