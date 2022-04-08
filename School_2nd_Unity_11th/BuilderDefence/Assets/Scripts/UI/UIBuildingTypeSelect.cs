using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingTypeSelect : MonoBehaviour
{
    [SerializeField] private Transform selectedImgTrm = null;

    // ȭ��ǥ ��ư ó�� ���� ����
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private BuildingGhost buildingGhost;

    private Transform arrowBtnTrm;

    private BuildingTypeListSO buildingTypeListSO;
    private Dictionary<BuildingTypeSO, Button> btnTypeButtonDict;

    [SerializeField] List<BuildingTypeSO> ignoreBuilidingTypeList; 

    private void Awake()
    {
        btnTypeButtonDict = new Dictionary<BuildingTypeSO, Button>();
        Transform btnTmpTrm = transform.Find("btnTamplate");
        btnTmpTrm.gameObject.SetActive(false);

        buildingTypeListSO = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        Debug.Log(buildingTypeListSO.btList.Count);

        int idx = 0;

        arrowBtnTrm = Instantiate(btnTmpTrm, transform);
        arrowBtnTrm.gameObject.SetActive(true);

        // ��ġ ����
        float offsetAmount = 146f;
        arrowBtnTrm.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * idx, 0);

        // �̹��� ����
        Transform imageTrm = arrowBtnTrm.Find("image");
        imageTrm.GetComponent<Image>().sprite = arrowSprite;
        imageTrm.GetComponent<RectTransform>().sizeDelta = new Vector2(-30, -30);

        Button imageButton = imageTrm.gameObject.AddComponent<Button>();
        imageButton.onClick.AddListener(() =>
        {
            BuilderManager.Instance.SetActiveBuildingType(null);

            selectedImgTrm.position = arrowBtnTrm.position;
        });

        idx++;

        foreach (var item in buildingTypeListSO.btList)
        {
            if (ignoreBuilidingTypeList.Contains(item)) continue;

            // ���� �� Ȱ��ȭ
            Transform btnTrm = Instantiate(btnTmpTrm, transform);
            btnTrm.gameObject.SetActive(true);

            // ��ġ ����
            btnTrm.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * idx, 0);

            // �̹��� ����
            btnTrm.Find("image").GetComponent<Image>().sprite = item.sprite;

            btnTrm.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuilderManager.Instance.SetActiveBuildingType(item);

                selectedImgTrm.position = btnTrm.position;
            });

            idx++;

            //���콺 �̺�Ʈ ĸó �ڵ�
            MouseEnterExitEvents mouseEnterExitEvents = btnTrm.GetComponent<MouseEnterExitEvents>();

            mouseEnterExitEvents.OnMouseEnter += () => UIToolTip.instance.ShowBulidingInfo(item.nameStr, item.buildResCostArr);
            mouseEnterExitEvents.OnMouseExit += () => UIToolTip.instance.Hide();

        }

    }

    private void Start()
    {
        selectedImgTrm.SetAsLastSibling();
    }
}
