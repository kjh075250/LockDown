using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_UpgradeButtons : UI_Object
{
    [SerializeField] UISO thisUISO;
    public UISO ThisUISO => thisUISO;
    UIManager.UIEventFlags thisUIEventFlag;

    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI summaryText;
    [SerializeField] private TextMeshProUGUI nowStatusText;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Button button;
    [SerializeField] private SkillButton buttonlist;
    
    public bool IsMissile;
    public bool IsOverCluck;
    public bool IsHyperLaser;

    public void OnEnable()
    {
        SetUI();
    }

    public override void SetUI()
    {
        thisUIEventFlag = thisUISO.ThisUIEventFlag;
        iconImage.sprite = thisUISO.Icon;
        nameText.text = thisUISO.SkillName;
        summaryText.text = thisUISO.Summary;
        nowStatusText.text = thisUISO.NowStatusText + " : " + thisUISO.NowStatus.ToString();


        if (thisUISO.IsUpgradeComplete() && button.interactable == true) 
        {
            DisableButton("더 이상 <br> 구매 불가");
        }

        if(!thisUISO.IsUpgradeComplete())
        {
            buttonText.text = "구매 하기 <br> " + thisUISO.UpgradeCost.ToString() + "골드";
            button.interactable = true;
        }

        
        if(IsMissile || IsOverCluck || IsHyperLaser)
        {
            DisableButton("스킬 <br> 구매하지 않음");
        }

    }

    public void ButtonClick()
    {
        if(GameManager.Instance.Gold > thisUISO.UpgradeCost)
        {
            UIManager.Instance.TriggerUIEvent(thisUIEventFlag);
            Purchase();
        }
    }

    public UIManager.UIEventFlags GetFlag()
    {
        return thisUIEventFlag;
    }

    public void DisableButton(string text)
    {
        buttonText.text = text;
        button.interactable = false;
    }

    public void Purchase()
    {
        GameManager.Instance.AddMoney(-thisUISO.UpgradeCost);
        thisUISO.SetCost((int)(thisUISO.UpgradeCost * thisUISO.CostIncreaseRatio));
        SetUI();
    }

    
}
