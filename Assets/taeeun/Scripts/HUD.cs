using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { HP, Dash, Skill, Combo }; // �ٷ�� �� ������ ���������� ����
    public InfoType type;

    Text myText;
    Slider mySlider;
    public Slider myDashSlider; // Dash �����̴��� �����Ϳ��� �������� �Ҵ�
    public Character character;
    public Player player;

    void Awake() // �ʱ�ȭ
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();

        // Player�� null�� ��� Find�� �������� ã��
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        // myDashSlider�� �ݵ�� �����Ϳ��� ����ǵ��� ����
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.HP:
                float curHP = character.Hp;
                float maxHP = character.maxhp;
                if (mySlider != null) // null üũ
                {
                    mySlider.value = curHP / maxHP;
                }
                break;

            case InfoType.Dash:
                if (player != null && myDashSlider != null) // null üũ
                {
                    myDashSlider.value = player.Stamina;
                }
                break;

            case InfoType.Skill:
                // ��ų ���� UI ������Ʈ (���� ����)
                break;

            case InfoType.Combo:
                // �޺� ���� UI ������Ʈ (���� ����)
                break;
        }
    }

    void Start()
    {
        // �ʱⰪ ���� (�ʿ��� ���)
    }

    void Update()
    {
        // ������Ʈ ó�� (�ʿ��� ���)
    }
}
