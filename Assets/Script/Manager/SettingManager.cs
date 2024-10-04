using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;
    public RectTransform background;
    public bool control = false;
    public TextMeshProUGUI conText;
    public AudioSource click;
    public RectTransform canvas;
    public bool withkey = false;
    Tweener set;
    Vector3[] pos = { new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
    private void Awake()
    {
        pos[1].y = -canvas.sizeDelta.y - 100;
        background.anchoredPosition = pos[1];
        if (Instance == null)
        {
            Instance = this;
        }
        if (PlayerPrefs.GetInt("control") == 0)
            conText.text = "Rotate with mouse";
        else
            conText.text = "Rotate with A, D";
    }
    public void OnClickTutorial()
    {
        SceneManager.LoadScene(1);
    }
    public void OnClickSetting()
    {
        if(set != null && set.IsActive()) { set.Kill(); }
        set = background.DOAnchorPos(pos[0], 0.5f);
        click.Play();
    }
    public void OnClickX()
    {
        if (set != null && set.IsActive()) { set.Kill(); }
        set = background.DOAnchorPos(pos[1], 0.5f);
        click.Play();
    }
    public void OnClickMirror()
    {
        click.Play();
        PlayerPrefs.SetInt("diraction", PlayerPrefs.GetInt("diraction") * -1);
        PlayerControl.Instance.turndir = PlayerPrefs.GetInt("diraction");
    }
    public void OnClickControl()
    {
        click.Play();
        if (PlayerPrefs.GetInt("control") == 1)
        {
            PlayerPrefs.SetInt("control", 0);
            conText.text = "Rotate with mouse";
        }
        else
        {
            PlayerPrefs.SetInt("control", 1);
            conText.text = "Rotate with A, D";
        }
        print(PlayerPrefs.GetInt("control"));
    }
}
