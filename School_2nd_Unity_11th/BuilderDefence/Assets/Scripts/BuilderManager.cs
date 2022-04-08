using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class BuilderManager : MonoBehaviour
{
    public static BuilderManager Instance { get; private set; }

    private Camera mainCam;

    [SerializeField]
    private Transform mouseVisualTrm;
    [SerializeField]
    private Transform woodPrefabTrm;

    // ���� ���ҽ� �ε�
    private BuildingTypeSO activeBuildingType = null;
    private BuildingTypeListSO buildingTypeList;
    public Action<Sprite> onBuildingModeChange;

    [SerializeField] Building ccBuilding = null;

    private void Awake()
    {
        Instance = this;
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }

    void Start()
    {
        mainCam = Camera.main;
    }

    Vector3 curMousePos = Vector3.zero;

    private void Update()
    {
        // ���콺 ���ʹ�ư Ŭ���ϸ� ��Ȯ�� ����
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            curMousePos = UtilClass.GetMouseWorldPos();
            if (activeBuildingType != null)
            {
                if(CanBuy(activeBuildingType))
                {
                    if (CanSpawnBuilding(activeBuildingType, curMousePos, out string errorMsg))
                    {
                        MakeNewBuilding(activeBuildingType);
                    }
                    else
                    {
                        UIToolTip.instance.Show(errorMsg, 1f);
                    }
                }
                else
                {
                    UIToolTip.instance.ShowBulidingInfo("�ڿ��� �����մϴ�.", activeBuildingType.buildResCostArr, 1f);
                }
            }
            /// mouseVisualTrm.position = GetMouseWorldPos();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Enemy.Create(UtilClass.GetMouseWorldPos() + UtilClass.GetRandomDir());
        }

        //// �ӽ�Ű �������� ��Ȯ�⸦ �ٲ㺸��
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    activeBuildingType = buildingTypeList.btList[0];
        //}

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    activeBuildingType = buildingTypeList.btList[1];
        //}

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    activeBuildingType = buildingTypeList.btList[2];
        //}
    }

    private bool CanBuy(BuildingTypeSO buildingType)
    {
        ResAmount buyResAmount = null;
        for (int i = 0; i < buildingType.buildResCostArr.Length; i++)
        {
            buyResAmount = buildingType.buildResCostArr[i];
            if (buyResAmount.amount > ResourceManager.Instance.GetResourceAmount(buyResAmount.resourceType))
            {
                return false;
            }
        }
        return true;
    }

    public void MakeNewBuilding(BuildingTypeSO buildingType)
    {
        ResAmount buyResAmount = null;
        for (int i = 0; i < buildingType.buildResCostArr.Length; i++)
        {
            buyResAmount = buildingType.buildResCostArr[i];
            int curResoruce = ResourceManager.Instance.GetResourceAmount(buyResAmount.resourceType);
            ResourceManager.Instance.SetResourceAmount(buyResAmount.resourceType, curResoruce - buyResAmount.amount);
        }

        Instantiate(activeBuildingType.prefab, curMousePos, Quaternion.identity);
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        if (buildingType != null)
        {
            onBuildingModeChange?.Invoke(buildingType.sprite);
        }
        else
        {
            onBuildingModeChange?.Invoke(null);
        }
    }


    // �ǹ� ��ġ�� ���� ��, �ٸ� ������Ʈ ( �ڿ�, �ǹ� ) �� ��ġ�� ���� ��
    // ���� ������ �ǹ��� �������� �ּ����� �Ÿ��� �����ؼ� ������ ��
    // Ŀ�ǵ弾�͸� �������� �ʹ� �ָ� �������� �ǹ��� ������ �� ��

    public bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 mousePos, out string errorMsg)
    {
        // �ٸ� ������Ʈ�� ���� �ϴ��� �˻�
        BoxCollider2D boxCol2D = buildingType.prefab.GetComponent<BoxCollider2D>();
        Collider2D[] colliders = Physics2D.OverlapBoxAll(mousePos + (Vector3)boxCol2D.offset, (Vector3)boxCol2D.size, 0);


        bool bAreaClear = colliders.Length == 0;
        if(!bAreaClear)
        {
            errorMsg = "�̰����� ���� �� �����ϴ�!";
            return false; // ù° ���� ( �ǹ��� ��ħ )
        }

        colliders = Physics2D.OverlapCircleAll(mousePos, buildingType.minConstructRadius);
        foreach (var item in colliders)
        {
            BuildingTypeHolder buildingTypeHolder = item.GetComponent<BuildingTypeHolder>();
            if ( buildingTypeHolder != null )
            {
                if (buildingTypeHolder.buildingType == buildingType)
                {
                    errorMsg = "���� Ÿ���� �ǹ��� ��ó�� �ֽ��ϴ�!";
                    return false; // ��° ���� ( ���� �ǹ��� �ֺ��� �ʹ� ������ ���� )
                }
            }
        }

        float maxConstructRadius = 50f;
        colliders = Physics2D.OverlapCircleAll(mousePos, maxConstructRadius);
        foreach (var item in colliders)
        {
            BuildingTypeHolder buildingTypeHolder = item.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                errorMsg = "";
                return true; // ���� ����
            }
        }
        errorMsg = "�Ǽ� �� �� �ִ� ������ ������ϴ�!";
        return false; // 3��° ���� ( Ŀ�ǵ� ���Ͷ� �ʹ� �� )
    }

    public Building GetCommandCenter()
    {
        return ccBuilding;
    }
}
