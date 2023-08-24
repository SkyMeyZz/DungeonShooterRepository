using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; set; }
    public Camera mainCamera;
    public CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("There is more of one instance of CameraManager");
        }
        instance = this;
    }
}
