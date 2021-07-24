using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryFomatterSave : MonoBehaviour
{
    readonly string saveFileName = "BinaryFormatterSaveFile.sav";

    private string name = "内内秤";
    private short level = 18;

    [System.Serializable]
    private class DataContainer
    {
        public string m_Name = "内内秤";
        public short m_Level = 18;

        public DataContainer(string name, short level)
        {
            m_Name = name;
            m_Level = level;
        }
    }

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
        DataContainer data = new DataContainer(name, level);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);

        bf.Serialize(fs, data);
        fs.Close();
    }

    void LoadData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filePath, FileMode.Open);

        DataContainer data = bf.Deserialize(fs) as DataContainer;
        Debug.Log($"Name : {data.m_Name}");
        Debug.Log($"Level : {data.m_Level}");
    }
}
