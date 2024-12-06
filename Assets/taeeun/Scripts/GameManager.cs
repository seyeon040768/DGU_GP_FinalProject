using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public DialogManager dialogManager;
    public GameObject dialogPanel;
    public Text dialogText;
    public GameObject scanObject;
    public bool isAction = false;
    public int talkIndex;
    private Coroutine dialogCoroutine;

    public AudioSource audioSource;
    public AudioClip typingSound;

    public GameObject gameOverUI; // Game Over UI
    public GameObject gameClearUI; // Game Clear UI
    public Text endMessage; // Game Over/Game Clear 메시지 텍스트
    public Button retryButton; // Retry 버튼
    public Button titleButton; // Title 버튼
    private bool isGameOver = false; // 중복 호출 방지

    void Start()
    {

        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (gameClearUI != null) gameClearUI.SetActive(false);
    }

    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjectData objectData = scanObject.GetComponent<ObjectData>();
        Talk(objectData.id, objectData.isNpc);
        dialogPanel.SetActive(isAction);
    }

    void Talk(int id, bool isNpc)
    {
        string dialogData = dialogManager.GetTalk(id, talkIndex);

        if (dialogData == null)
        {
            isAction = false;
            talkIndex = 0;
            return;
        }

        if (dialogCoroutine != null)
        {
            StopCoroutine(dialogCoroutine);
        }
        dialogCoroutine = StartCoroutine(TypeDialog(dialogData));
        isAction = true;
        talkIndex++;
    }

    IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = ""; // 초기화
        foreach (char letter in dialog.ToCharArray())
        {
            dialogText.text += letter; // 한 글자씩 추가
            if (typingSound != null && audioSource != null) // 효과음 재생
            {
                audioSource.PlayOneShot(typingSound);
            }
            yield return new WaitForSeconds(0.1f); // 0.1초에 한글자씩
        }
        isAction = false; // 대사 출력 완료
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            endMessage.text = "Game Over!"; // 메시지 설정
            retryButton.gameObject.SetActive(true); // Retry 버튼 활성화
            titleButton.gameObject.SetActive(false); // Title 버튼 비활성화
        }
    }

    public void GameClear()
    {
        if (isGameOver) return;

        isGameOver = true;
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
            endMessage.text = "Game Clear! 축하드립니다."; // 메시지 설정
            retryButton.gameObject.SetActive(false); // Retry 버튼 비활성화
            titleButton.gameObject.SetActive(true); // Title 버튼 활성화
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 재시작
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title"); // 타이틀 씬으로 이동 (타이틀 씬 이름 설정 필요)
    }

}
