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

        // �ڿ��� �ִ� ����
        nearByResourceAmount = Mathf.Clamp(nearByResourceAmount, 0, resGenDt.maxResAmount);
        // ��ġ �� �ڿ� ������ �׳� ����
        if(nearByResourceAmount == 0)
        {
            enabled = false;
        }
        else
        {
            // ���� �ӵ� ���� ����
            // �ڿ� ������ ����Ͽ� ����ӵ��� ����������
            // ������ �ƹ��� ������ ���Ƶ� �ְ� 0.5f �̻����� �������� ����.
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

            // ������ ƽ�� ��ŭ�� ä�������� �α� �ڵ�
            // print("Tick : " + buildingType.resGeneratorDt.resType.nameStr);

            ResourceManager.Instance.AddResource(resGenDt.resType, 1);
        }
    }
}
