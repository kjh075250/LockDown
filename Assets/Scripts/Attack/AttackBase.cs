using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AttackBase : MonoBehaviour
{
    protected float radius = 3f;

    [SerializeField] LayerMask layer;
    WaitForSeconds wait = new WaitForSeconds(0.2f);
    Collider2D[] collders;

    EnemyUnit nowTarget = null;
    public EnemyUnit NowTarget => nowTarget;

    List<EnemyUnit> nowtargets;
    public List<EnemyUnit> NowTargets => nowtargets;

    List<Transform> chaintargets;
    public List<Transform> Chaintargets => chaintargets;

    protected void Init()
    {
        nowtargets = new List<EnemyUnit>();
        StartCoroutine(OverlapCoroutine());
    }

    IEnumerator OverlapCoroutine()
    {
        while(true)
        {
            collders = Physics2D.OverlapCircleAll(transform.position, radius, layer);
            nowtargets.Clear();

            for(int i = 0; i < collders.Length; i++)
            {
                if(collders[i].GetComponent<EnemyUnit>().IsDead == false)
                    nowtargets.Add(collders[i].GetComponent<EnemyUnit>());
            }

            if (nowTarget == null || nowTarget.gameObject.activeInHierarchy == false || nowTarget.IsDead)
            {
                nowTarget = null;
                if (nowtargets.Count > 0)
                {
                    float shortDistance = Vector2.Distance(transform.position, nowtargets[0].transform.position);
                    nowTarget = nowtargets[0].GetComponent<EnemyUnit>();
                    foreach (EnemyUnit enemy in nowtargets)
                    {
                        float distance = Vector2.Distance(transform.position, enemy.transform.position);
                        if (distance < shortDistance)
                        {
                            shortDistance = distance;
                            nowTarget = enemy;
                        }
                    }
                }
            }
            yield return wait;
        }
    }

    protected void Overlap()
    {
        chaintargets = new List<Transform>();
        collders = Physics2D.OverlapCircleAll(transform.position, radius, layer);
        chaintargets.Clear();
        for (int i = 0; i < collders.Length - 1; i++)
        {
            if(collders[i].GetComponent<EnemyLaserAttack>().isalreadyHitLaser == false)
            {
                chaintargets.Add(collders[i].GetComponent<Transform>());
            }
        }
        List<Transform> temp = chaintargets.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToList();
        chaintargets = temp;
    }

}
