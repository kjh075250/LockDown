using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMove : MonoBehaviour
{
    [SerializeField] PlayerDefaultAttack defaultAttack;

    void Update()
    {
        CheckDirection();
    }

    private void CheckDirection()
    {
        if (defaultAttack.GetDirection() >= 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else if(defaultAttack.GetDirection() < 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);

        }
    }
}
