using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public GameObject exitObject; // ���� ������ �̵��ϴ� �ⱸ ������Ʈ
    private int totalEnemies; // �� �� ���� �� ����
    private int defeatedEnemies = 0; // ���ŵ� ���� ��

    void Start()
    {
        // �ⱸ ������Ʈ ��Ȱ��ȭ
        if (exitObject != null)
        {
            exitObject.SetActive(false);
        }

        // ���� �� ���� �ʱ�ȭ
        totalEnemies = FindObjectsOfType<Enemy>().Length;
        Debug.Log("�� �ο� :  " + totalEnemies);
    }

    // �� ���� �� ȣ��
    public void OnEnemyDefeated()
    {
        defeatedEnemies++;

        // ��� �� ���� �� �ⱸ Ȱ��ȭ
        if (defeatedEnemies >= totalEnemies)
        {
            if (exitObject != null)
            {
                exitObject.SetActive(true);
                Debug.Log("�ⱸ Ȱ��ȭ");
            }
        }
    }
}
