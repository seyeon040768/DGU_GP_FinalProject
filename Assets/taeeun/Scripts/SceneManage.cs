using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManage : MonoBehaviour
{
    public GameObject exitObject; // ���� ������ �̵��ϴ� �ⱸ ������Ʈ
    private int totalEnemies; // �� �� ���� �� ����
    public int defeatedEnemies = 0; // ���ŵ� ���� ��
    public Text GameOverText, RestartText;
    private float restartTimer = 3f; // ����� Ÿ�̸� (3��)
    private bool isGameOver = false; // GameOver ���� Ȯ��

    public SFXPool sfxPool;

    void Start()
    {
        // �ⱸ ������Ʈ ��Ȱ��ȭ
        if (exitObject != null)
        {
            exitObject.SetActive(false);
        }

        // ���� �� ���� �ʱ�ȭ
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        totalEnemies = enemies.Length;
        Debug.Log("�� �ο� :  " + totalEnemies);
    }

    // �� ���� �� ȣ��
    public void OnEnemyDefeated()
    {
        // ���� �ϳ� �׾��� �� defeatedEnemies ����
        defeatedEnemies++;
        // ��� �� ���� �� �ⱸ Ȱ��ȭ
        if (defeatedEnemies >= totalEnemies)
        {
            // ���࿡ ���� �� �̸��� Boss���
            if (SceneManager.GetActiveScene().name == "Boss")
            {
                GameClear();
            }
            if (exitObject != null)
            {
                exitObject.SetActive(true);
                Debug.Log("�ⱸ Ȱ��ȭ");
            }
        }
    }

    // Player�� ��� �� ȣ���� GameOver �޼���
    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true; // GameOver ���� Ȱ��ȭ
            Time.timeScale = 0;
            GameOverText.gameObject.SetActive(true);
            RestartText.gameObject.SetActive(true);

            StartCoroutine(RestartCountdown());
        }
    }

    // ����� ī��Ʈ�ٿ� �ڷ�ƾ
    private IEnumerator RestartCountdown()
    {
        float remainingTime = restartTimer;

        while (remainingTime > 0)
        {
            RestartText.text = $"The game will restart in {Mathf.CeilToInt(remainingTime)} seconds";
            yield return new WaitForSecondsRealtime(1f); // Time.timeScale�� 0�̾ ����
            remainingTime--;
        }

        // 3�� �� ���� �� �����
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Boss ������ ȣ���� �޼���
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

            yield return new WaitForSecondsRealtime(1f); // Time.timeScale�� 0�̾ ����
            remainingTime--;
        }

        // 5�� �� Ÿ��Ʋ ȭ������
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
