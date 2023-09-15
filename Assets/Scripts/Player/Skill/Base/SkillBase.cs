using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    public float Cooltime = 1;
    protected int damage = 1;
    protected bool isHaveCutScene = false;
    public bool IsHaveCutScene => isHaveCutScene;

    public virtual void Init()
    {

    }

    public virtual void UseSkill()
    {

    }
}
