using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResource : MonoBehaviour
{
    private ResourceTypeListSO resTypeList;

    private Dictionary<ResourceTypeSO, Transform> resTypeTrmDict;

    private void Awake()
    {
        resTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        resTypeTrmDict = new Dictionary<ResourceTypeSO, Transform>();

        Transform resTmpTrm = transform.Find("resourceTemplate");
        resTmpTrm.gameObject.SetActive(false);

        int idx = 0;

        foreach (var item in resTypeList.resList)
        {
            // 생성 및 활성화
            Transform resTrn = Instantiate(resTmpTrm, transform);
            resTrn.gameObject.SetActive(true);

            // 위치 적용
            float offsetAmount = -160f;
            resTrn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * idx, 0);

            // 이미지 적용
            resTrn.Find("image").GetComponent<Image>().sprite = item.sprite;

            resTypeTrmDict[item] = resTrn;

            idx++;
        }
    }

    private void Start()
    {
        foreach (var item in resTypeTrmDict)
        {
            Transform thisitemTrn = item.Value;
            ResourceTypeSO thisitemSO = item.Key;

            ResourceManager.Instance.onResourceAmountChanged += (x, y) =>
            {
                if (thisitemSO == x)
                {
                    thisitemTrn.Find("text").GetComponent<TextMeshProUGUI>().SetText($"{y}");
                }
            };
        }
    }
}
