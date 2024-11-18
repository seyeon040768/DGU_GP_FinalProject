using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private float shakeTimer;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeScreen(float intensity = 2f, float duration = 1f)
    {
        var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null)
        {
            // Noise 추가하기
            noise = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        // 흔들림 설정
        noise.m_AmplitudeGain = intensity;
        noise.m_FrequencyGain = intensity;
        shakeTimer = duration;

        StartCoroutine(StopShakeAfterDuration(noise));
    }

    private IEnumerator StopShakeAfterDuration(CinemachineBasicMultiChannelPerlin noise)
    {
        while (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            yield return null;
        }

        // 흔들림 종료
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}