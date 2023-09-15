using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileObject : MonoBehaviour
{
    float speed;
    float MaxSpeed = 30;
    float value;

    float time_move;
    float time_attackTime;

    float explosionRadius = 2.5f;
    [SerializeField] LayerMask layer;
    int MissileDamage = 10;

    bool IsFalling = false;
    bool IsExplode = false;

    Vector3 nowPos;
    Poolable target;

    private void Start()
    {
        MissileInit();
    }

    public void MissileInit()
    {
        value = 0;
        speed = 0;
        time_move = 0;
        time_attackTime = 0;
        IsFalling = false;
        IsExplode = false;
    }

    void Update()
    {
        time_move = Mathf.Clamp(time_move + Time.deltaTime * 2, 0, 1);
        time_attackTime = Mathf.Clamp(time_attackTime + Time.deltaTime, 0, 2.5f);

        if (time_attackTime < 2.5f)
        {
            UpperMove();
        }
        else
        {
            TargetPositionSetting();
            LowerMove();
        }

    }

    float Ease(float x)
    {
        return x < 0.5
        ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
        : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2;
    }

    float DownEase(float x)
    {
        return x * x * x;
    }

    void UpperMove()
    {
        value = Mathf.Lerp(speed, MaxSpeed, Ease(time_move));
        transform.position += Time.deltaTime * value * transform.up;
    }

    void LowerMove()
    {
        if (IsFalling == false) return;
        transform.position = Vector3.Lerp(nowPos, target.transform.position, DownEase(time_move));

        if (IsExplode) return;
        if (Vector3.Distance(transform.position, target.transform.position) < 1f)
        {
            Explosion();
            IsExplode = true;
        }
    }

    void TargetPositionSetting()
    {
        if (IsFalling) return;
        target = null;
        target = PoolManager.Instance.Pop(SkillManager.Instance.missileTargetObj);
        target.transform.position = GameManager.Instance.Player.transform.position;

        float angle = Random.Range(0f, 360f);
        float dist = Random.Range(2f, 5f);

        target.transform.Rotate(new Vector3(0, 0, angle));
        target.transform.Translate(new Vector3(dist, dist, 0));

        target.transform.rotation = Quaternion.Euler(0, 0, 0);

        nowPos = transform.position;

        IsFalling = true;
        time_move = 0;
    }

    void Explosion()
    {
        Poolable obj = PoolManager.Instance.Pop(SkillManager.Instance.missileEffectObject);
        obj.transform.position = this.transform.position;

        EventManager.Instance.TriggerEvent(EventManager.EventFlags.MissileExplosion);
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layer);
        for(int i = 0; i < cols.Length; i++)
        {
            EnemyUnit enemy = cols[i].GetComponent<EnemyUnit>();
            if (enemy == null || enemy.IsDead) continue;
                enemy.Damage(MissileDamage);
        }

        PoolManager.Instance.Push(target);
        PoolManager.Instance.Push(this.GetComponent<Poolable>());
    }

    public void SetDamage(int value)
    {
        MissileDamage = value;
    }
}
