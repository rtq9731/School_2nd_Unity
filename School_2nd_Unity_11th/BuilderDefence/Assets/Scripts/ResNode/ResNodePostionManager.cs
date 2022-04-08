using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResNodePostionManager : MonoBehaviour
{
    [Header("리소스 노드 그룹 프리팹 링크")]
    [SerializeField] private ResNodeGroupMaker _resNodeGroupPrefab;

    [Header("리소스 노드 배치 옵션")]

    [SerializeField] [Range(5, 7)] int _nodeGroupAmountMin = 5;
    [SerializeField] [Range(9, 12)] int _nodeGroupAmountMax = 9;
    [SerializeField] [Range(10f, 20f)] float _nodeRandomDist = 20f;

    float _checkMinDistance = 3f;

    public static Dictionary<string, GameObject> _resNodeObjDict;
    string _NodeKeyName;

    private void Awake()
    {
        _resNodeObjDict = new Dictionary<string, GameObject>();

        _NodeKeyName = this.gameObject.name;
    }

    private void Start()
    {
        SetResNodeGroup();
    }

    private void SetResNodeGroup()
    {
        GameObject nodeGroupObj = null;
        float randPosX, randPosY;

        if (_resNodeGroupPrefab == null)
        {
            Debug.LogWarning("_resNodePrefabs is null!");
            return;
        }

        // 노드 그룹 갯수 랜덤 세팅
        int nodeLength = UnityEngine.Random.Range(_nodeGroupAmountMin, _nodeGroupAmountMax + 1);

        string nodeObjKeyStr = "";
        _resNodeObjDict.Clear();

        for (int i = 0; i < nodeLength; i++)
        {
            nodeGroupObj = Instantiate(_resNodeGroupPrefab.gameObject, transform);
            nodeGroupObj.gameObject.SetActive(false);

            nodeObjKeyStr = $"{_NodeKeyName}{i}";
            _resNodeObjDict.Add(nodeObjKeyStr, nodeGroupObj);
        }

        print("리소스 노드 그룹 수" + _resNodeObjDict.Count);

        for (int i = 0; i < _resNodeObjDict.Count; i++)
        {
            bool bChanged = false;
            while(!bChanged)
            {
                randPosX = transform.position.x + UnityEngine.Random.Range(-_nodeRandomDist, _nodeRandomDist);
                randPosY = transform.position.y + UnityEngine.Random.Range(-_nodeRandomDist, _nodeRandomDist);

                nodeObjKeyStr = $"{_NodeKeyName}{i}";
                _resNodeObjDict[nodeObjKeyStr].transform.position = new Vector3(randPosX, randPosY, 0);

                //중심부와 너무 가까우면 새로 위치
                if((_resNodeObjDict[nodeObjKeyStr].transform.position - this.transform.position).magnitude < _checkMinDistance)
                {
                    continue;
                }
                else
                {
                    bChanged = true;
                    _resNodeObjDict[nodeObjKeyStr].SetActive(true);
                }
            }
        }
    }
}
