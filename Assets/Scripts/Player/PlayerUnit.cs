using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : UnitBase
{
    private int needLaserCount = 5;

    [SerializeField] PlayerStatSO playerStat;

    [SerializeField] PlayerDefaultAttack defaultAttack;
    [SerializeField] PlayerLaserAttack laserAttack;
    [SerializeField] PlayerElectricField electricField;
    PlayerSkill playerSkill;

    [SerializeField] Transform orbEffect;
    public SkillButton buttons;

    int laserCount = 0;
    int laserLength = 0;
    int chainlaserCount = 0;

    int laserAttackDamage = 0;

    int currentlaserAttackDamage;
    int overClucklaserAttackDamage = 0;
    
    int regenHP = 25;
    float time_Regeneration = 0;

    int elecFieldDamage = 0;
    float time_elecField = 0;
    float elecFieldDelay = 4f;
    float defaultelecFieldDelay = 4f;
    float overCluckelecFieldDelay = 1f;

    private void Start()
    {
        GameManager.Instance.Orb.GetComponent<OrbImage>().SetSprite(0);
        playerSkill = GetComponent<PlayerSkill>();
        UIManager.Instance.UIEventStopListening(UIManager.UIEventFlags.LaserDamageUpgrade, SetLaserDamage);
        UIManager.Instance.UIEventStopListening(UIManager.UIEventFlags.ChainLaserCountUpgrade, SetChainLaserCount);
        UIManager.Instance.UIEventStopListening(UIManager.UIEventFlags.ChainLaserUpgrade, SetChainLaserLength);
        UIManager.Instance.UIEventStopListening(UIManager.UIEventFlags.ElectricFieldUpgrade, SetElectricFieldDelay);
        UIManager.Instance.UIEventStopListening(UIManager.UIEventFlags.PlayerMaxHPUpgrade, AddMaxHP);
        UIManager.Instance.UIEventStopListening(UIManager.UIEventFlags.PlayerRegenUpgrade, AddRegen);


        UIManager.Instance.UIEventAddListener(UIManager.UIEventFlags.LaserDamageUpgrade, SetLaserDamage);
        UIManager.Instance.UIEventAddListener(UIManager.UIEventFlags.ChainLaserCountUpgrade, SetChainLaserCount);
        UIManager.Instance.UIEventAddListener(UIManager.UIEventFlags.ChainLaserUpgrade, SetChainLaserLength);
        UIManager.Instance.UIEventAddListener(UIManager.UIEventFlags.ElectricFieldUpgrade, SetElectricFieldDelay);
        UIManager.Instance.UIEventAddListener(UIManager.UIEventFlags.PlayerMaxHPUpgrade, AddMaxHP);
        UIManager.Instance.UIEventAddListener(UIManager.UIEventFlags.PlayerRegenUpgrade, AddRegen);

        StatSetting();
    }

    void FixedUpdate()
    {
        CheckLaserCount();
    }

    private void Update()
    {
        time_Regeneration = Mathf.Clamp(time_Regeneration + Time.deltaTime, 0, 1);
        time_elecField = Mathf.Clamp(time_elecField + Time.deltaTime, 0, elecFieldDelay);

        if (time_Regeneration == 1)
        {
            Regeneration(regenHP);
            time_Regeneration = 0;
        }

        if(time_elecField == elecFieldDelay)
        {
            electricField.TriggerElectricField(elecFieldDamage);
            time_elecField = 0;
        }
    }

    public override void Damage(int value)
    {
        base.Damage(value);
        playerStat.SetHP(_hp);
        UIManager.Instance.SetHPUI();
    }

    public void Regeneration(int value)
    {
        Heal(value);
        playerStat.SetHP(_hp);
        UIManager.Instance.SetHPUI();
    }

    void StatSetting()
    {
        _hp = playerStat.CurrentHp;
        _maxHp = playerStat.MaxHp;
        base.Init(_hp, _maxHp);

        GameManager.Instance.SetMoney(playerStat.CurrentGold);
        GameManager.Instance.SetWave(playerStat.CurrentWave);
        UIManager.Instance.SetHPUI();

        defaultAttack.SetFirstDelay();
        defaultAttack.SetDamage(playerStat.DefaultAttackDamage);

        laserCount = playerStat.LaserAttack_LaserCount;
        laserLength = playerStat.LaserAttack_ChainLaserLength;
        chainlaserCount = playerStat.LaserAttack_ChainLaserCount;
        laserAttackDamage = playerStat.LaserAttackDamage;
        currentlaserAttackDamage = laserAttackDamage;

        UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.MissileAmountUpgrade].IsMissile = playerStat.IsPurchaseMissile;
        UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.MissileDamageUpgrade].IsMissile = playerStat.IsPurchaseMissile;
        UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.HyperLaserUpgrade].IsHyperLaser = playerStat.IsPurchaseHyperLaser;
        UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.OverCluckUpgrade].IsOverCluck = playerStat.IsPurchaseOverCluck;

        buttons.SetMissileButton(playerStat.IsPurchaseMissile);
        buttons.SetOverCluckButton(playerStat.IsPurchaseOverCluck);
        buttons.SetHyperButton(playerStat.IsPurchaseHyperLaser);

        playerSkill.isSkillNotPurchased[0] = playerStat.IsPurchaseMissile;
        playerSkill.isSkillNotPurchased[1] = playerStat.IsPurchaseOverCluck;
        playerSkill.isSkillNotPurchased[2] = playerStat.IsPurchaseHyperLaser;
        regenHP = playerStat.RegenHP;


        UIManager.Instance.SetUI();
    }

    private void CheckLaserCount()
    {
        if(defaultAttack.LaserCount > needLaserCount - 2)
            orbEffect.gameObject.SetActive(true);
        else 
            orbEffect.gameObject.SetActive(false);

        if (defaultAttack.LaserCount >= needLaserCount)
        {
            StartCoroutine(LaserEffectCoroutine());
        }
    }

    IEnumerator LaserEffectCoroutine()
    {
        defaultAttack.SetLaserCount(0);
        GameManager.Instance.Orb.GetComponent<OrbImage>().SetSprite(0);
        laserAttack.Attack(laserCount, laserLength, chainlaserCount, laserAttackDamage);

        EventManager.Instance.TriggerEvent(EventManager.EventFlags.LaserAttack);
        laserAttack.SetEffectActive(true);
        yield return new WaitForSeconds(0.2f);
        laserAttack.SetEffectActive(false);
    }

    public void SetWaveStat(int value)
    {
        playerStat.SetWave(value);
    }

    public void OverCluck(bool isTurnOn)
    {
        defaultAttack.OverCluck(isTurnOn);
        if(isTurnOn)
        {
            overClucklaserAttackDamage = laserAttackDamage * 2;
            laserAttackDamage = overClucklaserAttackDamage;

            elecFieldDelay = overCluckelecFieldDelay;
        }
        else
        {
            laserAttackDamage = currentlaserAttackDamage;
            elecFieldDelay = defaultelecFieldDelay;
        }
    }

    public void AddGold(int value)
    {
        playerStat.AddPlayerValue(0, 0, value);

    }

    void SetLaserDamage()
    {
        var value = UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.LaserDamageUpgrade];
        playerStat.AddLaserAttackValue(0, 0, 0, 20);
        currentlaserAttackDamage = playerStat.LaserAttackDamage;
        laserAttackDamage = currentlaserAttackDamage;
        value.ThisUISO.SetNowStatus(laserAttackDamage);
        value.ThisUISO.LevelUp();
        value.SetUI();
    }

    void SetChainLaserCount()
    {
        var value = UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.ChainLaserCountUpgrade];
        playerStat.AddLaserAttackValue(0, 0, 1, 0);
        chainlaserCount = playerStat.LaserAttack_ChainLaserCount;
        value.ThisUISO.SetNowStatus(chainlaserCount);
        value.ThisUISO.LevelUp();
        value.SetUI();
    }

    void SetChainLaserLength()
    {
        var value = UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.ChainLaserUpgrade];
        playerStat.AddLaserAttackValue(0, 1, 0, 0);
        laserLength = playerStat.LaserAttack_ChainLaserLength;
        value.ThisUISO.SetNowStatus(laserLength);
        value.ThisUISO.LevelUp();
        value.SetUI();
    }

    void SetElectricFieldDelay()
    {
        var value = UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.ElectricFieldUpgrade];
        playerStat.SetElectricFieldDelay();
        defaultelecFieldDelay = playerStat.ElectricFieldCooltime;
        elecFieldDelay = defaultelecFieldDelay;
        value.ThisUISO.SetNowStatus(elecFieldDelay);
        value.ThisUISO.LevelUp();
        value.SetUI();
    }

    public void SetIsMissilePurchased()
    {
        playerStat.IsPurchaseMissile = false;
        playerSkill.isSkillNotPurchased[0] = false;
        buttons.SetMissileButton(playerStat.IsPurchaseMissile);
    }

    public void SetIsHyperPurchased()
    {
        playerStat.IsPurchaseHyperLaser = false;
        playerSkill.isSkillNotPurchased[1] = false;
        buttons.SetHyperButton(playerStat.IsPurchaseHyperLaser);
    }

    public void SetIsOverCluckPurchased()
    {
        playerStat.IsPurchaseOverCluck = false;
        playerSkill.isSkillNotPurchased[2] = false;
        buttons.SetOverCluckButton(playerStat.IsPurchaseOverCluck);
    }

    void AddMaxHP()
    {
        _maxHp += 250;
        playerStat.SetMaxHP(_maxHp);
        UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.PlayerMaxHPUpgrade].ThisUISO.SetNowStatus(_maxHp);
        UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.PlayerMaxHPUpgrade].ThisUISO.LevelUp();
    }

    void AddRegen()
    {
        regenHP += 25;
        playerStat.SetRegen(regenHP);
        UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.PlayerRegenUpgrade].ThisUISO.SetNowStatus(regenHP);
        UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.PlayerRegenUpgrade].ThisUISO.LevelUp();
    }

    public void ResetHP()
    {
        _hp = _maxHp;
        playerStat.SetHP(_maxHp);
    }
}
