using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManage : MonoBehaviour
{
    public GameObject exitObject; // 다음 씬으로 이동하는 출구 오브젝트
    private int totalEnemies; // 씬 내 적의 총 개수
    public int defeatedEnemies = 0; // 제거된 적의 수
    public Text GameOverText, RestartText;
    private float restartTimer = 3f; // 재시작 타이머 (3초)
    private bool isGameOver = false; // GameOver 상태 확인

    public SFXPool sfxPool;

    void Start()
    {
        // 출구 오브젝트 비활성화
        if (exitObject != null)
        {
            exitObject.SetActive(false);
        }

        // 적의 총 개수 초기화
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        totalEnemies = enemies.Length;
        Debug.Log("적 인원 :  " + totalEnemies);
    }

    // 적 제거 시 호출
    public void OnEnemyDefeated()
    {
        // 적이 하나 죽었을 때 defeatedEnemies 증가
        defeatedEnemies++;
        // 모든 적 제거 시 출구 활성화
        if (defeatedEnemies >= totalEnemies)
        {
            // 만약에 현재 씬 이름이 Boss라면
            if (SceneManager.GetActiveScene().name == "Boss")
            {
                GameClear();
            }
            if (exitObject != null)
            {
                exitObject.SetActive(true);
                Debug.Log("출구 활성화");
            }
        }
    }

    // Player가 사망 시 호출할 GameOver 메서드
    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true; // GameOver 상태 활성화
            Time.timeScale = 0;
            GameOverText.gameObject.SetActive(true);
            RestartText.gameObject.SetActive(true);

            StartCoroutine(RestartCountdown());
        }
    }

    // 재시작 카운트다운 코루틴
    private IEnumerator RestartCountdown()
    {
        float remainingTime = restartTimer;

        while (remainingTime > 0)
        {
            RestartText.text = $"The game will restart in {Mathf.CeilToInt(remainingTime)} seconds";
            yield return new WaitForSecondsRealtime(1f); // Time.timeScale이 0이어도 동작
            remainingTime--;
        }

        // 3초 후 현재 씬 재시작
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Boss 씬에서 호출할 메서드
    public void GameClear()
    {
        Time.timeScale = 0;
        GameOverText.text = "Game Clear!";
        sfxPool.Play("Clear");
        GameOverText.gameObject.SetActive(true);
        RestartText.gameObject.SetActive(true);
        StartCoroutine(GameClearCountdown());
    }

    private IEnumerator GameClearCountdown()
    {
        float remainingTime = 5f;

        while (remainingTime > 0)
        {
            RestartText.text = $"The game will return to the title screen\nin {Mathf.CeilToInt(remainingTime)} seconds.";

            yield return new WaitForSecondsRealtime(1f); // Time.timeScale이 0이어도 동작
            remainingTime--;
        }

        // 5초 후 타이틀 화면으로
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
