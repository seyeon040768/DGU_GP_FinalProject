using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolDown : MonoBehaviour
{
    public Image disable; // 쿨타임 이미지를 표시하는 UI
    private float coolTime = 3f; // 초기 쿨타임 값
    private float cooltime_max = 3f; // 최대 쿨타임 값
    private bool isCooldownActive = false;

    // 쿨타임을 시작하는 메서드
    public void StartCooldown(float cooldownDuration)
    {
        if (!isCooldownActive)
        {
            cooltime_max = cooldownDuration;
            coolTime = cooldownDuration;
            StartCoroutine(CoolTimeCnt());
        }
    }

    // 쿨타임 코루틴
    private IEnumerator CoolTimeCnt()
    {
        isCooldownActive = true;

        while (coolTime > 0.0f)
        {
            coolTime -= Time.deltaTime;

            disable.fillAmount = coolTime / cooltime_max;
            yield return new WaitForFixedUpdate();

        }

        isCooldownActive = false;
    }
}
