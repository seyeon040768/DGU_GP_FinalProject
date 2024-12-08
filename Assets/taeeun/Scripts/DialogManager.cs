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
        talkData.Add(1000, new string[] { " 나는 제로", " 정보경찰의 요원", " 저쪽이 다크네온의 본거지..", " 어디보자", "어떻게 처리할까", 
        " 일단 지금의 상황에 익숙해지자", " 방향키나 a/s가 이동이고", " 1번부터 3번을 누르면 무기 체인지", " Space바를 누르면 점프도 가능해",
        " 적을 공격하고 싶다면 좌클릭", " 그리고 우클릭을 누르면 빠른 이동", " 좋아 이제야 몸이 가볍군", " 모두 처리해버리자고" });
        talkData.Add(2000, new string[] { " 너희들도 다크네온과 똑같아", " 전부 잡아버려야겠어", " 하지만..", " 아직 경호 인력들이 많이 남았어", " 더 속도를 내자" });
        talkData.Add(3000, new string[] { " 앞에 있는 저 녀석만 처리하면", " 될 것 같군" });

        // Start stage
        talkData.Add(1, new string[] { " ... 이제 시작인가", "앞에 약 2명?", " 빠르게 처리하고 넘어가지" });
        talkData.Add(2, new string[] { " 위 아니면 아래?", " 내 생각에는 아래에 있을 거 같군" });

        // normal stage1
        talkData.Add(11, new string[] { " .. 알파.. 전방에 몇명이지?", "-제로. 여기는 알파", "-전방에 4명의 경호인력", "-너정도면 금방 끝낼거야", " 확인" });
        talkData.Add(12, new string[] { "- 제로 여기는 알파", " 무슨 일이지?", "-그냥..", "-이 곳은 높은 곳이 많아", "-떨어지지 않게", "-조심해", " 알았다" });

        // normal stage2
        talkData.Add(21, new string[] { "-이번 지형은 좋지 않아", "-높고 낮은 지형들이 많아", "-떨어지지 않도록 조심해", " 고맙다." });
        talkData.Add(22, new string[] { " 쳇, 여기가 아니군", " 다시 올라가야겠어" });
        talkData.Add(23, new string[] { "-앞에 있는 녀석이", "-마지막이야", " 확인했다" });

        // normal stage3
        talkData.Add(31, new string[] { "-이제 적의 본거지 내부야.", "-적은 6명.", " 확인했다.", "- 아 그리고 여긴 미로같아", "-길을 잃지 않도록", "-조심해", " 확인" });
        talkData.Add(32, new string[] { " ... 일단 내려가보자" });
        talkData.Add(33, new string[] { " 앞에 인기척이 느껴져", " 가보자" });
        talkData.Add(34, new string[] { " 쳇, 막다른 길인가", " 다시 내려가보자.." });
        talkData.Add(35, new string[] { " 여기 앞에도 인기척이 느껴지는군" });
        talkData.Add(36, new string[] { " 지긋지긋한 미로군..", "-제로. 여기는 알파", " 무슨 일이지", "-출구를 찾았어", "-다시 처음으로 올라가서", "-오른쪽으로 쭉 가", " 후.. 확인이다" });
        talkData.Add(37, new string[] { "-전방에 마지막 한 명.", " 좋았어, 이제 끝인가", "-아니 아직이야", "쳇.." });

        // normal stage4
        talkData.Add(41, new string[] { " 대체 여기 내부는", " 어떻게 되먹은거야", "-제로, 여기가 마지막이야", "-쭉 가다보면", "-올라가는 길이 있어", "-그곳이 출구야", "고맙다" });
        talkData.Add(42, new string[] { " ... 여기를 올라가라고?", " 후.. 어떻게든 해보자고" });
        talkData.Add(43, new string[] { " 올라가도 끝이 없다..", "-걱정마 거의 다 올라왔어", "-상단에 적 3명", " 알았다.. 후..", " 다 가만두지 않겠어" });

        // Boss Stage
        talkData.Add(51, new string[] { "-여기야. 도착했어", "-다크네온 보스의 방", " 후.. 올라오느라 힘들군", " 다 가만두지 않겠어..", " 금방 처리하고 돌아가지", " 끝이 보인다" });
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
