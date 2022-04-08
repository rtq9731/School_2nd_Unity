using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    SpriteRenderer _sr;
    // 건물을 선택하면 빌딩 고스트 이미지가 보여져야 한다 / 빌딩 타입에 따라서 이미지 변경
    // 화살표 선택하면 숨겨야한다.

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
