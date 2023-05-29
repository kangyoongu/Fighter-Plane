using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer s;
    public AudioMixer b;

    public Slider SfxSlider;
    public Slider BgmSlider;
    private void Start()
    {
        if (!PlayerPrefs.HasKey("bgm"))
        {
            PlayerPrefs.SetFloat("sfx", 4.5f);
            PlayerPrefs.SetFloat("bgm", 4.5f);
            PlayerPrefs.SetInt("control", 0);
        }
        SfxSlider.value = PlayerPrefs.GetFloat("sfx");
        BgmSlider.value = PlayerPrefs.GetFloat("bgm");
    }

    public void change()
    {
        s.SetFloat("SFX", Mathf.Log10(SfxSlider.value) * 20);
        PlayerPrefs.SetFloat("sfx", SfxSlider.value);
    }
    public void change2()
    {
        b.SetFloat("BGM", Mathf.Log10(BgmSlider.value) * 20);
        PlayerPrefs.SetFloat("bgm", BgmSlider.value);
    }
}