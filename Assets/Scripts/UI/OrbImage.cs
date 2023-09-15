using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbImage : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    [SerializeField] Image image;
    public void SetSprite(int index)
    {
        image.sprite = sprites[index];
    }
}
