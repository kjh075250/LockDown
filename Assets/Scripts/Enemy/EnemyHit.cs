using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    SpriteRenderer sprite;

    [SerializeField] Material currentMaterial;
    [SerializeField] Material whiteMaterial;

    WaitForSeconds wait = new WaitForSeconds(0.12f);

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.material = currentMaterial;
    }

    public void hitInit()
    {
        sprite.material = currentMaterial;
    }

    public void ColorChange()
    {
        if(this.enabled)
            StartCoroutine(ColorChangeCoroutine());
    }

    IEnumerator ColorChangeCoroutine()
    {
        sprite.material = whiteMaterial;
        yield return wait;
        sprite.material = currentMaterial;
    }

    public void SetSprite(Sprite _sprite)
    {
        this.sprite.sprite = _sprite;
    }
}
