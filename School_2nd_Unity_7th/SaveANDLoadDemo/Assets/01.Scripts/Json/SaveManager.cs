using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

public class SaveManager : MonoBehaviour
{

    public GameObject enemyObj;
    public Enemy[] enemies;

    readonly string saveFileName = "JsonUtillSaveFile.sav";
    string filePath;

    public List<ISerializable> objToSaveList;

    Rijndael myRijndael;

    private void Awake()
    {
        // GetEnemyObjs();

        objToSaveList = new List<ISerializable>();
    }

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
        // 적 리스트 저장

        //JObject jSaveGame = new JObject();
        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    Enemy curEnemy = enemies[i];
        //    JObject jEnemy = curEnemy.Serialize();
        //    jSaveGame.Add(curEnemy.gameObject.name, jEnemy);
        //}

        JObject jSaveGame = new JObject();
        for (int i = 0; i < objToSaveList.Count; i++)
        {
            jSaveGame.Add(objToSaveList[i].GetJsonKey(), objToSaveList[i].Serialize());
        }

        StreamWriter sw = new StreamWriter(filePath + saveFileName);
        print("save to : " + filePath);
        sw.WriteLine(jSaveGame.ToString());
        sw.Close();

        print(jSaveGame.ToString());
    }

    void LoadData()
    {
        print("Load To :" + filePath + saveFileName);

        string fileStr = filePath + saveFileName;
        if (File.Exists(fileStr))
        {
            StreamReader sr = new StreamReader(fileStr);
            string jsonString = sr.ReadToEnd();
            sr.Close();

            //JObject jSaveGame = JObject.Parse(jsonString);
            //for (int i = 0; i < enemies.Length; i++)
            //{
            //    Enemy curEnemy = enemies[i];
            //    string enemyJsonString = jSaveGame[curEnemy.gameObject.name].ToString();
            //    curEnemy.DeSerialize(enemyJsonString);
            //}

            JObject jSaveGame = JObject.Parse(jsonString);
            for (int i = 0; i < objToSaveList.Count; i++)
            {
                string objJsonString = jSaveGame[objToSaveList[i].GetJsonKey()].ToString();
                objToSaveList[i].DeSerialize(objJsonString);
            }

        }
        else
        {
            print("세이브 파일 없음!");
        }

    }

    private void GetEnemyObjs()
    {
        if(enemyObj != null)
        {
            int enemyCount = enemyObj.transform.childCount;
            enemies = new Enemy[enemyCount];

            for (int i = 0; i < enemyCount; i++)
            {
                enemies[i] = enemyObj.transform.GetChild(i).GetComponent<Enemy>();
            }

        }

        print($"적 갯수 : { enemies.Length }");

    }

    byte[] Encrypt(string message, byte[] Key, byte[] IV)
    {
        AesManaged aes = new AesManaged();
        ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);

        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        StreamWriter sw = new StreamWriter(cryptoStream);

        sw.WriteLine(message);

        sw.Close();
        cryptoStream.Close();
        memoryStream.Close();
    }


    string Decrypt(byte[] message, byte[] Key, byte[] IV)
    {
        AesManaged aes = new AesManaged();
        ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);

        MemoryStream memoryStream = new MemoryStream(message);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(cryptoStream);

        string decryptedMessage = sr.ReadToEnd();

        memoryStream.Close();
        cryptoStream.Close();
        sr.Close();

        return decryptedMessage;

    }
}
