using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPlatform : MonoBehaviour
{
    public SceneManage sceneManage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Character>().Hp = 0;
            // 2�� �ڿ� scenemanage�� gameover �޼��� ȣ��
            Invoke("GameOver", 1f);
        }
    }
    void GameOver()
    {
        sceneManage.GameOver();
    }
}
