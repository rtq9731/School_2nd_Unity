using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    // private BuildingTypeSO buildingType;

    private ResourceGeneratorData resGenDt;

    private float timer;
    private float timeMax;

    private void Awake()
    {
        timeMax = 1f;

        resGenDt = GetComponent<BuildingTypeHolder>().buildingType.resGeneratorDt;
        timeMax = resGenDt.timerMax;
    }

    private void Start()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, resGenDt.detectionRad);

        int nearByResourceAmount = 0;
        foreach (var item in colliders)
        {
            ResourceNode resourceNode = item.GetComponent<ResourceNode>();
            if(resourceNode != null)
            {
                if (resourceNode.resType == resGenDt.resType)
                {
                    nearByResourceAmount++;
                }
            }
        }

        // 자원수 최대 제한
        nearByResourceAmount = Mathf.Clamp(nearByResourceAmount, 0, resGenDt.maxResAmount);
        // 배치 후 자원 없으면 그냥 꺼짐
        if(nearByResourceAmount == 0)
        {
            enabled = false;
        }
        else
        {
            // 생산 속도 조절 공식
            // 자원 개수에 비례하여 생산속도가 빨라지도록
            // 하지만 아무리 갯수가 많아도 최고 0.5f 이상으로 빨라지지 않음.
            timeMax = resGenDt.timerMax / 2f + resGenDt.timerMax * (1 - (float)nearByResourceAmount / resGenDt.maxResAmount);
        }

        print(timeMax);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer += timeMax;

            // 실제로 틱당 얼만큼씩 채워지는지 로그 코드
            // print("Tick : " + buildingType.resGeneratorDt.resType.nameStr);

            ResourceManager.Instance.AddResource(resGenDt.resType, 1);
        }
    }
}
