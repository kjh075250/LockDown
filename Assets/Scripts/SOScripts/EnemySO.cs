using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "SO/Enemy")]
public class EnemySO : ScriptableObject
{
    [SerializeField] int attackDamage;
    [SerializeField] float moveSpeed;
    [SerializeField] int hp;
    [SerializeField] Sprite sprite;
    [SerializeField] int coin;

    public int AttackDamage => attackDamage;
    public float MoveSpeed => moveSpeed;
    public int Hp => hp;
    public Sprite EnemySprite => sprite;
    public int Coin => coin;
}
