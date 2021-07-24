using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class JsonDemo : MonoBehaviour
{
    readonly string saveFileName = "JsonObjSaveFile.sav";

    private string name = "���ڳ�";
    private short level = 18;

    [SerializeField]
    string[] friends;

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
        JObject jObj = new JObject();
        jObj.Add("componentName", GetType().ToString());

        JObject jDataObject = new JObject();
        jObj.Add("data", jDataObject);

        jDataObject.Add("name", name);
        jDataObject.Add("level", level);

        JArray jFriends = JArray.FromObject(friends);
        jDataObject.Add("friends", jFriends);

        //������ ����
        StreamWriter sw = new StreamWriter(filePath);
        sw.WriteLine(jObj.ToString());
        sw.Close();
    }

    void LoadData()
    {
        StreamReader sr = new StreamReader(filePath);
        string jsonString = sr.ReadToEnd();
        sr.Close();

        Debug.Log(jsonString);

        // �о���� �ڷ��� ���ؼ�
        JObject jObj = JObject.Parse(jsonString);

        name = jObj["data"]["name"].Value<string>();
        level = jObj["data"]["level"].Value<short>();
        friends = jObj["data"]["friends"].ToObject<string[]>();
    }
}
