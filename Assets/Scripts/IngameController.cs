using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameController : MonoBehaviour
{
    [SerializeField] GameObject _content;

    void Awake()
    {
        CanvasController.OpenIngame += Open;
        _content.SetActive(false);
    }

    private void Open(bool flag)
    {
        _content.SetActive(flag);
    }
}
