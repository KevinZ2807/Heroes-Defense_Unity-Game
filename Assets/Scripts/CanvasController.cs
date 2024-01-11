using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static event Action<bool> OpenMainMenu = delegate { };
    public static event Action<bool> OpenSetting = delegate { };
    public static event Action<bool> OpenPauseMenu = delegate { };
    public static event Action<bool> OpenIngame = delegate { };
    public static event Action<bool> OpenEndgame = delegate { };
    public static event Action<bool> OpenTutorial = delegate { };

    public string _previouseCanvasName { get; private set; }

    private static event Action<bool> _currentEvent = delegate { };

    private bool _canPause = false;

    private void Start()
    {
        OpenMenuCanvas(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _canPause)
        {

            if (GameManager.Ins.IsPlaying)
            {
                OpenPauseCanvas(true);
            }
            else
            {
                OpenPauseCanvas(false);
            }
        }
    }

    public void OpenMenuCanvas(bool flag)
    {
        _currentEvent.Invoke(!flag);
        OpenMainMenu.Invoke(flag);
        _currentEvent = OpenMainMenu;
        _previouseCanvasName = "Menu";
        _canPause = false;
    }

    public void OpenTutorialCanvas(bool flag)
    {
        _currentEvent.Invoke(!flag);
        OpenTutorial.Invoke(flag);
        _currentEvent = OpenTutorial;
        _previouseCanvasName = "Tutorial";
    }

    public void OpenSettingCanvas(bool flag)
    {
        _currentEvent.Invoke(!flag);
        OpenSetting.Invoke(flag);
        _currentEvent = OpenSetting;
        _canPause = false;
    }

    public void CloseSetting()
    {
        switch (_previouseCanvasName)
        {
            case "Menu":
                _previouseCanvasName = "Setting";
                OpenMenuCanvas(true);
                break;
            case "Pause":
                _previouseCanvasName = "Setting";
                OpenPauseCanvas(true);
                break;
            case "Ingame":
                _previouseCanvasName = "Setting";
                OpenIngameCanvas(true);
                break;
        }
    }

    public void OpenPauseCanvas(bool flag)
    {
        if(_previouseCanvasName != "Setting")
        {
            GameManager.Ins.IsPlaying = !GameManager.Ins.IsPlaying;
        }
        _currentEvent.Invoke(!flag);
        OpenPauseMenu.Invoke(flag);
        _currentEvent = OpenPauseMenu;
        _previouseCanvasName = "Pause";
        _canPause = true;

        if (!flag)
        {
            OpenIngameCanvas(true);
        }
    }

    public void OpenIngameCanvas(bool flag)
    {
        _currentEvent.Invoke(!flag);
        OpenIngame.Invoke(flag);
        _currentEvent = OpenIngame;
        _previouseCanvasName = "Ingame";
        _canPause = true;
    }

    public void OpenEndgameCanvas(bool flag)
    {
        _currentEvent.Invoke(!flag);
        OpenEndgame.Invoke(flag);
        _currentEvent = OpenEndgame;
        _previouseCanvasName = "Endgame";
        _canPause = false;
    }
}
