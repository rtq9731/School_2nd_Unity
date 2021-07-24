using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] int hp;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            PlayerPrefs.SetInt("CurHP", hp);
            Debug.Log("����! ����.. ���带 �ߴ�!");
            PlayerPrefs.Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("CurHP : " + PlayerPrefs.GetInt("CurHP"));
        }
    }
}
