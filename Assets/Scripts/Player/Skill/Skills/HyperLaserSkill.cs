using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperLaserSkill : SkillBase
{
    public int Damage => damage;
    public override void Init()
    {
        base.Init();
        isHaveCutScene = true;
    }

    public override void UseSkill()
    {
        SkillManager.Instance.HyperLaser(damage);
    }

    public void SetDamage(int value)
    {
        damage = value;
    }

    public void AddDamage()
    {
        damage += 10;
    }
}
