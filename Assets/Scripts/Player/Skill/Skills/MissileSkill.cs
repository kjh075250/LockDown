using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSkill : SkillBase
{
    public int Damage => damage;
    public int amount;
    
    public override void Init()
    {
        base.Init();
        isHaveCutScene = true;
    }

    public override void UseSkill()
    {
        base.UseSkill();
        SkillManager.Instance.UseMissile(amount, damage);
    }

    public void SetDamage(int value)
    {
        damage = value;
    }

    public void AddDamage()
    {
        damage += 50;
    }

    public void SetAmount(int value)
    {
        amount = value;
    }

    public void AddAmount()
    {
        amount += 25;
    }
}
