using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMove : MonoBehaviour
{
    [SerializeField] PlayerDefaultAttack defaultAttack;
    
    void Update()
    {
        if (defaultAttack.NowTarget == null) return;
        Vector2 vec = defaultAttack.NowTarget.transform.position - transform.position;
        LookAt2DLerp(this.transform, vec.normalized);
    }

    public void LookAt2DLerp(Transform transform, Vector2 dir, float lerpPercent = 0.05f)
    {
        float rotationZ = Mathf.Acos(dir.x / dir.magnitude)
            * 180 / Mathf.PI
            * Mathf.Sign(dir.y);
        float x;
        if (defaultAttack.GetDirection() > 0)
        {
            x = 0;
        }
        else
        {
            x = 180;
            rotationZ = -rotationZ;
        }

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(x, 0, rotationZ),
            lerpPercent
        );
    }
}
