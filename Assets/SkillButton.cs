using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] List<Button> buttons;
    
    public void SetMissileButton(bool isPurchase)
    {
        buttons[0].gameObject.SetActive(isPurchase == false);
    }
    public void SetHyperButton(bool isPurchase)
    {
        buttons[1].gameObject.SetActive(isPurchase == false);
    }
    public void SetOverCluckButton(bool isPurchase)
    {
        buttons[2].gameObject.SetActive(isPurchase == false);
    }
}
