using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int score = 0;
    public int Score
    {
        get
        {
            score = PlayerPrefs.GetInt("Score");
            return score;
        }
        set
        {
            score = value;
            PlayerPrefs.SetInt("Score", score);
        }
    }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
