using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Exit : MonoBehaviour
{
    // 해당 스크립트가 할당된 오브젝트와 충돌 시 호출
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 오브젝트가 Player인 경우
        if (collision.CompareTag("Player"))
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            // nextScene으로 넘어감
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
