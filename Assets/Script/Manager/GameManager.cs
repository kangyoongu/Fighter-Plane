using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool gameOver = true;
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (!PlayerPrefs.HasKey("Tutorial") && SceneManager.GetActiveScene().name == "Playing")
        {
            SceneManager.LoadScene(1);
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
       // PlayerControl.Instance.tire.SetBool("IsSky", true);
       // PlayerControl.Instance.miss.SetBool("Shot", false);
        FindEnemy.Instance.canShot = false;
        SceneManager.LoadScene(0);
    }
}
