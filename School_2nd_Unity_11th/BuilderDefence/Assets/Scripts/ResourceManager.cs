using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<ResourceTypeSO, int> resourceAmountDic;

    [SerializeField] private List<ResAmount> startingResAmount = new List<ResAmount>();

    public System.Action<ResourceTypeSO, int> onResourceAmountChanged = (x, y) => { };

    private void Awake()
    {
        Instance = this;

        resourceAmountDic = new Dictionary<ResourceTypeSO, int>();

        ResourceTypeListSO resTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        foreach (ResourceTypeSO resType in resTypeList.resList)
        {
            resourceAmountDic[resType] = 0;
        }

        foreach (var item in startingResAmount)
        {
            AddResource(item.resourceType, item.amount);
        }

        //TestLogResAmountDic();
    }

    private void Update()
    {
        // 임시키 세팅으로 리소스 추가 코드
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    ResourceTypeListSO resTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        //    AddResource(resTypeList.resList[0], 2);

        //    TestLogResAmountDic();
        //}
    }

    public void AddResource(ResourceTypeSO resType, int amount)
    {
        resourceAmountDic[resType] += amount;
        onResourceAmountChanged?.Invoke(resType, resourceAmountDic[resType]);

        //TestLogResAmountDic();
    }

    public int GetResourceAmount(ResourceTypeSO resType)
    {
        return resourceAmountDic[resType];
    }

    public void SetResourceAmount(ResourceTypeSO resType, int value)
    {
        resourceAmountDic[resType] = value;
        onResourceAmountChanged?.Invoke(resType, value);
    }

    void TestLogResAmountDic()
    {
        foreach(ResourceTypeSO resType in resourceAmountDic.Keys)
        {
            print(string.Format("{0}:{1}", resType.nameStr, resourceAmountDic[resType]));
        }
    }
}
