using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPatrol : MonoBehaviour
{
    [SerializeField] Transform[] wayPoints;
    [SerializeField] float remainDistMin = 1f; // �����Ÿ�

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
        // -> ����������� �غ���� ���� ��� ( ��� ���̸� true , ����� �������� false )
        // �� �������� ����� ���°�, �������� ����� �����ٸ� ���� ��������
        if (!playerAgnet.pathPending && playerAgnet.remainingDistance <= remainDistMin)
        {
            GoToNextPoint();
        }
    }

    private void GoToNextPoint()
    {
        if (wayPoints.Length == 0) // ����ó�� �ڵ�
        {
            Debug.LogError("��������Ʈ ���� �������� �ʾҽ��ϴ�!");
            enabled = false;
            return;
        }

        playerAgnet.destination = wayPoints[destPoint].position;

        // ��� 1
        //destPoint++;
        //if (destPoint >= wayPoints.Length)
        //    destPoint = 0;

        // ��� 2
        destPoint = (destPoint + 1) % wayPoints.Length;
        

    }
}
