using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillImage : MonoBehaviour
{
    RectTransform rectTransform;
    WaitForSeconds wait = new WaitForSeconds(0.01f);

    [SerializeField] Image skill;
    [SerializeField] TextMeshProUGUI tmp;

    Vector3 firstPos;
    Vector3 targetPos;
    Vector3 LastPos;

    public bool IsUsingSkillEffect;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        firstPos = new Vector3(-1600, 0, 0);
        targetPos = new Vector3(100, 0, 0);
        LastPos = new Vector3(3000, 0, 0);
        rectTransform.anchoredPosition = firstPos;
        IsUsingSkillEffect = false;
    }

    public void Init(Sprite image, string text)
    {
        skill.sprite = image;
        tmp.text = text;

        rectTransform.anchoredPosition = firstPos;
        IsUsingSkillEffect = true;
        StartCoroutine(EffectCoroutine());
    }

    IEnumerator EffectCoroutine()
    {
        Vector3 transform = rectTransform.anchoredPosition;
        float time_moveZeroTime = 0;
        while (time_moveZeroTime < 1)
        {
            time_moveZeroTime = Mathf.Clamp(time_moveZeroTime + 0.025f, 0, 1);
            rectTransform.anchoredPosition = Vector3.Lerp(transform, Vector3.zero, Ease(time_moveZeroTime));
            yield return wait;
        }

        transform = rectTransform.anchoredPosition;
        time_moveZeroTime = 0;
        while (time_moveZeroTime < 1)
        {
            time_moveZeroTime = Mathf.Clamp(time_moveZeroTime + 0.05f, 0, 1);
            rectTransform.anchoredPosition = Vector3.Lerp(transform, targetPos, Ease(time_moveZeroTime));
            yield return wait;
        }

        transform = rectTransform.anchoredPosition;
        time_moveZeroTime = 0;
        while (time_moveZeroTime < 1)
        {
            time_moveZeroTime = Mathf.Clamp(time_moveZeroTime + 0.05f, 0, 1);
            rectTransform.anchoredPosition = Vector3.Lerp(transform, LastPos, LastEase(time_moveZeroTime));
            yield return wait;
        }

        IsUsingSkillEffect = false;
    }

    float Ease(float x)
    {
        return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
    }

    float LastEase(float x)
    {
        return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
    }
}
