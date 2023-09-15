using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserAttack : AttackBase
{
    LineRenderer lineRenderer;
    WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.75f);
    public bool isalreadyHitLaser = false;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        isalreadyHitLaser = false;
        radius = 4f;
    }

    public void ChainLaser(int count, int laserCount, int damage)
    {
        if (isalreadyHitLaser) return;
        isalreadyHitLaser = true;

        if (this.enabled == false) return;
        StartCoroutine(ChainLaserCoroutine(count, laserCount, damage));
    }

    IEnumerator ChainLaserCoroutine(int count, int laserCount, int damage)
    {
        base.Overlap();
        yield return new WaitForSeconds(0.025f);
        Vector3 vec = transform.position;
        if (laserCount > Chaintargets.Count) laserCount = Chaintargets.Count;

        lineRenderer.positionCount = laserCount * 2;
        if (Chaintargets.Count > 0)
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
                    lineRenderer.SetPosition(i, Chaintargets[i / 2].transform.position);
                    if(count > 0)
                    {
                        EnemyUnit target = Chaintargets[i / 2]?.GetComponent<EnemyUnit>();
                        if (target?.GetComponent<EnemyLaserAttack>().isalreadyHitLaser == true || target.IsDead == true)
                        {
                            for (int j = 0; j < Chaintargets.Count; j++)
                            {
                                if (Chaintargets[j]?.GetComponent<EnemyLaserAttack>().isalreadyHitLaser == false && Chaintargets[j]?.GetComponent<EnemyUnit>().IsDead == false)
                                {
                                    target = Chaintargets[j]?.GetComponent<EnemyUnit>();
                                    break;
                                }
                                else
                                    continue;
                            }
                        }
                        if (target?.GetComponent<EnemyLaserAttack>().isalreadyHitLaser == false && target.IsDead == false)
                        {
                            count--;
                            target?.GetComponent<EnemyLaserAttack>().ChainLaser(count, laserCount, damage);
                        }
                    }
                    Chaintargets[i / 2].GetComponent<EnemyUnit>().Damage(damage);
                }
            }
        }
        yield return wait;
        lineRenderer.enabled = false;
        isalreadyHitLaser = false;
    }

    public void laserInit()
    {
        lineRenderer.enabled = false;
        isalreadyHitLaser = false;
        lineRenderer.positionCount = 0;
    }

}
