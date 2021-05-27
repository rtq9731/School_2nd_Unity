using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICvsTopBar : MonoBehaviour
{
    [SerializeField] Button btnPopup;
    [SerializeField] GameObject popupObj;

    private void Awake()
    {
        if(btnPopup != null)
        {
            btnPopup.onClick.AddListener(() => popupObj.SetActive(true));
        }
    }
}
