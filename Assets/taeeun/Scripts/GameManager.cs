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


    public Button retryButton; // Retry ��ư
    public Button titleButton; // Title ��ư

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
        if (dialogManager.is_aid == true) // ������ npc�� ����
        {
            dialogText.color = new Color(0, 0, 1); // �Ķ���
        }
        if (dialogManager.is_player == true) // �÷��̾��� ����
        {
            dialogText.color = new Color(0, 0, 0); // ������
        }
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


}
