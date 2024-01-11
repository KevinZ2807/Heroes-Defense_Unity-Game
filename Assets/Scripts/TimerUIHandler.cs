using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUIHandler : MonoBehaviour
{
    [SerializeField] List<Color> _colors;
    [SerializeField] List<float> _percents;
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _timeText;
    [SerializeField] Slider _timeLine;
    [SerializeField] Image _timeLineFill;

    private float _timeTarget;
    private float _timer;
    private bool _count;

    private void OnEnable()
    {
        EnemySpawner.SetCountEvent += SetCount;
    }

    private void OnDisable()
    {
        EnemySpawner.SetCountEvent -= SetCount;
    }

    private void Update()
    {
        if(_timer >= 0)
        {
            _timer += _count ? Time.deltaTime : -Time.deltaTime;
        }
        else
        {
            _timer = 0;
        }

        UpdateTimeUI();
    }

    private void SetCount(float count, bool isCount, int wave)
    {
        _timeTarget = count;
        _timer = isCount ? 0 : _timeTarget;
        _count = isCount;
        if (isCount)
        {
            _titleText.SetText("WAVE: " + wave);
        }
        else
        {
            _titleText.SetText("SET UP!");
        }
        UpdateTimeUI();
    }

    private void UpdateTimeUI()
    {
        _timeLine.value = _timer / _timeTarget;
        _timeText.SetText(ConvertTime(Mathf.Floor(_timer)));
        if (_count)
        {
            float temp = _timer / _timeTarget;
            for (int i = 0; i<_percents.Count; i++)
            {
                temp -= _percents[i];
                if (temp <= 0)
                {
                    _timeLineFill.color = _colors[i];
                    if (i == _percents.Count - 1)
                    {
                        Time.timeScale = 2;
                    }
                    else if (i == _percents.Count - 2)
                    {
                        Time.timeScale = 1.5f;
                    }
                    else
                    {
                        Time.timeScale = 1f;
                    }
                    break;
                }
            }
        }
        else
        {
            Time.timeScale = 1f;
            float temp = _timer / _timeTarget;
            for (int i = 0; i < _percents.Count; i++)
            {
                temp += _percents[i];
                if (temp >= 1)
                {
                    _timeLineFill.color = _colors[i];
                    break;
                }
            }
        }
    }

    private string ConvertTime(float time)
    {
        int tempTime = (int)time;
        return tempTime / 60 + ":" + tempTime % 60;
    }
}
