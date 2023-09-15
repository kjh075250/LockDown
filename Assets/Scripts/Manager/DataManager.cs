using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int currentHp;
    public int maxHp;
    public int regenHP;
    public int currentGold;
    public int currentWave;
    public float defaultAttackDelay;
    public int defaultAttackDamage;
    public int laserAttack_LaserCount;
    public int laserAttack_ChainLaserLength;
    public int laserAttack_ChainLaserCount;
    public int laserAttackDamage;
    public float electricFieldCooltime;

    public bool IsPurchaseMissile;
    public bool IsPurchaseOverCluck;
    public bool IsPurchaseHyperLaser;

    public List<string> _name = new List<string>();
    public List<string> summary = new List<string>();
    public List<string> nowStatusText = new List<string>();
    public List<float> nowStatus = new List<float>();
    public List<float> value = new List<float>();
    public List<Sprite> icon = new List<Sprite>();
    public List<int> upgradeCost = new List<int>();
    public List<float> costIncreaseRatio = new List<float>();
    public List<int> currentLevel = new List<int>();
    public List<int> maxLevel = new List<int>();
    public List<UIManager.UIEventFlags> thisUIEventFlag = new List<UIManager.UIEventFlags>();

}

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = GetComponent<DataManager>();
    }

    string path;

    void Start()
    {
        path = Application.persistentDataPath + "/Data.json";
        JsonLoad();
        JsonSave();
    }

    public void JsonLoad()
    {
        SaveData saveData = new SaveData();

        if (!File.Exists(path))
        {
            JsonSave();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                GameManager.Instance.Player.PlayerStat.SetJsonData(saveData);
                UIManager.Instance.SetUIJsonData(saveData);
            }
        }
    }

    public void JsonSave()
    {
        SaveData saveData = new SaveData();
        saveData.currentHp = GameManager.Instance.Player.PlayerStat.CurrentHp;
        saveData.maxHp = GameManager.Instance.Player.PlayerStat.MaxHp;
        saveData.regenHP = GameManager.Instance.Player.PlayerStat.RegenHP;
        saveData.currentGold = GameManager.Instance.Player.PlayerStat.CurrentGold;
        saveData.currentWave = GameManager.Instance.Player.PlayerStat.CurrentWave;
        saveData.defaultAttackDelay = GameManager.Instance.Player.PlayerStat.DefaultAttackDelay;
        saveData.defaultAttackDamage = GameManager.Instance.Player.PlayerStat.DefaultAttackDamage;
        saveData.laserAttack_LaserCount = GameManager.Instance.Player.PlayerStat.LaserAttack_LaserCount;
        saveData.laserAttack_ChainLaserLength = GameManager.Instance.Player.PlayerStat.LaserAttack_ChainLaserLength;
        saveData.laserAttack_ChainLaserCount = GameManager.Instance.Player.PlayerStat.LaserAttack_ChainLaserCount;
        saveData.laserAttackDamage = GameManager.Instance.Player.PlayerStat.LaserAttackDamage;
        saveData.electricFieldCooltime = GameManager.Instance.Player.PlayerStat.ElectricFieldCooltime;
        
        saveData.IsPurchaseMissile = GameManager.Instance.Player.PlayerStat.IsPurchaseMissile;
        saveData.IsPurchaseOverCluck = GameManager.Instance.Player.PlayerStat.IsPurchaseOverCluck;
        saveData.IsPurchaseHyperLaser = GameManager.Instance.Player.PlayerStat.IsPurchaseHyperLaser;

        for(int i = 0; i < UIManager.Instance.Ui_Upgrades.Count; i++)
        {
            saveData._name.Add(UIManager.Instance.Ui_Upgrades[i].ThisUISO.SkillName);
            saveData.summary.Add( UIManager.Instance.Ui_Upgrades[i].ThisUISO.Summary);
            saveData.nowStatusText.Add(UIManager.Instance.Ui_Upgrades[i].ThisUISO.NowStatusText);
            saveData.nowStatus.Add(UIManager.Instance.Ui_Upgrades[i].ThisUISO.NowStatus);
            saveData.value.Add( UIManager.Instance.Ui_Upgrades[i].ThisUISO.Value);
            saveData.icon.Add(UIManager.Instance.Ui_Upgrades[i].ThisUISO.Icon);
            saveData.upgradeCost.Add( UIManager.Instance.Ui_Upgrades[i].ThisUISO.UpgradeCost);
            saveData.costIncreaseRatio.Add( UIManager.Instance.Ui_Upgrades[i].ThisUISO.CostIncreaseRatio);
            saveData.currentLevel.Add(UIManager.Instance.Ui_Upgrades[i].ThisUISO.CurrentLevel);
            saveData.maxLevel.Add(UIManager.Instance.Ui_Upgrades[i].ThisUISO.MaxLevel);
            saveData.thisUIEventFlag.Add( UIManager.Instance.Ui_Upgrades[i].ThisUISO.ThisUIEventFlag);
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
}
