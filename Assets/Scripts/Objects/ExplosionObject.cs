using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObject : MonoBehaviour
{
    public void DestroyExplosion()
    {
        PoolManager.Instance.Push(this.gameObject.GetComponent<Poolable>());
    }
}
