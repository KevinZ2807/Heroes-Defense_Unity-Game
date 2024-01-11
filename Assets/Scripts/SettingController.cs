using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : MonoBehaviour
{
    [SerializeField] CanvasController _canvasController;
    [SerializeField] GameObject _content;
    [SerializeField] GameObject _menuBG;
    [SerializeField] GameObject _ingameBG;

    void Awake()
    {
        CanvasController.OpenSetting += Open;
        _content.SetActive(false);
    }

    private void Open(bool flag)
    {
        if(_canvasController._previouseCanvasName == "Menu")
        {
            _menuBG.SetActive(true);
            _ingameBG.SetActive(false);
        }
        else
        {
            _menuBG.SetActive(false);
            _ingameBG.SetActive(true);
        }

        _content.SetActive(flag);
        Time.timeScale = flag ? 0.0f : 1.0f;
    }
}
