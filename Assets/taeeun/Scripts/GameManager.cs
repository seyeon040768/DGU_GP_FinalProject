using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        if(dialogData == null)
        {
            isAction = false;
            talkIndex = 0;
            return;
        }

        if(dialogCoroutine != null)
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


}
