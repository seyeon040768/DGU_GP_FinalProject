using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    public enum InfoType {HP, Dash, Skill, Combo}; // �ٷ�� �� ������ ���������� ����
    public InfoType type;

    Text myText;
    Slider mySlider;
    
    void Awake() // �ʱ�ȭ
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.HP:

                break;
            case InfoType.Dash:

                break;
            case InfoType.Skill:

                break;
            case InfoType.Combo:

                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
