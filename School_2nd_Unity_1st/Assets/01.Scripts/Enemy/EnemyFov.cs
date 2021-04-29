using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFov : MonoBehaviour
{
    public float viewRange = 15f;
    [Range(0, 360)]
    public float viewAngle = 120f;

    public LayerMask layerMask;
    private Transform playerTr;
    private int playerLayer;

    private void Start()
    {
        GameObject temp = GameObject.FindWithTag("Player");
        if(temp != null)
        {
            playerTr = temp.transform;
        }
        playerLayer = LayerMask.NameToLayer("Player");

    }


    //반지름 1인 원의 원주에 있는 점의 좌표를 구하는 함수
    public Vector3 CirclePoint(float angel)
    {
        angel += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angel * Mathf.Deg2Rad), 0, Mathf.Cos(angel * Mathf.Deg2Rad));
    }

    public bool isTracePlayer()
    {
        bool isTrace = false;
        Collider[] colls = Physics.OverlapSphere(transform.position, viewRange, 1 << playerLayer);
        if(colls.Length == 1)
        {
            Vector3 dir = (playerTr.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, dir) < viewAngle * 0.5f)
            {
                isTrace = true;
            }
        }
        return isTrace;
    }

    public bool IsViewPlayer()
    {
        bool isView = false;

        return isView;
    }

}
