using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public Image fillImage;

    protected void UpdateFillImage(float current, float max)
    {
        fillImage.fillAmount = current/max;
    }
}
