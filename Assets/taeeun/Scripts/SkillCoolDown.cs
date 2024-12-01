using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolDown : MonoBehaviour
{
    public Image disable; // ��Ÿ�� �̹����� ǥ���ϴ� UI
    private float coolTime = 3f; // �ʱ� ��Ÿ�� ��
    private float cooltime_max = 3f; // �ִ� ��Ÿ�� ��
    private bool isCooldownActive = false;

    // ��Ÿ���� �����ϴ� �޼���
    public void StartCooldown(float cooldownDuration)
    {
        if (!isCooldownActive)
        {
            cooltime_max = cooldownDuration;
            coolTime = cooldownDuration;
            StartCoroutine(CoolTimeCnt());
        }
    }

    // ��Ÿ�� �ڷ�ƾ
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
