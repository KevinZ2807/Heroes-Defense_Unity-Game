using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] string _param;
    [SerializeField] UIBarHandler _barHandler;
    [SerializeField] List<int> _listValue;

    private int _index;

    private void Start()
    {
        int value = 3;
        if (PlayerPrefs.HasKey(_param + "_value"))
        {
            value = PlayerPrefs.GetInt(_param + "_value");
        }
        SetValue(value);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(_param + "_value", _index);
        PlayerPrefs.Save();
    }

    public void SetValue(int value)
    {
        _index = value;
        _audioMixer.SetFloat(_param, _listValue[_index]);
        _barHandler.SetValue(value);
    }

    public void IncValue()
    {
        if (_index < _listValue.Count - 1)
        {
            _audioMixer.SetFloat(_param, _listValue[++_index]);
            _barHandler.IncValue();
        }
    }

    public void DecValue()
    {
        if (_index > 0)
        {
            _audioMixer.SetFloat(_param, _listValue[--_index]);
            _barHandler.DecValue();
        }
    }
}
