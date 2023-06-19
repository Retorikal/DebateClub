using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class HandleCameraShake : MonoBehaviour
{
    [SerializeField] float[] _eachMachIntensity = new float[3]; //there are 3 mach levels 

    CinemachineVirtualCamera virtualCamera;
    float shakeTimer;
    float shakeIntensity;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        shakeIntensity = _eachMachIntensity[0];
    }

    public void SetIntensityOnMach(int machLevel)
    {
        Debug.Log(machLevel);
        //if (machLevel == 0) shakeIntensity = _eachMachIntensity[0];//default
        if (machLevel == 1) shakeIntensity = _eachMachIntensity[1];
        if (machLevel == 2) shakeIntensity = _eachMachIntensity[2];
    }
    
    public void CameraShake(float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel =
            virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        cinemachineBasicMultiChannel.m_AmplitudeGain = shakeIntensity;
        shakeTimer = time;
    }

    void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannel =
                    virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannel.m_AmplitudeGain = 0;
            }
        }
    }
}
