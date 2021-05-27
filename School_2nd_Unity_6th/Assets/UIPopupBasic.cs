using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIPopupBasic : MonoBehaviour
{
    [SerializeField] Transform panelTrm;
    [SerializeField] Transform tapToCloseTrm;
    [SerializeField] Button btnBlank;

    Sequence seq1, seq2;

    private void Awake()
    {
        if(btnBlank != null)
        {
            btnBlank.onClick.AddListener(() =>
            {
                CallCloseFunc();
            });
        }
    }

    public void Init()
    {
        if(btnBlank != null)
        {
            btnBlank.interactable = false;
        }

        if(tapToCloseTrm != null)
        {
            tapToCloseTrm.gameObject.SetActive(false);
        }

        seq1.Kill();
        seq2.Kill();

        panelTrm.localScale = Vector3.zero;
        panelTrm.gameObject.SetActive(true);

        seq1 = DOTween.Sequence();
        seq1.Append(panelTrm.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f));
        seq1.Append(panelTrm.DOScale(new Vector3(1, 1, 1), 0.2f));
        seq1.AppendCallback(() =>
        {
            if(tapToCloseTrm != null)
            {
                tapToCloseTrm.localScale = Vector3.zero;
                tapToCloseTrm.gameObject.SetActive(true);

                seq2 = DOTween.Sequence();
                seq2.Append(tapToCloseTrm.DOScale(Vector3.one, 1.5f));
                seq2.AppendCallback(() =>
                {
                    btnBlank.interactable = true;
                });
            }
        });

    }

    private void CallCloseFunc()
    {
        RemovePanel();
    }

    void RemovePanel()
    {
        panelTrm.DOScale(Vector3.zero, 1f).OnComplete(() => panelTrm.gameObject.SetActive(false));

    }
}
