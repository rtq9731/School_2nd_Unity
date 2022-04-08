using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIToolTip : MonoBehaviour
{
    public static UIToolTip instance
    {
        get;
        private set;
    }

    [SerializeField] private RectTransform cvsRectTrm;

    private RectTransform myRectTrm;
    private RectTransform backGroundRectTrm;

    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        instance = this;

        myRectTrm = this.GetComponent<RectTransform>();
        textMeshPro = this.GetComponentInChildren<TextMeshProUGUI>();
        backGroundRectTrm = transform.Find("Background").GetComponent<RectTransform>();

        Hide();
    }

    private void SetText(string toolTipText)
    {
        textMeshPro.SetText(toolTipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(8, 8);

        backGroundRectTrm.sizeDelta = textSize + padding;
    }

    private Vector3 HandleFollowMouse()
    {
        Vector3 anchoredPos = Input.mousePosition / cvsRectTrm.localScale.x;
        if (anchoredPos.x + backGroundRectTrm.rect.width > cvsRectTrm.rect.width)
        {
            anchoredPos.x = cvsRectTrm.rect.width - backGroundRectTrm.rect.width;
        }

        if (anchoredPos.y + backGroundRectTrm.rect.height > cvsRectTrm.rect.height)
        {
            anchoredPos.y = cvsRectTrm.rect.height - backGroundRectTrm.rect.height;
        }
        return anchoredPos;
    }

    public void Show(string toolTipText, float disappearTime)
    {
        myRectTrm.anchoredPosition = HandleFollowMouse();
        gameObject.SetActive(true);
        SetText(toolTipText);
        Invoke("Hide", disappearTime);
    }
    public void Show(string toolTipText)
    {
        myRectTrm.anchoredPosition = HandleFollowMouse();
        gameObject.SetActive(true);
        SetText(toolTipText);
    }

    public void ShowBulidingInfo(string toolTipText, ResAmount[] amountArr, float disappearTime)
    {
        myRectTrm.anchoredPosition = HandleFollowMouse();
        gameObject.SetActive(true);
        SetText(toolTipText);
        Invoke("Hide", disappearTime);
    }
    public void ShowBulidingInfo(string toolTipText, ResAmount[] amountArr)
    {
        myRectTrm.anchoredPosition = HandleFollowMouse();
        gameObject.SetActive(true);
        SetText(toolTipText);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
