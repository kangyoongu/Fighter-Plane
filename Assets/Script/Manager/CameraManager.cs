using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    public CinemachineVirtualCamera mainView;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void MakePlayingView()
    {
        mainView.Priority = 9;
    }
}
