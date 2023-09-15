using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance;
    public static SkillManager Instance => instance;

    public GameObject missileObject;
    public GameObject missileEffectObject;
    public GameObject missileTargetObj;

    [SerializeField] Transform hyperStartTrans;
    [SerializeField] Transform trans;
    [SerializeField] LayerMask layer;

    private LineRenderer hyperLaserRenderer;
    private float overCluckEffectTime = 10;
    private float time_OverCluck = 0;
    private bool isOverCluckKnow;

    private void Awake()
    {
        if (instance == null)
            instance = GetComponent<SkillManager>();
        hyperLaserRenderer = trans.GetComponent<LineRenderer>();
    }

    private void Start()
    {
        SetMisslie();
        time_OverCluck = overCluckEffectTime;
        isOverCluckKnow = false;
    }

    private void Update()
    {
        if (!isOverCluckKnow) return;
        time_OverCluck = Mathf.Clamp(time_OverCluck + Time.deltaTime, 0, overCluckEffectTime);

        if (time_OverCluck == overCluckEffectTime)
        {
            GameManager.Instance.Player.OverCluck(false);
            isOverCluckKnow = false;
        }
    }

    void SetMisslie()
    {
        missileObject = ResourceManager.Instance.Load<GameObject>("Prefabs/MissileObject");
        missileEffectObject = ResourceManager.Instance.Load<GameObject>("Prefabs/ExplosionObject");
        missileTargetObj = ResourceManager.Instance.Load<GameObject>("Prefabs/Target");

        PoolManager.Instance.CreatePool(missileObject, 200);
        PoolManager.Instance.CreatePool(missileEffectObject, 200);
        PoolManager.Instance.CreatePool(missileTargetObj, 200);
    }

    public void UseMissile(int index, int damage)
    {
        StartCoroutine(MissileCoroutine(index, damage));
    }

    IEnumerator MissileCoroutine(int index, int damage)
    {
        for(int i = 0; i < index; i++)
        {
            Poolable obj = PoolManager.Instance.Pop(missileObject);
            obj.GetComponent<MissileObject>().SetDamage(damage);
            obj.GetComponent<MissileObject>().MissileInit();
            obj.transform.position = new Vector3(GameManager.Instance.Orb.position.x + Random.Range(-0.2f, 0.2f), GameManager.Instance.Orb.position.y + Random.Range(-0.2f, 0.2f)) ;
            yield return new WaitForSeconds(0.045f);
        }
    }

    public void HyperLaser(int damage)
    {
        StartCoroutine(HyperLaserCoroutine(damage));
    }

    IEnumerator HyperLaserCoroutine(int damage)
    {
        yield return new WaitForSeconds(1f);
        EventManager.Instance.TriggerEvent(EventManager.EventFlags.LaserAttack);

        float degree = 0;
        trans.position = GameManager.Instance.Player.transform.position;
        hyperStartTrans.position = GameManager.Instance.Player.transform.position;

        trans.Translate(trans.right * 100);
        hyperStartTrans.Translate(hyperStartTrans.right * 1.15f);

        hyperLaserRenderer.SetWidth(5, 5);
        hyperLaserRenderer.gameObject.SetActive(true);
        while (degree < 1.95f)
        {
            hyperStartTrans.RotateAround(GameManager.Instance.Player.transform.position, Vector3.forward, degree);
            hyperLaserRenderer.SetPosition(0, hyperStartTrans.position);

            trans.RotateAround(GameManager.Instance.Player.transform.position, Vector3.forward, degree);
            hyperLaserRenderer.SetPosition(1, trans.position);

            degree = Mathf.Clamp(degree + 0.005f, 0, 1.95f);
            for(int i = 0; i < EnemyManager.Instance.EnemysList.Count; i++)
            {
                EnemyUnit enemy = EnemyManager.Instance.EnemysList[i];

                Vector3 dirToTarget = (enemy.transform.position - hyperStartTrans.position).normalized;

                if (Vector3.Dot((trans.position - hyperStartTrans.position).normalized, dirToTarget) > Mathf.Cos((90 / 2) * Mathf.Deg2Rad))
                {
                    if (enemy && !enemy.IsDead)
                    {
                        enemy.Damage(damage);
                        EventManager.Instance.TriggerEvent(EventManager.EventFlags.LaserHit);
                    }
                }
            }

            yield return new WaitForSeconds(0.01f);
        }

        float time = 0;
        while(time < 1f)
        {
            time = Mathf.Clamp(time + 0.025f, 0, 1);
            float x =  Mathf.Lerp(5, 0.1f, time);

            hyperLaserRenderer.SetWidth(x, x);
            yield return new WaitForSeconds(0.01f);
        }

        hyperLaserRenderer.gameObject.SetActive(false);
    }

    public void OverCluck()
    {
        time_OverCluck = 0;
        isOverCluckKnow = true;
        GameManager.Instance.Player.OverCluck(true);
    }
    
    public float GetOverCluckTime()
    {
        return overCluckEffectTime;
    }

    public void SetOverCluckEffectTime(float value)
    {
        overCluckEffectTime = value;
    }

    public void AddOverCluckEffectTime()
    {
        overCluckEffectTime += 5f;
    }
}
