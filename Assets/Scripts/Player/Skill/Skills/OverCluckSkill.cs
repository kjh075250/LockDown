using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverCluckSkill : SkillBase
{
    public override void Init()
    {
        isHaveCutScene = true;   
    }

    public override void UseSkill()
    {
        base.UseSkill();
        SkillManager.Instance.OverCluck();
    }

}
