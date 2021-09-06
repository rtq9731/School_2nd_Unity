using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public Transform playerTr;

    [Header("Enemy create info")]
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float createCool = 5f;
    [SerializeField] int maxEnemy = 5;
    public bool isGameOver = false;

    private int enemyCount = 0;
    private List<EnemyHealth> enemyList = new List<EnemyHealth>();
    private WaitForSeconds wsSpawn;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        wsSpawn = new WaitForSeconds(createCool);
        for (int i = 0; i < maxEnemy; i++)
        {
            GameObject e = CreateEnemy();

            e.SetActive(false);
            EnemyHealth eh = e.GetComponent<EnemyHealth>();

            enemyList.Add(eh);
        }

    }

    private GameObject CreateEnemy()
    {
        return Instantiate(enemyPrefab, transform.position, Quaternion.identity, transform);
    }

    private void Start()
    {
        spawnPoints = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        StartCoroutine(SpawnEnemy());
        playerTr.GetComponent<PlayerHealth>().OnDeath += () =>
        {
            isGameOver = true;
            enemyList.FindAll(x => x.gameObject.activeSelf).ForEach(x => x.Die());
            Debug.Log(isGameOver);
        };
    }

    private IEnumerator SpawnEnemy()
    {
        while (!isGameOver)
        {
            yield return wsSpawn;

            if(enemyCount < maxEnemy && !isGameOver)   
            {
                int idx = UnityEngine.Random.Range(1, spawnPoints.Length);
                EnemyHealth eh = enemyList.Find(x => !x.gameObject.activeSelf); 

                if(eh != null)
                {
                    GameObject e = CreateEnemy();
                    eh = e.GetComponent<EnemyHealth>();
                    enemyList.Add(eh);
                }
                eh.transform.position = spawnPoints[idx].position;
                eh.gameObject.SetActive(true);
                enemyCount++;

                Action handler = null; 

                handler = () =>
                {
                    enemyCount--;
                    eh.OnDeath -= handler;
                };

                eh.OnDeath += handler;
            }
        }
    }

    public Transform GetPlayer()
    {
        return playerTr;
    }

}
