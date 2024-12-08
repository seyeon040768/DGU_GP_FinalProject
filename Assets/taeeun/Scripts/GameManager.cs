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


    public Button retryButton; // Retry 버튼
    public Button titleButton; // Title 버튼

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
        if (dialogManager.is_aid == true) // 조력자 npc의 대사면
        {
            dialogText.color = new Color(0, 0, 1); // 파란색
        }
        if (dialogManager.is_player == true) // 플레이어의 대사면
        {
            dialogText.color = new Color(0, 0, 0); // 검은색
        }
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


}
