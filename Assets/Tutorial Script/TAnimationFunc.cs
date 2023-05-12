using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAnimationFunc : MonoBehaviour
{
    public TPlayerControl playerControl;
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Tofalse()
    {
        anim.SetBool("Shot", false);
    }
    void ShotMis()
    {
        playerControl.ShotMis();
    }
}
