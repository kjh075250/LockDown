using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = GetComponent<EnemyManager>();
    }

    GameObject coin;
    public GameObject Coin => coin;
    
    GameObject enemy;
    
    List<EnemyUnit> enemys = new List<EnemyUnit>();
    public List<EnemyUnit> EnemysList => enemys;

    List<EnemyUnit> raidEnemys = new List<EnemyUnit>();

    [SerializeField] List<EnemySO> enemySOList = new List<EnemySO>();
    public List<EnemySO> EnemySOList => enemySOList;

    [SerializeField] float enemySummonDelay = 0.1f;
    float setenemySummonDelay = 0.35f;
    [SerializeField] int MaxEnemyCount;

    float time_SummonTime;
    float time_RaidEndTime;

    private bool IsRaid;
    
    Vector3 angleVec;
    Vector3 enemyVec;

    void Start()
    {
        IsRaid = false;
        enemy = ResourceManager.Instance.Load<GameObject>("Prefabs/Enemy");
        coin = ResourceManager.Instance.Load<GameObject>("Prefabs/Coin");
        enemys.Clear();
        PoolManager.Instance.CreatePool(enemy, 500);
        PoolManager.Instance.CreatePool(coin, 500);
        angleVec = Vector3.zero;
        enemyVec = Vector3.zero;
        enemySummonDelay = setenemySummonDelay;
    }

    void Update()
    {
        time_SummonTime = Mathf.Clamp(time_SummonTime + Time.deltaTime, 0, enemySummonDelay);

        if(IsRaid)
        {
            if(raidEnemys.Count < MaxEnemyCount)
            {
                Summon();
            }
            else if(raidEnemys.Count == MaxEnemyCount && enemys.Count == 0)
            {
                time_RaidEndTime = Mathf.Clamp(time_RaidEndTime + Time.deltaTime, 0, 1f);
                if (time_RaidEndTime == 1)
                {
                    raidEnemys.Clear();
                    EventManager.Instance.TriggerEvent(EventManager.EventFlags.RaidEnd);
                    enemySummonDelay = setenemySummonDelay;
                    IsRaid = false;
                }
            }
        }
        else
        {
            if (enemys.Count < MaxEnemyCount)
            {
                Summon();
            }
        }
    }

    void Summon()
    {
        if (time_SummonTime == enemySummonDelay)
        {
            SummonEnemy();
            time_SummonTime = 0;
        }
    }

    void SummonEnemy()
    {
        Poolable thisEnemy = PoolManager.Instance.Pop(enemy, EnemyManager.Instance.transform);
        thisEnemy.transform.position = GameManager.Instance.Player.transform.position;

        int waveindex = GameManager.Instance.GetWave();
        if (thisEnemy.GetComponent<EnemyUnit>()._thisindex != waveindex)
        {
            thisEnemy.GetComponent<EnemyUnit>().EnemyUpgrade(enemySOList[waveindex].AttackDamage, 
                enemySOList[waveindex].MoveSpeed,
                enemySOList[waveindex].Hp,
                enemySOList[waveindex].EnemySprite,
                enemySOList[waveindex].Coin);
            thisEnemy.GetComponent<EnemyUnit>()._thisindex = waveindex;
        }

        angleVec.z = Random.Range(0f, 360f);
        enemyVec.x = Random.Range(11f, 18f);
        enemyVec.y = Random.Range(11f, 18f);

        thisEnemy.transform.Rotate(angleVec);
        thisEnemy.transform.Translate(enemyVec);
        thisEnemy.transform.rotation = Quaternion.Euler(0, 0, 0);

        thisEnemy.GetComponent<EnemyUnit>().EnemyInit();

        if(IsRaid)
            raidEnemys.Add(thisEnemy.GetComponent<EnemyUnit>());

        enemys.Add(thisEnemy.GetComponent<EnemyUnit>());
    }

    public void dead(EnemyUnit enemy)
    {
        enemys.Remove(enemy);
    }

    public void StartRaid()
    {
        if (IsRaid) return;
        IsRaid = true;
        enemySummonDelay = 0.1f;
        raidEnemys.Clear();
        EventManager.Instance.TriggerEvent(EventManager.EventFlags.RaidStart);
        GameManager.Instance.NextWave();
    }

    public bool GetIsRaid()
    {
        return IsRaid;
    }
}
