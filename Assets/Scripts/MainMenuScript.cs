using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject _content;

    void Awake()
    {
        CanvasController.OpenMainMenu += Open;
        _content.SetActive(false);
    }

    private void Open(bool flag)
    {
        _content.SetActive(flag);
        Time.timeScale = flag ? 0.0f : 1f;
        if (flag)
        {
            AudioController.Ins.PlayMenuSoundtrack();
        }
    }

    public void PlayGame()
    {
        GameManager.Ins.StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
