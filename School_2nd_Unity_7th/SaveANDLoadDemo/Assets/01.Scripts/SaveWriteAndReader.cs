using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveWriteAndReader : MonoBehaviour
{
    readonly string saveFileName = "SaveFile.sav";

    private string name = "ÄÚÄÚ³Ó";
    private short level = 18;

    string filePath;
    private void Start()
    {
        filePath = Application.persistentDataPath + "/" + saveFileName;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveFile();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFile();
        }
    }

    void SaveFile()
    {
        Debug.Log(filePath);

        StreamWriter sw = new StreamWriter(filePath);
        sw.WriteLine(name);
        sw.WriteLine(level);

        sw.Close();
    }

    void LoadFile()
    {
        Debug.Log(filePath);

        StreamReader sr = new StreamReader(filePath);

        Debug.Log(sr.ReadLine());
        Debug.Log(sr.ReadLine());

        sr.Close();
    }
}
