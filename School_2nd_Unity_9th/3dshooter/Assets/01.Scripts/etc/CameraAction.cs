using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraAction : MonoBehaviour
{
    public static CameraAction Instance { get; private set; }

    private CinemachineVirtualCamera followCam;
    private CinemachineBasicMultiChannelPerlin bPerin;

    private bool isShake = false;
    private float currentTime = 0f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        followCam = GetComponent<CinemachineVirtualCamera>();
        bPerin = followCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    
    }

    public void ShakeCam(float intensity, float time)
    {
        // 코루틴 호출
        if(!isShake)
        {
            isShake = true;
            StartCoroutine(ShakeUpdate(intensity, time));
        }
        else
        {
            bPerin.m_AmplitudeGain = intensity;
        }

    }

    public IEnumerator ShakeUpdate(float intensity, float time) // 수행평가 나올거임.
    {
        bPerin.m_AmplitudeGain = intensity;
        currentTime = 0;

        while (true)
        {
            yield return null;
            currentTime += Time.deltaTime;
            if(currentTime >= time)
            {
                break;
            }
            bPerin.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, currentTime / time);
        }

        isShake = false;
        bPerin.m_AmplitudeGain = 0;
    }

}
