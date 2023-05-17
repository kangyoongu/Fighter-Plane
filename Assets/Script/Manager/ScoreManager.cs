using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TextMeshProUGUI main_Score;
    public TextMeshProUGUI main_Best;
    public TextMeshProUGUI play_Score;

    public int Score
    {
        get
        {
            return PlayerPrefs.GetInt("Score");
        }
        set
        {
            PlayerPrefs.SetInt("Score", value);
            main_Score.text = PlayerPrefs.GetInt("Score").ToString();
            play_Score.text = PlayerPrefs.GetInt("Score").ToString();
            HighScore = PlayerPrefs.GetInt("Score");
        }
    }
    public int HighScore
    {
        get
        {
            return PlayerPrefs.GetInt("HighScore");
        }
        set
        {
            if(PlayerPrefs.GetInt("HighScore") < value)
            {
                PlayerPrefs.SetInt("HighScore", value);
            }
            main_Best.text = PlayerPrefs.GetInt("HighScore").ToString();
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.SetInt("Score", 0);
        }
        Score = PlayerPrefs.GetInt("Score");
    }
}
