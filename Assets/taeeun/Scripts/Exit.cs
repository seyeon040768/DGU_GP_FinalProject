using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Exit : MonoBehaviour
{
    // �ش� ��ũ��Ʈ�� �Ҵ�� ������Ʈ�� �浹 �� ȣ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹�� ������Ʈ�� Player�� ���
        if (collision.CompareTag("Player"))
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            // nextScene���� �Ѿ
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
