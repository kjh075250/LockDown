using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "SO/Player")]
public class PlayerStatSO : ScriptableObject
{
    [SerializeField] int currentHp;
    [SerializeField] int maxHp;
    [SerializeField] int regenHP;

    [SerializeField] int currentGold;
    [SerializeField] int currentWave;

    [SerializeField] float defaultAttackDelay;
    [SerializeField] int defaultAttackDamage;

    [SerializeField] int laserAttack_LaserCount;
    [SerializeField] int laserAttack_ChainLaserLength;
    [SerializeField] int laserAttack_ChainLaserCount;
    [SerializeField] int laserAttackDamage;
    [SerializeField] float electricFieldCooltime;

    public bool IsPurchaseMissile;
    public bool IsPurchaseOverCluck;
    public bool IsPurchaseHyperLaser;

    [SerializeField] PlayerStatSO defaultData;

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public int RegenHP => regenHP;

    public int CurrentGold => currentGold;
    public int CurrentWave => currentWave;

    public float DefaultAttackDelay => defaultAttackDelay;
    public int DefaultAttackDamage => defaultAttackDamage;

    public int LaserAttack_LaserCount => laserAttack_LaserCount;
    public int LaserAttack_ChainLaserLength => laserAttack_ChainLaserLength;
    public int LaserAttack_ChainLaserCount => laserAttack_ChainLaserCount;
    public int LaserAttackDamage => laserAttackDamage;
    public float ElectricFieldCooltime => electricFieldCooltime;

    public void AddPlayerValue(int _hp, int _maxHp, int _gold)
    {
        currentHp += _hp;
        maxHp += _maxHp;
        currentGold += _gold;
    }

    public void SetHP(int _hp)
    {
        currentHp = _hp;
    }

    public void SetRegen(int _regenhp)
    {
        regenHP = _regenhp;
    }

    public void SetMaxHP(int _maxhp)
    {
        maxHp = _maxhp;
    }

    public void AddDefaultAttackValue(float _delay, int _damage)
    {
        defaultAttackDelay += _delay;
        defaultAttackDamage += _damage;
    }

    public void AddLaserAttackValue(int _laserCount, int _chainLength, int _chainLaserCount, int _damage)
    {
        laserAttack_LaserCount += _laserCount;
        laserAttack_ChainLaserLength += _chainLength;
        laserAttack_ChainLaserCount += _chainLaserCount;
        laserAttackDamage += _damage;
    }

    public void SetElectricFieldDelay()
    {
        electricFieldCooltime -= electricFieldCooltime * 0.1f;
    }

    public void SetWave(int value)
    {
        currentWave = value;
    }

    public void SetData(PlayerStatSO _playerStatSO)
    {
        currentHp = _playerStatSO.CurrentHp;
        maxHp = _playerStatSO.MaxHp;
        currentGold = _playerStatSO.CurrentGold;
        currentWave = _playerStatSO.CurrentWave;
        defaultAttackDelay = _playerStatSO.defaultAttackDelay;
        defaultAttackDamage = _playerStatSO.DefaultAttackDamage;
        laserAttack_LaserCount = _playerStatSO.LaserAttack_LaserCount;
        laserAttack_ChainLaserLength = _playerStatSO.LaserAttack_ChainLaserLength;
        laserAttack_ChainLaserCount = _playerStatSO.LaserAttack_ChainLaserCount;
        laserAttackDamage = _playerStatSO.LaserAttackDamage;
        electricFieldCooltime = _playerStatSO.ElectricFieldCooltime;
    }

    public void SetJsonData(SaveData saveData)
    {
        currentHp = saveData.currentHp;
        maxHp = saveData.maxHp;
        regenHP = saveData.regenHP;
        currentGold = saveData.currentGold;
        currentWave = saveData.currentWave;
        defaultAttackDelay = saveData.defaultAttackDelay;
        defaultAttackDamage = saveData.defaultAttackDamage;
        laserAttack_LaserCount = saveData.laserAttack_LaserCount;
        laserAttack_ChainLaserLength = saveData.laserAttack_ChainLaserLength;
        laserAttack_ChainLaserCount = saveData.laserAttack_ChainLaserCount;
        laserAttackDamage = saveData.laserAttackDamage;
        electricFieldCooltime = saveData.electricFieldCooltime;

        IsPurchaseMissile = saveData.IsPurchaseMissile;
        IsPurchaseOverCluck = saveData.IsPurchaseOverCluck;
        IsPurchaseHyperLaser = saveData.IsPurchaseHyperLaser;
    }
}
