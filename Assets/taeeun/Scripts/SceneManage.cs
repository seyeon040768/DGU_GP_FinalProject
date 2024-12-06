using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public GameObject exitObject; // 다음 씬으로 이동하는 출구 오브젝트
    private int totalEnemies; // 씬 내 적의 총 개수
    private int defeatedEnemies = 0; // 제거된 적의 수

    void Start()
    {
        // 출구 오브젝트 비활성화
        if (exitObject != null)
        {
            exitObject.SetActive(false);
        }

        // 적의 총 개수 초기화
        totalEnemies = FindObjectsOfType<Enemy>().Length;
        Debug.Log("적 인원 :  " + totalEnemies);
    }

    // 적 제거 시 호출
    public void OnEnemyDefeated()
    {
        defeatedEnemies++;

        // 모든 적 제거 시 출구 활성화
        if (defeatedEnemies >= totalEnemies)
        {
            if (exitObject != null)
            {
                exitObject.SetActive(true);
                Debug.Log("출구 활성화");
            }
        }
    }
}
