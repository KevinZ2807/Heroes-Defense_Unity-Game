using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGraphicsHandler : MonoBehaviour
{
    public void ChangeQuality(int value)
    {
        QualitySettings.SetQualityLevel(value);
    }

    public void SetFullScreen(bool value)
    {
        Screen.fullScreen = value;
    }
}
