using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverHandler : MonoBehaviour
{
    public static bool isSelectedHero = false;
    [SerializeField] SpriteRenderer _hoverImage;
    [SerializeField] SpriteRenderer _heroImage;
    [SerializeField] Color _availableColor;
    [SerializeField] Color _unavailableColor;

    public void UpdateSelectedHero(BaseHero hero)
    {
        _heroImage.sprite = hero.sprite;
    }

    public void ShowAvailable(bool flag)
    {
        if (flag)
        {
            _hoverImage.color = _availableColor;
        }
        else
        {
            _hoverImage.color = _unavailableColor;
        }
    }
}
