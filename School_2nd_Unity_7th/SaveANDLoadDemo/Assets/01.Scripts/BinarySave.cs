using System.IO;    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinarySave : MonoBehaviour
{
    readonly string saveFileName = "BinarySaveFile.sav";

    private string name = "ÄÚÄÚ³Ó";
    private short level = 18;

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
        Debug.Log("SaveData!");

        FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
        BinaryWriter bw = new BinaryWriter(fs);

        bw.Write(name);
        bw.Write(level);

        fs.Close();
        bw.Close();
    }

    void LoadData()
    {
        FileStream fs = new FileStream(filePath, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);

        Debug.Log(br.ReadString());
        Debug.Log(br.ReadInt16());

        fs.Close();
        br.Close();
    }

}
