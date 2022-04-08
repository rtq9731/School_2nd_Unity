using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    SpriteRenderer _sr;
    // �ǹ��� �����ϸ� ���� ��Ʈ �̹����� �������� �Ѵ� / ���� Ÿ�Կ� ���� �̹��� ����
    // ȭ��ǥ �����ϸ� ���ܾ��Ѵ�.

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        BuilderManager.Instance.onBuildingModeChange += x => ShowUI(x);
    }

    public void ShowUI(Sprite sprite)
    {
        if (sprite == null)
        {
            gameObject.SetActive(false);
        }

        gameObject.SetActive(true);
        _sr.sprite = sprite;
    }

    private void Update()
    {
        if(gameObject.activeSelf)
        {
            transform.position = UtilClass.GetMouseWorldPos();
        }
    }
}
