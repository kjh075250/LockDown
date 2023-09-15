using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : UnitBase
{
    public EnemyLaserAttack laserAttack;

    private EnemyHit hit;
    private EnemyMove move;
    private EnemyHPBar[] hpBars;

    private int _thishp = 30;
    private int _thisMaxhp = 30;
    public int _thisindex = -1;
    public int _thisCoinValue = 0;

    public bool IsDead = false;
    public bool IsStop = false;
    public bool IsGodMod = false;

    float reviveTimer = 0;
    float deathTimer = 0;

    [SerializeField] private Transform Deatheffect;

    private void Awake()
    {
        base.Init(_thishp, _thisMaxhp);
        laserAttack = GetComponent<EnemyLaserAttack>();
        hit = GetComponentInChildren<EnemyHit>();
        move = GetComponent<EnemyMove>();
        hpBars = GetComponentsInChildren<EnemyHPBar>();
        IsDead = false;

        _thisindex = -1;
        reviveTimer = 0;
        Heal(MaxHp);
    }

    private void Update()
    {
        reviveTimer = Mathf.Clamp(reviveTimer + Time.deltaTime, 0, 0.5f);
        if(reviveTimer < 0.5f)
        {
            IsGodMod = true;
        }
        else
        {
            IsGodMod = false;
        }

        if (IsDead)
        {
            deathTimer = Mathf.Clamp(deathTimer + Time.deltaTime, 0, 0.5f);
            if(deathTimer == 0.5f)
            {
                PoolManager.Instance.Push(this.GetComponent<Poolable>());
            }
        }
    }

    public void EnemyInit()
    {
        base.Init(_thishp, _thisMaxhp);

        IsDead = false;
        laserAttack.laserInit();
        hit.hitInit();
        Deatheffect.gameObject.SetActive(false);
        reviveTimer = 0;
        deathTimer = 0;
        move.SetMoveStop(false);
        
        for (int i = 0; i < hpBars.Length; i++)
        {
            hpBars[i].HpbarInit();
        }
    }

    public void EnemyUpgrade(int attackDamage, float moveSpeed, int hp, Sprite sprite, int coinValue)
    {
        move.SetDamage(attackDamage);
        move.SetMoveSpeed(moveSpeed);
        _thisMaxhp = hp;
        _thishp = hp;
        base.Init(_thishp, _thisMaxhp);
        hit.SetSprite(sprite);
        _thisCoinValue = coinValue;
    }

    public override void Damage(int value)
    {
        if (IsDead) return;
        if (IsGodMod) return;
        base.Damage(value);
        StartCoroutine(StopCoroutine());
        hit.ColorChange();

        for (int i = 0; i < hpBars.Length; i++)
        {
            hpBars[i].Hpbar();
        }
    }

    public override void Die()
    {
        base.Die();
        Dead();
    }

    void Dead()
    {
        IsDead = true;
        EnemyManager.Instance.dead(this);

        GameManager.Instance.Player.Regeneration(move.GetDamage() / 2);
        UIManager.Instance.SetHPUI();
        
        move.SetMoveStop(true);
        Deatheffect.gameObject.SetActive(true);
        Poolable coin = PoolManager.Instance.Pop(EnemyManager.Instance.Coin);
        coin.GetComponent<CoinObject>().SetValue(_thisCoinValue);
        coin.gameObject.transform.position = this.transform.position;
    }

    IEnumerator StopCoroutine()
    {
        move.SetMoveStop(true);
        yield return new WaitForSeconds(0.25f);
        move.SetMoveStop(false);
    }
}
