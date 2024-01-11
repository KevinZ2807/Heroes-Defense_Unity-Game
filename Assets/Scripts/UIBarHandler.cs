using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarHandler : MonoBehaviour
{
    [SerializeField] List<Sprite> _listSprite;

    private int _index;
    private Image _image;

    private void Awake()
    {
        _index = 0;
        _image = GetComponent<Image>();
    }

    public void SetValue(int value)
    {
        _index = value;
        _image.sprite = _listSprite[_index];
    }

    public void IncValue()
    {
        if(_index < _listSprite.Count - 1)
        {
            _image.sprite = _listSprite[++_index];
        }
    }

    public void DecValue()
    {
        if (_index > 0)
        {
            _image.sprite = _listSprite[--_index];
        }
    }
}
