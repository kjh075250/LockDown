using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillObject : MonoBehaviour
{
    [SerializeField] SkillSO skillSO;
    [SerializeField] List<UISO> skillUISO;
    [SerializeField] Image skillCoolTimeImage;

    public  SkillSO SkillSO => skillSO;
    private SkillBase thisSkill;
    
    void Start()
    {
        SkillSetting();
        UIManager.Instance.UIEventStopListening(UIManager.UIEventFlags.HyperLaserUpgrade, SetHyperLaserDamage);
        UIManager.Instance.UIEventStopListening(UIManager.UIEventFlags.MissileDamageUpgrade, SetMissileDamage);
        UIManager.Instance.UIEventStopListening(UIManager.UIEventFlags.MissileAmountUpgrade, SetMissileAmount);
        UIManager.Instance.UIEventStopListening(UIManager.UIEventFlags.OverCluckUpgrade, SetOverCluckTime);

        UIManager.Instance.UIEventAddListener(UIManager.UIEventFlags.HyperLaserUpgrade, SetHyperLaserDamage);
        UIManager.Instance.UIEventAddListener(UIManager.UIEventFlags.MissileDamageUpgrade, SetMissileDamage);
        UIManager.Instance.UIEventAddListener(UIManager.UIEventFlags.MissileAmountUpgrade, SetMissileAmount);
        UIManager.Instance.UIEventAddListener(UIManager.UIEventFlags.OverCluckUpgrade, SetOverCluckTime);
    }

    private void Update()
    {
        if (skillSO == null || thisSkill == null) return;
        thisSkill.Cooltime = Mathf.Clamp(thisSkill.Cooltime + Time.deltaTime, 0, skillSO.CoolTime);

        skillCoolTimeImage.fillAmount = thisSkill.Cooltime / skillSO.CoolTime;
        if (thisSkill.Cooltime == skillSO.CoolTime)
            skillCoolTimeImage.fillAmount = 0;
    }

    public void SkillSetting()
    {
        if (skillSO != null) thisSkill = skillSO.Skill;
        thisSkill.Init();
        thisSkill.Cooltime = skillSO.CoolTime;

        var skill = thisSkill.GetType();
        switch (skill.Name)
        {
            case "HyperLaserSkill":

                var hyperskill = thisSkill as HyperLaserSkill;

                hyperskill.SetDamage((int)skillUISO[0].Value);
                skillUISO[0].SetNowStatus(hyperskill.Damage);

                break;
            case "MissileSkill":

                var missileskill = thisSkill as MissileSkill;

                missileskill.SetDamage((int)skillUISO[1].Value);
                skillUISO[1].SetNowStatus(missileskill.Damage);

                missileskill.SetAmount((int)skillUISO[2].Value);
                skillUISO[2].SetNowStatus(missileskill.amount);

                break;
            case "OverCluckSkill":

                SkillManager.Instance.SetOverCluckEffectTime(skillUISO[3].Value);
                skillUISO[3].SetNowStatus(SkillManager.Instance.GetOverCluckTime());
                break;
        }

    }

    public void UseSkill()
    {
        if(UIManager.Instance.SkillImageEffect.IsUsingSkillEffect || thisSkill.Cooltime < SkillSO.CoolTime)
        {
            Debug.Log("Can not Use Skill yet");
            return;
        }

        if (thisSkill.IsHaveCutScene)
        {
            UIManager.Instance.SkillImageEffect.Init(skillSO.SkillImage, skillSO.SkillName);
        }
        thisSkill.UseSkill();
        thisSkill.Cooltime = 0;
    }

    void SetHyperLaserDamage()
    {
        var nowskill = thisSkill as HyperLaserSkill;
        if(nowskill)
        {
            nowskill.AddDamage();
            skillUISO[0].SetValue((float)nowskill.Damage);
            skillUISO[0].SetNowStatus((float)nowskill.Damage);
            skillUISO[0].LevelUp();
        }
    }

    void SetMissileDamage()
    {
        var nowskill = thisSkill as MissileSkill;
        if (nowskill)
        {
            nowskill.AddDamage();
            skillUISO[1].SetValue((float)nowskill.Damage);
            skillUISO[1].SetNowStatus((float)nowskill.Damage);
            skillUISO[1].LevelUp();
        }
    }

    void SetMissileAmount()
    {
        var nowskill = thisSkill as MissileSkill;
        if (nowskill)
        {
            nowskill.AddAmount();
            skillUISO[2].SetValue((float)nowskill.amount);
            skillUISO[2].SetNowStatus((float)nowskill.amount);
            skillUISO[2].LevelUp();
        }
    }


    void SetOverCluckTime()
    {
        var nowskill = thisSkill as OverCluckSkill;
        if (nowskill)
        {
            SkillManager.Instance.AddOverCluckEffectTime();
            skillUISO[3].SetValue(SkillManager.Instance.GetOverCluckTime());
            skillUISO[3].SetNowStatus(SkillManager.Instance.GetOverCluckTime());
            skillUISO[3].LevelUp();
        }
    }
}
