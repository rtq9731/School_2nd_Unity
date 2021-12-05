using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingTypeSelect : MonoBehaviour
{
    private BuildingTypeListSO buildingTypeListSO;

    private Dictionary<BuildingTypeSO, Button> btnTypeButtonDict;

    private void Awake()
    {
        btnTypeButtonDict = new Dictionary<BuildingTypeSO, Button>();
        Transform btnTmpTrm = transform.Find("btnTamplate");
        btnTmpTrm.gameObject.SetActive(false);

        buildingTypeListSO = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        Debug.Log(buildingTypeListSO.btList.Count);

        int idx = 0;


        foreach (var item in buildingTypeListSO.btList)
        {
            // ���� �� Ȱ��ȭ
            Transform btnTrm = Instantiate(btnTmpTrm, transform);
            btnTrm.gameObject.SetActive(true);

            // ��ġ ����
            float offsetAmount = 146f;
            btnTrm.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * idx, 0);

            // �̹��� ����
            btnTrm.Find("image").GetComponent<Image>().sprite = item.sprite;

            btnTrm.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuilderManager.Instance.SetActiveBuildingType(item);
            });

            idx++;
        }
    }
}
