using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    public Character character; // ü���� ������ ĳ����
    private Slider hpSlider; // HP�� ǥ���� �����̴�

    void Awake()
    {
        hpSlider = GetComponent<Slider>();
    }

    void Update()
    {
        if (character != null && hpSlider != null)
        {
            float curHP = character.Hp;
            float maxHP = character.maxhp;
            hpSlider.value = curHP / maxHP; // ü�� ���� ���
        }
    }
}
