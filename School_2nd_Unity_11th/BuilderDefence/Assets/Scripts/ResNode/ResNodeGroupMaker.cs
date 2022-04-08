using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResNodeGroupMaker : MonoBehaviour
{
    [Header("리소스 노드 프리팹 링크")]
    [SerializeField] private GameObject[] _resNodePrefabs;

    enum eNodeType : int
    {
        Wood = 0,
        Stone,
        Gold
    }

    [Header("리소스 노드 생성 옵션")]
    [SerializeField] bool _bNodeTypeRandom = true;
    [SerializeField] eNodeType _nodeType;
    [SerializeField] [Range(1, 3)] int _nodeAmountMin = 1;
    [SerializeField] [Range(5, 7)] int _nodeAmountMax = 5;
    [SerializeField] [Range(1f, 5f)] float _nodeDist = 1f;

    private void Awake()
    {
        RandomMakeNodeGroup();
    }

    private void RandomMakeNodeGroup()
    {
        GameObject nodeObj = null;
        float randPosX, randPosY;

        if(_resNodePrefabs == null)
        {
            Debug.LogWarning("_resNodePrefabs is null!");
            return;
        }

        // 노드 타입 정하기
        if(_bNodeTypeRandom)
        {
            int nodeTypeMax = System.Enum.GetValues(typeof(eNodeType)).Length;
            _nodeType = (eNodeType)UnityEngine.Random.Range(0, nodeTypeMax);
        }

        // 노드 갯수 정하기
        int nodeLength = UnityEngine.Random.Range(_nodeAmountMin, _nodeAmountMax + 1);
        for (int i = 0; i < nodeLength; i++)
        {
            nodeObj = Instantiate(_resNodePrefabs[(int)_nodeType], transform);

            randPosX = transform.position.x + UnityEngine.Random.Range(-_nodeDist, _nodeDist);
            randPosY = transform.position.y + UnityEngine.Random.Range(-_nodeDist, _nodeDist);

            nodeObj.transform.SetPositionAndRotation(new Vector3(randPosX, randPosY, 0f), Quaternion.identity);
        }
    }
}
