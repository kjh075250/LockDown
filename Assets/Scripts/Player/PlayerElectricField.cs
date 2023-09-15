using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElectricField : MonoBehaviour
{
    Collider2D elecCollider;
    [SerializeField] Transform field;

    int damage;
    float time_thisTime;

    private void Awake()
    {
        elecCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        time_thisTime = Mathf.Clamp(time_thisTime + Time.deltaTime, 0, 2f);

        if(time_thisTime > 0.175f)
        {
            elecCollider.enabled = false;
        }
        if(time_thisTime == 2f)
        {
            field.gameObject.SetActive(false);
            time_thisTime = 0;
        }
    }

    public void TriggerElectricField(int value)
    {
        elecCollider.enabled = false;
        field.gameObject.SetActive(false);
        time_thisTime = 0;

        damage = value;
        elecCollider.enabled = true;
        field.gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyUnit enemy = collision.gameObject.GetComponent<EnemyUnit>();
        if (enemy)
            enemy.Damage(damage);
    }
}
