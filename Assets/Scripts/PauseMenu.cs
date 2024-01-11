using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _content;

    void Awake()
    {
        CanvasController.OpenPauseMenu += Open;
        _content.SetActive(false);
    }

    public void Open(bool flag)
    {
        _content.SetActive(flag);
        if (flag)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    private void Resume()
    {
        Time.timeScale = 1.0f;
    }
    private void Pause()
    {
        Time.timeScale= 0f;
    }
}
