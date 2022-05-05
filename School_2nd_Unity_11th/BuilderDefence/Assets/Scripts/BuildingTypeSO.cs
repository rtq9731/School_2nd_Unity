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

    public float minConstructRadius = 1; // �ǹ� �� �Ÿ���

    // ���� �ڽ�Ʈ �߰� ����

    public ResAmount[] buildResCostArr;

    public int healthAmountMax;
}