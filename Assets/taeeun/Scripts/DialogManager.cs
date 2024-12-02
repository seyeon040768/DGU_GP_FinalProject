using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(1000, new string[] { "���� ����"," ���������� ����̴�", "������ ��ũ�׿��� �������ΰ�", "�����", "��� ó���ұ�", 
        "�ϴ� ������ ��Ȳ�� �ͼ�������", "����Ű�� a/s�� �̵��̰�", "1������ 3���� ������ ���� ü����", "Space�ٸ� ������ ������ ������"
        , "���� �����ϰ� �ʹٸ� ��Ŭ��", "�׸��� ��Ŭ���� ������ ���� �̵�", "���� ������ ���� ������", "��� ó���ع����ڰ�" });
        talkData.Add(2000, new string[] { "����鵵 ��ũ�׿°� �Ȱ���", "���� ��ƹ����߰ھ�", "������..", "���� ��ȣ �ηµ��� ���� ���Ҿ�", "�� �ӵ��� ����"});
        talkData.Add(3000, new string[] { "�տ� �ִ� �� �༮�� ó���ϸ� �� �� ����" });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if(talkIndex == talkData[id].Length)
        {
            return null;
        }
        else
            return talkData[id][talkIndex]; // id�� ��ȭ �迭 ã�ư��� talkIndex�� �ѹ��徿
    }
}
