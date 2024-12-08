using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public bool is_aid = false;
    public bool is_player = false;


    Dictionary<int, string[]> talkData;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();

    }

    void GenerateData()
    {
        talkData.Add(1000, new string[] { " ���� ����", " ���������� ���", " ������ ��ũ�׿��� ������..", " �����", "��� ó���ұ�", 
        " �ϴ� ������ ��Ȳ�� �ͼ�������", " ����Ű�� a/s�� �̵��̰�", " 1������ 3���� ������ ���� ü����", " Space�ٸ� ������ ������ ������",
        " ���� �����ϰ� �ʹٸ� ��Ŭ��", " �׸��� ��Ŭ���� ������ ���� �̵�", " ���� ������ ���� ������", " ��� ó���ع����ڰ�" });
        talkData.Add(2000, new string[] { " ����鵵 ��ũ�׿°� �Ȱ���", " ���� ��ƹ����߰ھ�", " ������..", " ���� ��ȣ �ηµ��� ���� ���Ҿ�", " �� �ӵ��� ����" });
        talkData.Add(3000, new string[] { " �տ� �ִ� �� �༮�� ó���ϸ�", " �� �� ����" });

        // Start stage
        talkData.Add(1, new string[] { " ... ���� �����ΰ�", "�տ� �� 2��?", " ������ ó���ϰ� �Ѿ��" });
        talkData.Add(2, new string[] { " �� �ƴϸ� �Ʒ�?", " �� �������� �Ʒ��� ���� �� ����" });

        // normal stage1
        talkData.Add(11, new string[] { " .. ����.. ���濡 �������?", "-����. ����� ����", "-���濡 4���� ��ȣ�η�", "-�������� �ݹ� �����ž�", " Ȯ��" });
        talkData.Add(12, new string[] { "- ���� ����� ����", " ���� ������?", "-�׳�..", "-�� ���� ���� ���� ����", "-�������� �ʰ�", "-������", " �˾Ҵ�" });

        // normal stage2
        talkData.Add(21, new string[] { "-�̹� ������ ���� �ʾ�", "-���� ���� �������� ����", "-�������� �ʵ��� ������", " ����." });
        talkData.Add(22, new string[] { " ��, ���Ⱑ �ƴϱ�", " �ٽ� �ö󰡾߰ھ�" });
        talkData.Add(23, new string[] { "-�տ� �ִ� �༮��", "-�������̾�", " Ȯ���ߴ�" });

        // normal stage3
        talkData.Add(31, new string[] { "-���� ���� ������ ���ξ�.", "-���� 6��.", " Ȯ���ߴ�.", "- �� �׸��� ���� �̷ΰ���", "-���� ���� �ʵ���", "-������", " Ȯ��" });
        talkData.Add(32, new string[] { " ... �ϴ� ����������" });
        talkData.Add(33, new string[] { " �տ� �α�ô�� ������", " ������" });
        talkData.Add(34, new string[] { " ��, ���ٸ� ���ΰ�", " �ٽ� ����������.." });
        talkData.Add(35, new string[] { " ���� �տ��� �α�ô�� �������±�" });
        talkData.Add(36, new string[] { " ���������� �̷α�..", "-����. ����� ����", " ���� ������", "-�ⱸ�� ã�Ҿ�", "-�ٽ� ó������ �ö󰡼�", "-���������� �� ��", " ��.. Ȯ���̴�" });
        talkData.Add(37, new string[] { "-���濡 ������ �� ��.", " ���Ҿ�, ���� ���ΰ�", "-�ƴ� �����̾�", "��.." });

        // normal stage4
        talkData.Add(41, new string[] { " ��ü ���� ���δ�", " ��� �Ǹ����ž�", "-����, ���Ⱑ �������̾�", "-�� ���ٺ���", "-�ö󰡴� ���� �־�", "-�װ��� �ⱸ��", "����" });
        talkData.Add(42, new string[] { " ... ���⸦ �ö󰡶��?", " ��.. ��Ե� �غ��ڰ�" });
        talkData.Add(43, new string[] { " �ö󰡵� ���� ����..", "-������ ���� �� �ö�Ծ�", "-��ܿ� �� 3��", " �˾Ҵ�.. ��..", " �� �������� �ʰھ�" });

        // Boss Stage
        talkData.Add(51, new string[] { "-�����. �����߾�", "-��ũ�׿� ������ ��", " ��.. �ö������ ���鱺", " �� �������� �ʰھ�..", " �ݹ� ó���ϰ� ���ư���", " ���� ���δ�" });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex >= talkData[id].Length)
        {
            EndTalk();
            return null;
        }

        string talk = talkData[id][talkIndex];

        
        if (talk.StartsWith("-"))
        {
            is_aid = true;


        }
        else if(talk.StartsWith(" "))
        {
            is_player = true;
        }

        return talk;
    }

    public void EndTalk()
    {
        
        is_aid = false;
        is_player = false;

    }
}
