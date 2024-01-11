using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    private static Rect rect = new Rect(Vector2.zero, Vector2.one);
    public float speed;

    [SerializeField]
    private RawImage _img;
    [SerializeField]
    private float _x, _y;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        _img.enabled = false;
        _img.uvRect = rect;
        _img.enabled = true;
    }

    private void OnDisable()
    {
        rect = _img.uvRect;
    }

    // Update is called once per frame
    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, 0) * speed, _img.uvRect.size);
    }
}
