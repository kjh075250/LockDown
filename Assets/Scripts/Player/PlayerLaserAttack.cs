using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserAttack : AttackBase
{
    LineRenderer lineRenderer;
    WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.75f);

    [SerializeField] Transform particle;

    void Start()
    {
        base.Init();
        radius = 7.5f;
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Attack(int lasers, int chainlaserCount, int chain, int damage)
    {
        StartCoroutine(LaserCoroutine(lasers, chainlaserCount, chain,  damage));
    }

    public void SetEffectActive(bool isActive)
    {
        particle.gameObject.SetActive(isActive);
    }

    IEnumerator LaserCoroutine(int lasers, int chainlaserCount, int chain, int damage)
    {
        Vector3 vec = transform.position;
        if (lasers > NowTargets.Count)
            lasers = NowTargets.Count;
        lineRenderer.positionCount = lasers * 2;
        if (NowTargets.Count > 0)
        {
            lineRenderer.enabled = true;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                if (i % 2 == 0)
                {
                    lineRenderer.SetPosition(i, vec);
                }
                else
                {
                    lineRenderer.SetPosition(i, NowTargets[i / 2].transform.position);
                    NowTargets[i / 2]?.Damage(damage);
                    if (chainlaserCount > 0)
                    {
                        chainlaserCount--;
                        NowTargets[i / 2]?.laserAttack.ChainLaser(chainlaserCount, chain, damage);
                    }
                }
            }
        }
        yield return wait;
        lineRenderer.enabled = false;
    }
}
