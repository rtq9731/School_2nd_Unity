using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonUtill : MonoBehaviour
{
    readonly string saveFileName = "JsonUtillSaveFile.sav";

    [SerializeField]
    private string name = "ÄÚÄÚ³Ó";
    [SerializeField]
    private short level = 18;

    public Transform myTr;

    string filePath;
    private void Start()
    {
        filePath = Application.persistentDataPath + "/" + saveFileName;
    }
    void Update()
    {
        InputKey();
    }

    void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(filePath);
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(filePath);
            LoadData();
        }
    }

    void SaveData()
    {
        string jsonString = JsonUtility.ToJson(this);

        StreamWriter sw = new StreamWriter(filePath);

        sw.WriteLine(jsonString);
        sw.Close();
    }

    void LoadData()
    {
        if(File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonString = sr.ReadToEnd();
            sr.Close();

            JsonUtility.FromJsonOverwrite(jsonString, this);

            Debug.Log(jsonString);
        }
        else
        {
            Debug.LogWarning("We Have Not File");
        }
    }
}
