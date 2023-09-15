using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Transform MoveTarget;
    Transform[] movetargets = null;
    Vector3 nowTarget;

    [SerializeField] float moveSpeed;

    float moveTime = 0;
    float idleTime = 0;
    int index;

    Vector3 direction;

    void Start()
    {
        movetargets = MoveTarget.GetComponentsInChildren<Transform>();
        SetTarget();
    }

    void Update()
    {
        moveTime = Mathf.Clamp(moveTime + Time.deltaTime, 0, 10);
        if(moveTime == 10)
        {
            nowTarget = transform.position;
            idleTime = Mathf.Clamp(idleTime + Time.deltaTime, 0, 5);
            if(idleTime == 5)
            {
                SetTarget();
                idleTime = 0;
                moveTime = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (nowTarget == null) return;
        direction = (nowTarget - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        Debug.DrawLine(transform.position, nowTarget);
    }

    void SetTarget()
    {
        if (movetargets == null) return;
        index = Random.Range(0, movetargets.Length - 1);
        nowTarget = movetargets[index].localPosition;
        while (nowTarget == movetargets[index].localPosition)
            nowTarget = movetargets[Random.Range(0, movetargets.Length - 1)].localPosition;
    }
}
