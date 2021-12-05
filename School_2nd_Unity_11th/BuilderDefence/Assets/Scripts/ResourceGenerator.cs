using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private BuildingTypeSO buildingType;

    private float timer;
    private float timeMax;

    private void Awake()
    {
        timeMax = 1f;

        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        timeMax = buildingType.resGeneratorDt.timerMax;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer += timeMax;

            // ������ ƽ�� ��ŭ�� ä�������� �α� �ڵ�
            print("Tick : " + buildingType.resGeneratorDt.resType.nameStr);

            ResourceManager.Instance.AddResource(buildingType.resGeneratorDt.resType, 1);
        }
    }
}
