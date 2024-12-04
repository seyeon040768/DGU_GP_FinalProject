using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private CinemachineBrain cinemachineBrain;

    // 배경의 고정된 Z 위치
    private float fixedZPosition;

    private void Start()
    {
        // 씬에 있는 CinemachineBrain 가져오기
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

        // 배경의 초기 Z 위치 저장
        fixedZPosition = transform.position.z;

        if (cinemachineBrain == null)
        {
            Debug.LogError("CinemachineBrain이 메인 카메라에 없습니다. Cinemachine을 확인하세요.");
        }
    }

    private void LateUpdate()
    {
        if (cinemachineBrain != null && cinemachineBrain.ActiveVirtualCamera != null)
        {
            // 현재 활성화된 가상 카메라의 위치를 가져옴
            Transform virtualCameraTransform = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform;

            // 가상 카메라의 위치에 맞춰 배경 위치를 업데이트
            Vector3 newPosition = new Vector3(virtualCameraTransform.position.x, virtualCameraTransform.position.y, fixedZPosition);
            transform.position = newPosition;
        }
    }

}
