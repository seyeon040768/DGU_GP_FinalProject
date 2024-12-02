using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UltiSetting : MonoBehaviour
{
    public GameObject ultiIcon;
    public Player player;
    public Text comboText;
    void Awake()
    {
        if (ultiIcon == null)
            ultiIcon = GameObject.Find("UltiIcon"); // UltiIcon 오브젝트를 찾아 설정
        if (player == null)
            player = GetComponent<Player>(); // Player 스크립트 참조
        if (comboText == null)
            comboText = GetComponentInChildren<Text>(); // 자식 Text 컴포넌트 참조

    }
    void Update()
    {
        UpdateComboUI();
        CheckUltiReady();
    }
    public void UpdateComboUI()
    {
        if (comboText != null && player != null)
        {
            comboText.text = player.combo + " hits"; // Combo 텍스트 업데이트
        }
    }

    public void CheckUltiReady()
    {
        if (ultiIcon != null && player != null)
        {
            if (player.combo >= 20)
            {
                ultiIcon.SetActive(false); // Combo가 20 이상이면 궁극기 아이콘 활성화
            }
        }
    }
}
