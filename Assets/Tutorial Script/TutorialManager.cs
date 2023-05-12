using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public GameObject[] UIs;
    public GameObject[] point;
    public int level = 0;
    public TextMeshProUGUI[] time;
    public GameObject playerRange;
    public GameObject player;
    private Rigidbody rigid;
    private Transform playerPos;
    public bool isStop = false;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        playerPos = player.transform;
        rigid = player.GetComponent<Rigidbody>();
        Time.timeScale = 0;
        StartCoroutine(OneSet());
    }
    void Update()
    {
        
    }
    IEnumerator OneSet()
    {
        UIs[0].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        UIs[0].SetActive(false);
        UIs[1].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        UIs[1].SetActive(false);
        UIs[2].SetActive(true);
        point[0].SetActive(true);
        level = 1;
        Time.timeScale = 1;
    }
    public void TwoSet()
    {
        UIs[2].SetActive(false);
        UIs[3].SetActive(true);
    }
    public IEnumerator ThreeSet()
    {
        level = 3;
        Time.timeScale = 0;
        UIs[3].SetActive(false);
        UIs[4].SetActive(true);
        time[0].text = "3";
        yield return new WaitForSecondsRealtime(1);
        time[0].text = "2";
        yield return new WaitForSecondsRealtime(1);
        time[0].text = "1";
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        UIs[4].SetActive(false);
    }
    public IEnumerator FourSet()
    {
        level = 4;
        UIs[5].SetActive(true);
        yield return new WaitForSecondsRealtime(4);
        UIs[5].SetActive(false);
        UIs[6].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        UIs[6].SetActive(false);
        UIs[7].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        UIs[7].SetActive(false);
        UIs[8].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        UIs[8].SetActive(false);
        UIs[9].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        UIs[9].SetActive(false);
        UIs[10].SetActive(true);
    }
    public IEnumerator FiveSet()
    {
        level = 5;
        Time.timeScale = 0;
        isStop = true;
        UIs[10].SetActive(false);
        UIs[11].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        UIs[11].SetActive(false);
        UIs[12].SetActive(true);
        playerRange.SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        UIs[12].SetActive(false);
        UIs[13].SetActive(true);
        time[1].text = "3";
        yield return new WaitForSecondsRealtime(1);
        time[1].text = "2";
        yield return new WaitForSecondsRealtime(1);
        time[1].text = "1";
        yield return new WaitForSecondsRealtime(1);
        UIs[13].SetActive(false);
        rigid.velocity = Vector3.zero;
        isStop = false;
        Time.timeScale = 1;
    }
    public IEnumerator SixSet()
    {
        level = 6;
        UIs[14].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        StopAllCoroutines();
        rigid.velocity = Vector3.zero;
        for (int i = 0; i < UIs.Length; i++)
        {
            UIs[i].SetActive(false);
        }
        if (level >= 2 && level <= 4)
        {
            TPlayerControl.Instance.playState = TState.GROUND;
            playerPos.position = point[0].transform.position;
            playerPos.rotation = point[0].transform.rotation;
            level = 2;
            TwoSet();
        }
        else if(level == 5)
        {
            TPlayerControl.Instance.playState = TState.FLY;
            playerPos.position = point[1].transform.position;
            playerPos.rotation = point[1].transform.rotation;
        }
    }
}
