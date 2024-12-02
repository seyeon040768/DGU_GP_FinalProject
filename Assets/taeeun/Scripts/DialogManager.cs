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
        talkData.Add(1000, new string[] { "나는 제로"," 정보경찰의 요원이다", "저쪽이 다크네온의 본거지인가", "어디보자", "어떻게 처리할까", 
        "일단 지금의 상황에 익숙해지자", "방향키나 a/s가 이동이고", "1번부터 3번을 누르면 무기 체인지", "Space바를 누르면 점프도 가능해"
        , "적을 공격하고 싶다면 좌클릭", "그리고 우클릭을 누르면 빠른 이동", "좋아 이제야 몸이 가볍군", "모두 처리해버리자고" });
        talkData.Add(2000, new string[] { "너희들도 다크네온과 똑같아", "전부 잡아버려야겠어", "하지만..", "아직 경호 인력들이 많이 남았어", "더 속도를 내자"});
        talkData.Add(3000, new string[] { "앞에 있는 저 녀석만 처리하면 될 것 같군" });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if(talkIndex == talkData[id].Length)
        {
            return null;
        }
        else
            return talkData[id][talkIndex]; // id로 대화 배열 찾아가고 talkIndex로 한문장씩
    }
}
