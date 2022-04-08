using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public Action OnWaveNumberChanged;  // 퀴즈?

    // 1 웨이브 기본
    private enum eWaveState
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }
    private eWaveState state;


    // 2 웨이브 넘버에 따른 관련 변수들
    private int waveNumber; // 웨이브 숫자는 시간이 갈 수록 증가함
    private float nextWaveSpawnTimer;   // 다음 웨이브까지의 대기시간
    private float nextEnemySpawnTimer;  // 적 스폰시 동시에 여러마리 스폰을 방지하기 위한 타이머
    private int remainingEnemySpawnAmount;  // 한 웨이브에 몇마리의 적을 생성할 것인지?


    // 3 스폰 지점을 리스트로 생성
    //[SerializeField] private Transform spawnPositionTransform;
    [SerializeField] private List<Transform> spawnPositionTransformList;
    private Vector3 spawnPosition;

    // 다음 생성될 위치를 이미지로 표시
    [SerializeField] private Transform nextWaveSpawnPositionTransform;


    private void Awake()
    {
        // 자식으로 적 스폰 위치가 존재한다면 리스트에 넣어줌
        if (spawnPositionTransformList != null && spawnPositionTransformList.Count > 0)
            spawnPositionTransformList.Clear();

        // 자식의 첫번째 오브젝트는 nextWaveSpawnPos이므로 스킵하고 다음부터 추가
        for (int i = 1; i < this.transform.childCount; i++)
        {
            spawnPositionTransformList.Add(this.transform.GetChild(i));
        }
    }

    private void Start()
    {
        // 최소 스폰 시작 전 세팅
        state = eWaveState.WaitingToSpawnNextWave;

        // 3 스폰 생성 위치를 랜덤으로 지정해준다
        // spawnPosition = spawnPositionTransform.position;
        SetRandomSpawnPos();

        // 최초 게임 시작 후 3초 이후에 순환 시작
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
                    스폰 예정된 적의 숫자가 0이 될 때까지 순환됨
                    nextEnemySpawnTimer 값 세팅은 매번 200ms의 랜덤값을 줌으로써 겹치게 생성하는걸 방지할 수 있음
                */
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0f, .2f);

                        // 실제 적 생성 후 remainingEnemySpawnAmount 하나씩 감소
                        Enemy.Create(spawnPosition + UtilClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
                        remainingEnemySpawnAmount--;

                        // 스폰 예정된 적을 모두 소진했다면 새로운 스폰위치를 랜덤으로 받고 다시 스폰 대기상태로...
                        if (remainingEnemySpawnAmount <= 0)
                        {
                            state = eWaveState.WaitingToSpawnNextWave;

                            // 3 위치를 지정한 스폰 리스트에서 랜덤으로 가져옴
                            // spawnPosition = spawnPositionTransform.position;
                            SetRandomSpawnPos();

                            nextWaveSpawnTimer = 10f;   // 이런값들은 외부시트로 관리
                        }
                    }
                }
                break;
        }
    }

    private void SpawnWave()
    {
        // 웨이브 숫자가 늘어날수록 스폰하는 적의 숫자로 같이 늘려줌
        remainingEnemySpawnAmount = 5 + 3 * waveNumber;     // 이런값들은 외부시트로 관리

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