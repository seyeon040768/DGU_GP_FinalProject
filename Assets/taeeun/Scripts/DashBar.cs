using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    public Player player; // ���¹̳��� ������ �÷��̾�
    private Slider dashSlider; // ���¹̳��� ǥ���� �����̴�

    void Awake()
    {
        dashSlider = GetComponent<Slider>();
    }

    void Update()
    {
        if (player != null && dashSlider != null)
        {
            dashSlider.value = player.Stamina; // ��� ���¹̳� ������Ʈ
        }
    }
}
