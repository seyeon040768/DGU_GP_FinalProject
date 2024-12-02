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
            ultiIcon = GameObject.Find("UltiIcon"); // UltiIcon ������Ʈ�� ã�� ����
        if (player == null)
            player = GetComponent<Player>(); // Player ��ũ��Ʈ ����
        if (comboText == null)
            comboText = GetComponentInChildren<Text>(); // �ڽ� Text ������Ʈ ����

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
            comboText.text = player.combo + " hits"; // Combo �ؽ�Ʈ ������Ʈ
        }
    }

    public void CheckUltiReady()
    {
        if (ultiIcon != null && player != null)
        {
            if (player.combo >= 20)
            {
                ultiIcon.SetActive(false); // Combo�� 20 �̻��̸� �ñر� ������ Ȱ��ȭ
            }
        }
    }
}
