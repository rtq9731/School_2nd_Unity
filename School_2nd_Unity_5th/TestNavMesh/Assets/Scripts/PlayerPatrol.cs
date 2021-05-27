using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPatrol : MonoBehaviour
{
    [SerializeField] Transform[] wayPoints;
    [SerializeField] float remainDistMin = 1f; // 감지거리

    int destPoint = 0;

    NavMeshAgent playerAgnet;

    private void Start()
    {
        playerAgnet = GetComponent<NavMeshAgent>();
        if(playerAgnet != null)
        {
            playerAgnet.autoBraking = false;
            GoToNextPoint();
        }
    }

    private void Update()
    {
        // pathPending
        // -> 계산중이지만 준비되지 않은 경로 ( 계산 중이면 true , 계산이 끝났으면 false )
        // 즉 움직임의 계산을 끝냈고, 목적지랑 충분히 가깝다면 다음 목적지로
        if (!playerAgnet.pathPending && playerAgnet.remainingDistance <= remainDistMin)
        {
            GoToNextPoint();
        }
    }

    private void GoToNextPoint()
    {
        if (wayPoints.Length == 0) // 에외처리 코드
        {
            Debug.LogError("웨이포인트 값이 지정되지 않았습니다!");
            enabled = false;
            return;
        }

        playerAgnet.destination = wayPoints[destPoint].position;

        // 방법 1
        //destPoint++;
        //if (destPoint >= wayPoints.Length)
        //    destPoint = 0;

        // 방법 2
        destPoint = (destPoint + 1) % wayPoints.Length;
        

    }
}
