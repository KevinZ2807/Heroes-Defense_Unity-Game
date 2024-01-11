using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndgameController : MonoBehaviour
{
    [SerializeField] GameObject _content;
    [SerializeField] TextMeshProUGUI _title;
    [SerializeField] TextMeshProUGUI _continueText;

    void Awake()
    {
        CanvasController.OpenEndgame += Open;
        _content.SetActive(false);
    }

    private void Open(bool flag)
    {
        if (GameManager.Ins.IsWin)
        {
            _title.SetText("YOU WIN");
            _title.color = Color.yellow;
            _continueText.SetText("NEXT LEVEL");
        }
        else
        {
            _title.SetText("YOU LOSE");
            _title.color = Color.red;
            _continueText.SetText("RETRY");
        }

        _content.SetActive(flag);
        Time.timeScale = flag ? 0.0f : 1.0f;
    }

    public void ResetLevel()
    {
        GameManager.Ins.ResetLevel();
    }
}
