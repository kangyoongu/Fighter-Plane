using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TierManager : MonoBehaviour
{
    public GameObject showTier;
    public TextMeshProUGUI persent;
    public TextMeshProUGUI level;
    public TextMeshProUGUI tt;
    public Image tierBar;
    public GameObject[] butStyle;
    private int tier;
    int[] weight = {100, 110, 120, 130, 140, 150};
    int upPoint = 100;
    string[] rom = { "V", "IV", "III", "II", "I" };
    Color32[] tierColor = {new Color32(180, 91, 60, 255), new Color32(165, 164, 173, 255), new Color32(224, 179, 0, 255), new Color32(55, 229, 229, 255), new Color32(80, 101, 255, 255)};
    Color32[] outlineColor = { new Color32(116, 27, 12, 255), new Color32(96, 95, 107, 255), new Color32(190, 115, 0, 255), new Color32(10, 202, 202, 255), new Color32(20, 33, 255, 255)};
    Color32[] inColor = { new Color32(132, 59, 35, 255), new Color32(128, 127, 135, 255), new Color32(179, 143, 0, 255), new Color32(14, 186, 186, 255), new Color32(53, 72, 204, 255)};
    public GameObject[] tierImage;
    public int tierNum;
    public int Tier
    {
        get
        {
            tier = PlayerPrefs.GetInt("Tier");
            return tier;
        }
        set
        {
            tier = value;
            PlayerPrefs.SetInt("Tier", tier);
            int val = 0;
            bool end = true;
            for(int i = 0; i < 6; i++)
            {
                if (val > tier)
                {
                    upPoint = val;
                    tierNum = i - 1;
                    tierBar.fillAmount = (float)(tier - (upPoint - weight[tierNum])) / weight[tierNum];
                    tierBar.color = tierColor[tierNum];
                    level.outlineColor = outlineColor[tierNum];
                    persent.outlineColor = outlineColor[tierNum];
                    level.color = inColor[tierNum];
                    tt.color = tierColor[tierNum];
                    end = false;
                    for(int x = 0; x < 6; x++)
                    {
                        tierImage[x].SetActive(false);
                    }
                    tierImage[tierNum].SetActive(true);
                    break;
                }
                for (int j = 0; j < 5; j++)
                {
                    val += weight[i];
                    if (val > tier)
                    {
                        level.text = rom[j];
                        break;
                    }
                }
            }
            if (end == false)
            {
                persent.text = tier.ToString() + "/" + upPoint;
                butStyle[0].SetActive(true);
                butStyle[1].SetActive(false);
            }
            else
            {
                persent.text = tier.ToString();
                for (int x = 0; x < 6; x++)
                {
                    tierImage[x].SetActive(false);
                }
                tierImage[5].SetActive(true);
                butStyle[0].SetActive(false);
                butStyle[1].SetActive(true);
                level.text = "";
                tierNum = 5;
            }
        }
    }
    private void Awake()
    {
        PlayerPrefs.DeleteKey("Tier");
        if (!PlayerPrefs.HasKey("Tier"))
        {
            PlayerPrefs.SetInt("Tier", 0);
        }
        Tier = Tier;
    }
    public void OnClickTierOn()
    {
        showTier.SetActive(true);
    }
    public void OnClickTierOff()
    {
        showTier.SetActive(false);
    }
}