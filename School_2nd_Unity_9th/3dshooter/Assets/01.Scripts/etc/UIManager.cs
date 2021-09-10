using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;
    public Text remainBullet;
    public RectTransform bulletMarkPanel;

    public Sprite[] markerImages;

    public Gun playerGun;
    private List<Image> bulletMarkList = new List<Image>();

    private void Awake()
    {
        bulletMarkPanel.GetComponentsInChildren(bulletMarkList);

        bulletMarkList.RemoveAt(0);
        bulletMarkList.ForEach(x => x.gameObject.SetActive(false));

        playerGun.UpdateMaxBullet += value =>
        {
            for (int i = 0; i < value / 10; i++)
            {
                bulletMarkList[i].gameObject.SetActive(true);
            }
        };

        playerGun.UpdateBullet += value =>
        {
            remainBullet.text = value.ToString("00");

            int cnt = Mathf.FloorToInt(value / 10);
            for (int i = 0; i < bulletMarkList.Count; i++)
            {
                if (!bulletMarkList[i].gameObject.activeSelf)
                {
                    break;
                }

                bulletMarkList[i].sprite = i < cnt ? markerImages[0] : markerImages[1];
            }
        };
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}       
