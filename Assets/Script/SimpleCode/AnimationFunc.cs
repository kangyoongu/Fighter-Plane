using UnityEngine;

public class AnimationFunc : MonoBehaviour
{
    public PlayerControl playerControl;
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
