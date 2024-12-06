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
    public Text endMessage; // Game Over/Game Clear �޽��� �ؽ�Ʈ
    public Button retryButton; // Retry ��ư
    public Button titleButton; // Title ��ư
    private bool isGameOver = false; // �ߺ� ȣ�� ����

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
        dialogText.text = ""; // �ʱ�ȭ
        foreach (char letter in dialog.ToCharArray())
        {
            dialogText.text += letter; // �� ���ھ� �߰�
            if (typingSound != null && audioSource != null) // ȿ���� ���
            {
                audioSource.PlayOneShot(typingSound);
            }
            yield return new WaitForSeconds(0.1f); // 0.1�ʿ� �ѱ��ھ�
        }
        isAction = false; // ��� ��� �Ϸ�
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            endMessage.text = "Game Over!"; // �޽��� ����
            retryButton.gameObject.SetActive(true); // Retry ��ư Ȱ��ȭ
            titleButton.gameObject.SetActive(false); // Title ��ư ��Ȱ��ȭ
        }
    }

    public void GameClear()
    {
        if (isGameOver) return;

        isGameOver = true;
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
            endMessage.text = "Game Clear! ���ϵ帳�ϴ�."; // �޽��� ����
            retryButton.gameObject.SetActive(false); // Retry ��ư ��Ȱ��ȭ
            titleButton.gameObject.SetActive(true); // Title ��ư Ȱ��ȭ
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���� �� �����
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title"); // Ÿ��Ʋ ������ �̵� (Ÿ��Ʋ �� �̸� ���� �ʿ�)
    }

}
