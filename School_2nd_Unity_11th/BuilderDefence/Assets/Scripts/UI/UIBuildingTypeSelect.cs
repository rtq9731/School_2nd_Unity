using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingTypeSelect : MonoBehaviour
{
    [SerializeField] private Transform selectedImgTrm = null;

    // 화살표 버튼 처리 관련 변수
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

        // 위치 적용
        float offsetAmount = 146f;
        arrowBtnTrm.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * idx, 0);

        // 이미지 적용
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

            // 생성 및 활성화
            Transform btnTrm = Instantiate(btnTmpTrm, transform);
            btnTrm.gameObject.SetActive(true);

            // 위치 적용
            btnTrm.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * idx, 0);

            // 이미지 적용
            btnTrm.Find("image").GetComponent<Image>().sprite = item.sprite;

            btnTrm.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuilderManager.Instance.SetActiveBuildingType(item);

                selectedImgTrm.position = btnTrm.position;
            });

            idx++;

            //마우스 이벤트 캡처 코드
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
