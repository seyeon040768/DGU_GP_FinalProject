using UnityEngine;

public class HUD : MonoBehaviour
{
    public HPbar hpbar; // HPbar ��ũ��Ʈ ����
    public DashBar dashBar; // DashBar ��ũ��Ʈ ����
    public SkillCoolDown[] skillCooldown; // SkillCooldown ��ũ��Ʈ ����

    void Start()
    {
        if (hpbar == null)
            hpbar = GetComponentInChildren<HPbar>();
        if (dashBar == null)
            dashBar = GetComponentInChildren<DashBar>();
        if (skillCooldown == null)
            skillCooldown = GetComponentsInChildren<SkillCoolDown>();
    }

    public void StartSkillCooldown(float duration)
    {
        foreach (SkillCoolDown cooldown in skillCooldown)
        {
            if (cooldown != null)
            {
                cooldown.StartCooldown(duration);
            }
        }
    }
    public void OnWeaponChanged(int weaponNum)
    {
        StartSkillCooldown(3f);
    }

}
