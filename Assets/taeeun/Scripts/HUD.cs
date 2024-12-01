using UnityEngine;

public class HUD : MonoBehaviour
{
    public HPbar hpbar; // HPbar 스크립트 참조
    public DashBar dashBar; // DashBar 스크립트 참조
    public SkillCoolDown[] skillCooldown; // SkillCooldown 스크립트 참조

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
