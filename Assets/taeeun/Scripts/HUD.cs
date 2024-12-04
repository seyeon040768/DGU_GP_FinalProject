using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    public HPbar hpbar; // HPbar 스크립트 참조
    public DashBar dashBar; // DashBar 스크립트 참조
    public SkillCoolDown[] skillCooldown; // SkillCooldown 스크립트 참조
    public UltiSetting ultiSetting;
    public Text comboText;

    void Start()
    {
        if (hpbar == null)
            hpbar = GetComponentInChildren<HPbar>();
        if (dashBar == null)
            dashBar = GetComponentInChildren<DashBar>();
        if (skillCooldown == null)
            skillCooldown = GetComponentsInChildren<SkillCoolDown>();
        if (ultiSetting == null)
            ultiSetting = GetComponentInChildren<UltiSetting>();
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
    public void comboWrite()
    {
        ultiSetting.UpdateComboUI();
        ultiSetting.CheckUltiReady();
    }
    public void OnWeaponChanged(int weaponNum)
    {
        StartSkillCooldown(3f);
    }

}
