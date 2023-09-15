using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private PlayerUnit target;
    [SerializeField] private float AttackDistance = 2f;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float attackDelay = 3f;

    private float time_delay = 1f;
    private int damage = 1;

    private bool isCanMove;
    private bool isMoveStop;
    private bool isAttacking;

    void Start()
    {
        target = GameManager.Instance.Player;
        isCanMove = true;
        isAttacking = false;
        isMoveStop = false;
    }

    void Update()
    {
        CheckDistance();
        Attack();
        Move();
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(this.transform.position, target.transform.position) <= AttackDistance)
        {
            isCanMove = false;
            isAttacking = true;
        }
        else
        {
            if (isMoveStop)
                isCanMove = false;
            else
                isCanMove = true;
            isAttacking = false;
        }
    }

    private void Attack()
    {
        time_delay = Mathf.Clamp(time_delay - Time.deltaTime, 0, attackDelay);
        if (!isAttacking) return;
        if (time_delay == 0)
        {
            target.Damage(damage);
            time_delay = attackDelay;
        }
    }

    private void Move()
    {
        if (!isCanMove) return;
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    public void SetDamage(int speed)
    {
        damage = speed;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void SetMoveStop(bool moveStop)
    {
        isMoveStop = moveStop;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

}
