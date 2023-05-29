using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;
    public GameObject background;
    public bool control = false;
    public TextMeshProUGUI conText;
    public AudioSource click;
    public bool withkey = false;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        if (PlayerPrefs.GetInt("control") == 0)
            conText.text = "���콺�� ȸ��";
        else
            conText.text = "A, D�� ȸ��";
    }
    public void OnClickTutorial()
    {
        SceneManager.LoadScene(1);
    }
    public void OnClickSetting()
    {
        background.SetActive(true);
        click.Play();
    }
    public void OnClickX()
    {
        background.SetActive(false);
        click.Play();
    }
    public void OnClickControl()
    {
        click.Play();
        if (PlayerPrefs.GetInt("control") == 1)
        {
            PlayerPrefs.SetInt("control", 0);
            conText.text = "���콺�� ȸ��";
        }
        else
        {
            PlayerPrefs.SetInt("control", 1);
            conText.text = "A, D�� ȸ��";
        }
        print(PlayerPrefs.GetInt("control"));
    }
}
