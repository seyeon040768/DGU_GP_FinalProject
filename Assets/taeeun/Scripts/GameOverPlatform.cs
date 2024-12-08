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
            // 2초 뒤에 scenemanage의 gameover 메서드 호출
            Invoke("GameOver", 1f);
        }
    }
    void GameOver()
    {
        sceneManage.GameOver();
    }
}
