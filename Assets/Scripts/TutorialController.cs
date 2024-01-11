using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] CanvasController _canvasController;
    [SerializeField] GameObject _content;

    void Awake()
    {
        CanvasController.OpenTutorial += Open;
        _content.SetActive(false);
    }

    private void Open(bool flag)
    {
        _content.SetActive(flag);
    }
}
