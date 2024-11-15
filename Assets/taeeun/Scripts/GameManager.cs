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
        if (isNpc)
        {
            dialogText.text = dialogData;
        }
        else {
            dialogText.text = dialogData;
        }
        isAction = true;
        talkIndex++;
    }
}
