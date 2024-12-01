using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    public Player player; // 스태미나를 참조할 플레이어
    private Slider dashSlider; // 스태미나를 표시할 슬라이더

    void Awake()
    {
        dashSlider = GetComponent<Slider>();
    }

    void Update()
    {
        if (player != null && dashSlider != null)
        {
            dashSlider.value = player.Stamina; // 대시 스태미나 업데이트
        }
    }
}
