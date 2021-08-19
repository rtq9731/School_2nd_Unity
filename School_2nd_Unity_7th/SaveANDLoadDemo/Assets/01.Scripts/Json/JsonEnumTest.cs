using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonEnumTest : MonoBehaviour
{

    [System.Serializable]
    public enum State
    {
        rightUpper,
        rightLower,
        leftUpper,
        leftLower
    }

    public State state = State.rightUpper;

    readonly string saveFileName = "JsonEnumSaveFile.sav";
    string filePath;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/" + saveFileName;
        Debug.Log(filePath);
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
        if (File.Exists(filePath))
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
