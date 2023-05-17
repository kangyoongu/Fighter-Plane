using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject playingCanvas;
    public GameObject MainCanvas;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void StartPlay()
    {
        playingCanvas.SetActive(true);
        MainCanvas.SetActive(false);
    }
}
