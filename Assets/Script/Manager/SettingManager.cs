using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;
    public GameObject background;
    public bool control = false;
    public TextMeshProUGUI conText;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void OnClickSetting()
    {
        background.SetActive(true);
    }
    public void OnClickX()
    {
        background.SetActive(false);
    }
    public void OnClickControl()
    {
        control = !control;
        if (control == false)
            conText.text = "마우스로 회전";
        else
            conText.text = "A, D로 회전";
    }
}
