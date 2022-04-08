using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public Action OnWaveNumberChanged;  // ����?

    // 1 ���̺� �⺻
    private enum eWaveState
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }
    private eWaveState state;


    // 2 ���̺� �ѹ��� ���� ���� ������
    private int waveNumber; // ���̺� ���ڴ� �ð��� �� ���� ������
    private float nextWaveSpawnTimer;   // ���� ���̺������ ���ð�
    private float nextEnemySpawnTimer;  // �� ������ ���ÿ� �������� ������ �����ϱ� ���� Ÿ�̸�
    private int remainingEnemySpawnAmount;  // �� ���̺꿡 ����� ���� ������ ������?


    // 3 ���� ������ ����Ʈ�� ����
    //[SerializeField] private Transform spawnPositionTransform;
    [SerializeField] private List<Transform> spawnPositionTransformList;
    private Vector3 spawnPosition;

    // ���� ������ ��ġ�� �̹����� ǥ��
    [SerializeField] private Transform nextWaveSpawnPositionTransform;


    private void Awake()
    {
        // �ڽ����� �� ���� ��ġ�� �����Ѵٸ� ����Ʈ�� �־���
        if (spawnPositionTransformList != null && spawnPositionTransformList.Count > 0)
            spawnPositionTransformList.Clear();

        // �ڽ��� ù��° ������Ʈ�� nextWaveSpawnPos�̹Ƿ� ��ŵ�ϰ� �������� �߰�
        for (int i = 1; i < this.transform.childCount; i++)
        {
            spawnPositionTransformList.Add(this.transform.GetChild(i));
        }
    }

    private void Start()
    {
        // �ּ� ���� ���� �� ����
        state = eWaveState.WaitingToSpawnNextWave;

        // 3 ���� ���� ��ġ�� �������� �������ش�
        // spawnPosition = spawnPositionTransform.position;
        SetRandomSpawnPos();

        // ���� ���� ���� �� 3�� ���Ŀ� ��ȯ ����
        nextWaveSpawnTimer = 3f;
    }

    private void Update()
    {
        switch (state)
        {
            case eWaveState.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer < 0f)
                {
                    SpawnWave();
                }
                break;
            case eWaveState.SpawningWave:
                /*
                    ���� ������ ���� ���ڰ� 0�� �� ������ ��ȯ��
                    nextEnemySpawnTimer �� ������ �Ź� 200ms�� �������� �����ν� ��ġ�� �����ϴ°� ������ �� ����
                */
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, .2f);

                        // ���� �� ���� �� remainingEnemySpawnAmount �ϳ��� ����
                        Enemy.Create(spawnPosition + UtilClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
                        remainingEnemySpawnAmount--;

                        // ���� ������ ���� ��� �����ߴٸ� ���ο� ������ġ�� �������� �ް� �ٽ� ���� �����·�...
                        if (remainingEnemySpawnAmount <= 0)
                        {
                            state = eWaveState.WaitingToSpawnNextWave;

                            // 3 ��ġ�� ������ ���� ����Ʈ���� �������� ������
                            // spawnPosition = spawnPositionTransform.position;
                            SetRandomSpawnPos();

                            nextWaveSpawnTimer = 10f;   // �̷������� �ܺν�Ʈ�� ����
                        }
                    }
                }
                break;
        }
    }

    private void SpawnWave()
    {
        // ���̺� ���ڰ� �þ���� �����ϴ� ���� ���ڷ� ���� �÷���
        remainingEnemySpawnAmount = 5 + 3 * waveNumber;     // �̷������� �ܺν�Ʈ�� ����

        state = eWaveState.SpawningWave;
        waveNumber++;

        OnWaveNumberChanged?.Invoke();
    }

    private void SetRandomSpawnPos()
    {
        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public float GetNextWaveSpawnTimer()
    {
        return nextWaveSpawnTimer;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}