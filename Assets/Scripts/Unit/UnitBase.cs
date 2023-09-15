using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    protected int _hp;
    public int Hp => _hp;
    protected int _maxHp;
    public int MaxHp => _maxHp;

    protected void Init(int hp, int maxHp)
    {
        _hp = hp;
        _maxHp = maxHp;
    }

    public void Heal(int value)
    {
        _hp = Mathf.Clamp(Hp + value, 0, MaxHp);
    }

    public virtual void Damage(int value)
    {
        _hp -= value;
        if (Hp <= 0)
            Die();
    }

    public virtual void Die()
    {

    }

}
