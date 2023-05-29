using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HitEnemy : MonoBehaviour
{
    public static HitEnemy Instance;
    [SerializeField] private Volume cam;
    private ColorAdjustments vol;
    private Tweener tweener;
    private void Awake()
    {
        cam.profile.TryGet(out vol);
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void hitEnemy()
    {
        if (tweener != null && tweener.IsActive())
        {
            tweener.Kill();
        }
        float currentValue = 2f;
        tweener = DOTween.To(() => currentValue, x => currentValue = x, 0.2f, 1.5f).SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                vol.postExposure.value = currentValue;
            });
    }
}
