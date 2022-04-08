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

    // 빌딩 리소스 로딩
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
        // 마우스 왼쪽버튼 클릭하면 수확기 생성
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
                    UIToolTip.instance.ShowBulidingInfo("자원이 부족합니다.", activeBuildingType.buildResCostArr, 1f);
                }
            }
            /// mouseVisualTrm.position = GetMouseWorldPos();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Enemy.Create(UtilClass.GetMouseWorldPos() + UtilClass.GetRandomDir());
        }

        //// 임시키 세팅으로 수확기를 바꿔보자
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


    // 건물 배치가 있을 때, 다른 오브젝트 ( 자원, 건물 ) 와 겹치지 않을 것
    // 같은 종류의 건물을 지을때는 최소한의 거리를 유지해서 짓게할 것
    // 커맨드센터를 기준으로 너무 멀리 떨어지면 건물을 못짓게 할 것

    public bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 mousePos, out string errorMsg)
    {
        // 다른 오브젝트가 존재 하는지 검사
        BoxCollider2D boxCol2D = buildingType.prefab.GetComponent<BoxCollider2D>();
        Collider2D[] colliders = Physics2D.OverlapBoxAll(mousePos + (Vector3)boxCol2D.offset, (Vector3)boxCol2D.size, 0);


        bool bAreaClear = colliders.Length == 0;
        if(!bAreaClear)
        {
            errorMsg = "이곳에는 지을 수 없습니다!";
            return false; // 첫째 조건 ( 건물이 겹침 )
        }

        colliders = Physics2D.OverlapCircleAll(mousePos, buildingType.minConstructRadius);
        foreach (var item in colliders)
        {
            BuildingTypeHolder buildingTypeHolder = item.GetComponent<BuildingTypeHolder>();
            if ( buildingTypeHolder != null )
            {
                if (buildingTypeHolder.buildingType == buildingType)
                {
                    errorMsg = "같은 타입의 건물이 근처에 있습니다!";
                    return false; // 둘째 조건 ( 같은 건물이 주변에 너무 가까이 있음 )
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
                return true; // 조건 만족
            }
        }
        errorMsg = "건설 할 수 있는 범위를 벗어났습니다!";
        return false; // 3번째 조건 ( 커맨드 센터랑 너무 멈 )
    }

    public Building GetCommandCenter()
    {
        return ccBuilding;
    }
}
