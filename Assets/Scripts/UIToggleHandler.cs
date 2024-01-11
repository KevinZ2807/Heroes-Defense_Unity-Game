using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleHandler : MonoBehaviour
{
    [SerializeField] Sprite _OnSprite;
    [SerializeField] Sprite _OffSprite;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetValue(bool value)
    {
        if (value)
        {
            _image.sprite = _OnSprite;
        }
        else
        {
            _image.sprite = _OffSprite;
        }
    }
}