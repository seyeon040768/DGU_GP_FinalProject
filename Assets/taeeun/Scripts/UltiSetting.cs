using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UltiSetting : MonoBehaviour
{
    public GameObject ultiIcon;
    public Player player;
    public Text comboText;

    private Vector3 comboTextPosition;
    private float shakeComboTextCool;
    public float shakeComboTextDuration;
    public float shakeMagnitude;

    void Awake()
    {
        if (ultiIcon == null)
            ultiIcon = GameObject.Find("UltiIcon"); // UltiIcon 오브젝트를 찾아 설정
        if (player == null)
            player = GetComponent<Player>(); // Player 스크립트 참조

        comboTextPosition = comboText.transform.position;
    }
    void Update()
    {
        CheckUltiReady();

        shakeComboTextCool -= Time.deltaTime;
        if (shakeComboTextCool > 0.0f)
        {
            Vector3 newPos = comboTextPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0.0f) * shakeMagnitude;
            comboText.transform.position = newPos;
        }
        else
        {
            shakeComboTextCool = 0.0f;
        }

        if (player.combo <= 0)
        {
            comboText.gameObject.SetActive(false);
        }
    }
    public void UpdateComboUI()
    {
        if (comboText != null && player != null)
        {
            shakeComboTextCool = shakeComboTextDuration;

            comboText.text = player.combo + " hits"; // Combo 텍스트 업데이트

            comboText.gameObject.SetActive(true);
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
