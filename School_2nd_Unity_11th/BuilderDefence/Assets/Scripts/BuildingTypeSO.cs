using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string nameStr;
    public Transform prefab;

    public ResourceGeneratorData resGeneratorDt;

    public Sprite sprite;

    public float minConstructRadius = 1; // 건물 간 거리값

    // 빌딩 코스트 추가 변수

    public ResAmount[] buildResCostArr;

    public int healthAmountMax;
}
