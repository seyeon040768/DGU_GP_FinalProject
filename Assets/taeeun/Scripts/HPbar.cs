using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    public Character character; // 체력을 참조할 캐릭터
    private Slider hpSlider; // HP를 표시할 슬라이더

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
            hpSlider.value = curHP / maxHP; // 체력 비율 계산
        }
    }
}
