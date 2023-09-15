using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultAttack : AttackBase
{
    [SerializeField] EnemyUnit _nowTarget;
    LineRenderer lineRenderer;

    float attackDelay = 1f;
    float time_delay = 1f;
    bool isOverCluck;

    float currentDelay;
    
    int damage;

    float lerpTime;
    int laserCount = 0;
    public int LaserCount => laserCount;

    WaitForSeconds wait = new WaitForSeconds(0.005f);
    WaitForSeconds wait2 = new WaitForSeconds(0.1f);

    void Start()
    {
        base.Init();
        radius = 5f;
        lineRenderer = GetComponent<LineRenderer>();
        UIManager.Instance.UIEventAddListener(UIManager.UIEventFlags.AttackSpeedUpgrade, SetDelay);
    }

    void Update()
    {
        if (NowTarget && time_delay == 0)
        {
            time_delay = attackDelay;
            Attack(NowTarget);
        }
        time_delay = Mathf.Clamp(time_delay - Time.deltaTime, 0, 100);

        if (NowTarget)
            _nowTarget = NowTarget;
    }

    void Attack(EnemyUnit target)
    {
        laserCount++;
        GameManager.Instance.Orb.GetComponent<OrbImage>().SetSprite(laserCount);
        target.Damage(damage);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        StartCoroutine(LineCoroutine(target.transform.position));
    }

    IEnumerator LineCoroutine(Vector3 targetVec)
    {
        lerpTime = 0;
        while(lerpTime != 1)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(1,Vector3.Lerp(transform.position, targetVec, lerpTime));
            lerpTime = Mathf.Clamp(lerpTime + 0.25f, 0, 1);
            yield return wait;
        }
        lineRenderer.SetPosition(1, targetVec);
        yield return wait2;
        lineRenderer.enabled = false;
    }

    public void SetLaserCount(int value)
    {
        laserCount = value;
    }

    public float GetDirection()
    {
        if (NowTarget == null) return 0;
        Vector2 direction = (NowTarget.transform.position - this.transform.position).normalized;
        float a = Vector2.Dot(transform.right, direction);
        return a;
    }

    public void SetFirstDelay()
    {
        var value = UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.AttackSpeedUpgrade];
        attackDelay = value.ThisUISO.Value;
        currentDelay = attackDelay;
        value.ThisUISO.SetNowStatus(attackDelay);
        value.SetUI();
    }

    public void SetDelay()
    {
        var value = UIManager.Instance.UIObjDictionary[UIManager.UIEventFlags.AttackSpeedUpgrade];
        value.ThisUISO.SetValue(currentDelay - 0.2f);
        if(!value.ThisUISO.IsUpgradeComplete())
        {
            value.ThisUISO.LevelUp();
        }
        currentDelay = value.ThisUISO.Value;

        if (!isOverCluck)
            attackDelay = currentDelay;
        
        value.ThisUISO.SetNowStatus(attackDelay);
        value.SetUI();
    }

    public void SetDamage(int value)
    {
        damage = value;
    }

    public void OverCluck(bool isTurnOn)
    {
        if(isTurnOn == true)
        {
            isOverCluck = true;
            currentDelay = attackDelay;
            attackDelay = 0.15f;
        }
        else
        {
            isOverCluck = false;
            attackDelay = currentDelay;
        }
    }

}
