using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UISO", menuName = "SO/UIs")]
public class UISO : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] string summary;
    [SerializeField] string nowStatusText;

    [SerializeField] float nowStatus;
    [SerializeField] float value;

    [SerializeField] Sprite icon;

    [SerializeField] int upgradeCost;
    [SerializeField] float costIncreaseRatio;
    [SerializeField] int currentLevel;
    [SerializeField] int maxLevel;

    [SerializeField] UIManager.UIEventFlags thisUIEventFlag;

    [SerializeField] UISO defaultData;

    public string SkillName => _name;
    public string Summary => summary;
    public string NowStatusText => nowStatusText;

    public float NowStatus => nowStatus;
    public float Value => value;

    public Sprite Icon => icon;

    public int UpgradeCost => upgradeCost;
    public float CostIncreaseRatio => costIncreaseRatio;
    public int CurrentLevel => currentLevel;
    public int MaxLevel => maxLevel;


    public UIManager.UIEventFlags ThisUIEventFlag => thisUIEventFlag;


    public void SetCost(int value)
    {
        upgradeCost = value;
    }

    public void SetValue(float _value)
    {
        value = _value;
    }

    public void SetNowStatus(float value)
    {
        nowStatus = value;
    }

    public void LevelUp()
    {
        currentLevel++;
    }

    public bool IsUpgradeComplete()
    {
        if (currentLevel >= maxLevel)
        {
            return true;
        }
        else
            return false;
    }


    public void SetData(UISO uiSO)
    {
         _name = uiSO.SkillName;
         summary = uiSO.Summary;
         nowStatusText =uiSO.NowStatusText;

         nowStatus = uiSO.NowStatus;
         value = uiSO.Value;

         icon = uiSO.Icon;

         upgradeCost = uiSO.UpgradeCost;
         costIncreaseRatio =uiSO.CostIncreaseRatio;
         currentLevel = uiSO.CurrentLevel;
         maxLevel = uiSO.MaxLevel;
    }

    public void SetJSonUIData(SaveData saveData, int index)
    {
        _name = saveData._name[index];
        summary = saveData.summary[index];
        nowStatusText = saveData.nowStatusText[index];
        nowStatus = saveData.nowStatus[index];
        value = saveData.value[index];
        icon = saveData.icon[index];
        upgradeCost = saveData.upgradeCost[index];
        costIncreaseRatio = saveData.costIncreaseRatio[index];
        currentLevel = saveData.currentLevel[index];
        maxLevel = saveData.maxLevel[index];
        thisUIEventFlag = saveData.thisUIEventFlag[index];
    }

}
