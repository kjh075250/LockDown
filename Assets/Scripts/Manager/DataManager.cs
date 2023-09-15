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


}

public class DataManager : MonoBehaviour
{
    string path;

    void Start()
    {
        //Path.Combine(Application.persistentDataPath, "database.json");
        path = Path.Combine(Application.dataPath + "/Data/", "database.json");
        JsonLoad();
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
                saveData.stat = GameManager.Instance.Player.PlayerStat;
            }
        }
    }

    public void JsonSave()
    {
        SaveData saveData = new SaveData();
        saveData.stat = GameManager.Instance.Player.PlayerStat;
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
}
