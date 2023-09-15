using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] private bool isBackGround;
    [SerializeField] private EnemyUnit enemyUnit;
    private Image image;

    void Awake()
    {
        image = GetComponentInChildren<Image>();
        if (enemyUnit.Hp == enemyUnit.MaxHp)
            image.gameObject.SetActive(false);
    }

    public void HpbarInit()
    {
        image.gameObject.SetActive(false);
    }

    public void Hpbar()
    {
        if (image.gameObject.activeInHierarchy == true) return;
            image.gameObject.SetActive(true);
    }

    void Update()
    {
        if (isBackGround) return;
        float a = (float)enemyUnit.Hp / (float)enemyUnit.MaxHp;
        image.fillAmount = a;
    }
}
