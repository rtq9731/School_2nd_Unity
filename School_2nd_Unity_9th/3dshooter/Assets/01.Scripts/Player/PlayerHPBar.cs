using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField] Image fillImage;

    public void SetFill(float current, float max)
    {
        fillImage.fillAmount = Mathf.Clamp(current / max, 0, 1);
    }
}
