using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { HP, Dash, Skill, Combo }; // 다루게 될 데이터 열거형으로 선언
    public InfoType type;

    Text myText;
    Slider mySlider;
    public Slider myDashSlider; // Dash 슬라이더를 에디터에서 수동으로 할당
    public Character character;
    public Player player;

    void Awake() // 초기화
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();

        // Player가 null일 경우 Find로 동적으로 찾기
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        // myDashSlider는 반드시 에디터에서 연결되도록 설정
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.HP:
                float curHP = character.Hp;
                float maxHP = character.maxhp;
                if (mySlider != null) // null 체크
                {
                    mySlider.value = curHP / maxHP;
                }
                break;

            case InfoType.Dash:
                if (player != null && myDashSlider != null) // null 체크
                {
                    myDashSlider.value = player.Stamina;
                }
                break;

            case InfoType.Skill:
                // 스킬 관련 UI 업데이트 (추후 구현)
                break;

            case InfoType.Combo:
                // 콤보 관련 UI 업데이트 (추후 구현)
                break;
        }
    }

    void Start()
    {
        // 초기값 설정 (필요한 경우)
    }

    void Update()
    {
        // 업데이트 처리 (필요한 경우)
    }
}
