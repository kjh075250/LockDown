using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookPlayer : MonoBehaviour
{
    private PlayerUnit target;
    private Vector3 dotAxis;

    private void Start()
    {
        dotAxis = transform.right;
        target = GameManager.Instance.Player;
    }
    private void Update()
    {
        CheckDirection();
    }

    private void CheckDirection()
    {
        if (GetDirection() > 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
    }

    private float GetDirection()
    {
        Vector2 dir = (target.transform.position - transform.position).normalized;
        float a = Vector2.Dot(dotAxis, dir);
        return a;
    }
}
