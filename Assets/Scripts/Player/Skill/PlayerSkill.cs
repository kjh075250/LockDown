using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] List<SkillObject> skillList;
    [SerializeField] bool IsAuto;

    [SerializeField] Button AutoButton;
    [SerializeField] TextMeshProUGUI AutoButtonText;

    public List<SkillObject> SkillList => skillList;

    private WaitForSeconds wait = new WaitForSeconds(1f);

    public List<bool> isSkillNotPurchased;

    private void Start()
    {
        StartCoroutine(AutoUseSkill());
    }

    IEnumerator AutoUseSkill()
    {
        while(true)
        {
            if (!IsAuto)
            {
                yield return wait;
                continue;
            }
            if(!isSkillNotPurchased[0])
            {
                skillList[0].UseSkill();
            }

            if(!isSkillNotPurchased[1])
            {
                skillList[1].UseSkill();
            }

            if(!isSkillNotPurchased[2])
            {
                skillList[2].UseSkill();
            }
            yield return wait;
        }
    }

    public void SetAuto()
    {
        if (IsAuto)
        {
            IsAuto = false;
            AutoButtonText.text = "AUTO <br> OFF";
        }
        else
        {
            IsAuto = true;
            AutoButtonText.text = "AUTO <br> ON";
        }
    }
}
