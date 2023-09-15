using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObject : MonoBehaviour
{
    Transform now;
    Transform target;

    public int value = 10;

    float time_moveTime;

    float speed;
    void Start()
    {
        now = this.transform;
        target = GameManager.Instance.Player.transform;
        time_moveTime = 0;
        speed = Random.Range(0.2f, 0.7f);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, target.position) < 1f)
            AddCoin();
        time_moveTime = Mathf.Clamp(time_moveTime + Time.deltaTime * speed, 0, 1);
        transform.position = Vector3.Slerp(now.transform.position, target.transform.position, Ease(time_moveTime));

    }

    void AddCoin()
    {
        time_moveTime = 0;
        speed = Random.Range(0.2f, 0.7f);
        GameManager.Instance.AddMoney(value);
        UIManager.Instance.SetUI();
        PoolManager.Instance.Push(this.GetComponent<Poolable>());
    }

    float Ease(float x)
    {
        return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    }

    public void SetValue(int _value)
    {
        value = _value;
    }
}
