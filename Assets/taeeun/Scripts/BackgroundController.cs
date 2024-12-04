using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private CinemachineBrain cinemachineBrain;

    // ����� ������ Z ��ġ
    private float fixedZPosition;

    private void Start()
    {
        // ���� �ִ� CinemachineBrain ��������
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

        // ����� �ʱ� Z ��ġ ����
        fixedZPosition = transform.position.z;

        if (cinemachineBrain == null)
        {
            Debug.LogError("CinemachineBrain�� ���� ī�޶� �����ϴ�. Cinemachine�� Ȯ���ϼ���.");
        }
    }

    private void LateUpdate()
    {
        if (cinemachineBrain != null && cinemachineBrain.ActiveVirtualCamera != null)
        {
            // ���� Ȱ��ȭ�� ���� ī�޶��� ��ġ�� ������
            Transform virtualCameraTransform = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform;

            // ���� ī�޶��� ��ġ�� ���� ��� ��ġ�� ������Ʈ
            Vector3 newPosition = new Vector3(virtualCameraTransform.position.x, virtualCameraTransform.position.y, fixedZPosition);
            transform.position = newPosition;
        }
    }

}
