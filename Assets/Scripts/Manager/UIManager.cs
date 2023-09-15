using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = GetComponent<UIManager>();

        uiObjDictionary.Clear();
        for (int i = 0; i < ui_Upgrades.Count; i++)
        {
            ui_Upgrades[i].SetUI();
            uiObjDictionary.Add(ui_Upgrades[i].GetFlag(), ui_Upgrades[i]);
        }
    }

    public enum UIEventFlags
    {
        AttackSpeedUpgrade,
        LaserDamageUpgrade,
        ChainLaserCountUpgrade,
        ChainLaserUpgrade,
        ElectricFieldUpgrade,
        HyperLaserUpgrade,
        MissileAmountUpgrade,
        MissileDamageUpgrade,
        OverCluckUpgrade,
        MissilePurchase,
        OverCluckPurchase,
        HyperLaserPurchase,
        PlayerMaxHPUpgrade,
        PlayerRegenUpgrade,
    }

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI Raidtext;
    [SerializeField] TextMeshProUGUI WaveText;
    [SerializeField] TextMeshProUGUI HPText;
    [SerializeField] Image HPImage;

    [SerializeField] Button RaidButton;
    [SerializeField] List<Image> skillImage;
    [SerializeField] List<UI_UpgradeButtons> ui_Upgrades;

    [SerializeField] List<Button> uiPanelButtons;
    [SerializeField] List<GameObject> uiPanels;

    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject RaidFailedPanel;

    private Dictionary<UIEventFlags, UI_UpgradeButtons> uiObjDictionary = new Dictionary<UIEventFlags, UI_UpgradeButtons>();
    public Dictionary<UIEventFlags, UI_UpgradeButtons> UIObjDictionary => uiObjDictionary;

    public SkillImage SkillImageEffect;

    private float time_RaidTextEffect;
    private float frequency = 0.25f;
    private bool raidText;

    void Start()
    {
        Debug.Log("Start");
        GameOverPanel.SetActive(false);
        RaidFailedPanel.SetActive(false);

        SetUI();
        EventManager.Instance.StopListening(EventManager.EventFlags.RaidStart, RaidStartText);
        EventManager.Instance.StopListening(EventManager.EventFlags.RaidEnd, RaidEndText);
        EventManager.Instance.StopListening(EventManager.EventFlags.GameOver, SetGameOverPanel);
        EventManager.Instance.StopListening(EventManager.EventFlags.RaidFailed, SetRaidFailPanel);

        EventManager.Instance.AddListener(EventManager.EventFlags.RaidStart, RaidStartText);
        EventManager.Instance.AddListener(EventManager.EventFlags.RaidEnd, RaidEndText);
        EventManager.Instance.AddListener(EventManager.EventFlags.GameOver, SetGameOverPanel);
        EventManager.Instance.AddListener(EventManager.EventFlags.RaidFailed, SetRaidFailPanel);

        UIEventStopListening(UIEventFlags.MissilePurchase, CheckPurchaseMissile);
        UIEventStopListening(UIEventFlags.OverCluckPurchase, CheckPurchaseOverCluck);
        UIEventStopListening(UIEventFlags.HyperLaserPurchase, CheckPurchaseHyperLaser);

        UIEventAddListener(UIEventFlags.MissilePurchase, CheckPurchaseMissile);
        UIEventAddListener(UIEventFlags.OverCluckPurchase, CheckPurchaseOverCluck);
        UIEventAddListener(UIEventFlags.HyperLaserPurchase, CheckPurchaseHyperLaser);

        time_RaidTextEffect = 0;
    }

    private void Update()
    {
        if (!raidText) return;
        time_RaidTextEffect = Mathf.Clamp(time_RaidTextEffect + Time.deltaTime, 0, 2);
        Raidtext.alpha = Mathf.Sin(2 * Mathf.PI * frequency * time_RaidTextEffect) * 1;

        if (time_RaidTextEffect == 2)
        {
            raidText = false;
            time_RaidTextEffect = 0;
        }

    }

    public void SetUI()
    {
        text.text = GetThousandCommaText(GameManager.Instance.Gold).ToString();
        for (int i = 0; i < skillImage.Count; i++)
        {
            skillImage[i].sprite = GameManager.Instance.Player.GetComponent<PlayerSkill>().SkillList[i].SkillSO.Icon;
        }

        for (int i = 0; i < ui_Upgrades.Count; i++)
        {
            ui_Upgrades[i].SetUI();
        }
    }

    private string GetThousandCommaText(double data)
    {
        return string.Format("{0:#,###}", data);
    }

    private Dictionary<UIEventFlags, Action> uieventDictionary = new Dictionary<UIEventFlags, Action>();

    public void UIEventAddListener(UIEventFlags flag, Action listener)
    {
        Action thisEvent;
        if (uieventDictionary.TryGetValue(flag, out thisEvent))
        {
            thisEvent += listener;
            uieventDictionary[flag] = thisEvent;
        }
        else
        {
            uieventDictionary.Add(flag, listener);
        }
    }

    public void UIEventStopListening(UIEventFlags flag, Action listener)
    {
        Action thisEvent;
        if (uieventDictionary.TryGetValue(flag, out thisEvent))
        {
            thisEvent -= listener;
            uieventDictionary[flag] = thisEvent;
        }
        else
        {
            uieventDictionary.Remove(flag);
        }
    }

    public void TriggerUIEvent(UIEventFlags flag)
    {
        if (uieventDictionary[flag] == null)
        {
            Debug.Log(flag + "not Existing");
            return;
        }
        uieventDictionary[flag]?.Invoke();
    }

    void RaidStartText()
    {
        Raidtext.text = "강력한 적들이 몰려옵니다..!";
        RaidButton.interactable = false;
        raidText = true;
    }

    void RaidEndText()
    {
        Raidtext.text = "습격 종료";
        if (EnemyManager.Instance.EnemySOList.Count - 1 >= GameManager.Instance.GetWave())
            RaidButton.interactable = false;
        else
            RaidButton.interactable = true;
        raidText = true;
    }

    public void SetWaveText(int value)
    {
        WaveText.text = "현재 웨이브 : " + value.ToString();
    }

    public void SetHPUI()
    {
        HPText.text = "플레이어 체력 : " + GameManager.Instance.Player.Hp;
        HPImage.fillAmount = (float)((float)GameManager.Instance.Player.Hp / (float)GameManager.Instance.Player.MaxHp);
    }

    void CheckPurchaseMissile()
    {
        for (int i = 0; i < ui_Upgrades.Count; i++)
        {
            ui_Upgrades[i].IsMissile = false;
        }
        uiObjDictionary[UIEventFlags.MissilePurchase].ThisUISO.LevelUp();
        GameManager.Instance.Player.SetIsMissilePurchased();
        uiObjDictionary[UIEventFlags.MissilePurchase].SetUI();
    }

    void CheckPurchaseHyperLaser()
    {
        for (int i = 0; i < ui_Upgrades.Count; i++)
        {
            ui_Upgrades[i].IsHyperLaser = false;
        }
        uiObjDictionary[UIEventFlags.HyperLaserPurchase].ThisUISO.LevelUp();
        GameManager.Instance.Player.SetIsHyperPurchased();
        uiObjDictionary[UIEventFlags.HyperLaserPurchase].SetUI();

    }

    void CheckPurchaseOverCluck()
    {
        for (int i = 0; i < ui_Upgrades.Count; i++)
        {
            ui_Upgrades[i].IsOverCluck = false;
        }
        uiObjDictionary[UIEventFlags.OverCluckPurchase].ThisUISO.LevelUp();
        GameManager.Instance.Player.SetIsOverCluckPurchased();
        uiObjDictionary[UIEventFlags.OverCluckPurchase].SetUI();

    }

    public void SetPlayerUIPanel()
    {
        for(int i = 0; i < uiPanels.Count; i++)
        {
            uiPanels[i].SetActive(false);
            uiPanelButtons[i].interactable = true;
        }
        uiPanels[0].SetActive(true);
        uiPanelButtons[0].interactable = false;
    }
    public void SetAttackUIPanel()
    {
        for (int i = 0; i < uiPanels.Count; i++)
        {
            uiPanels[i].SetActive(false);
            uiPanelButtons[i].interactable = true;
        }
        uiPanels[1].SetActive(true);
        uiPanelButtons[1].interactable = false;
    }
    public void SetSkillUpgradeUIPanel()
    {
        for (int i = 0; i < uiPanels.Count; i++)
        {
            uiPanels[i].SetActive(false);
            uiPanelButtons[i].interactable = true;
        }
        uiPanels[2].SetActive(true);
        uiPanelButtons[2].interactable = false;
    }
    public void SetSkillPurchaseUIPanel()
    {
        for (int i = 0; i < uiPanels.Count; i++)
        {
            uiPanels[i].SetActive(false);
            uiPanelButtons[i].interactable = true;
        }
        uiPanels[3].SetActive(true);
        uiPanelButtons[3].interactable = false;
    }

    void SetGameOverPanel()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void SetRaidFailPanel()
    {
        RaidFailedPanel.SetActive(true);
        Time.timeScale = 0f;
    }

}
