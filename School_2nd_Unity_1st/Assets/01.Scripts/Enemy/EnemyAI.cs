using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Trace,
        Attack,
        Die
    };

    public EnemyState state = EnemyState.Patrol; // 처음에는 순찰 상태

    private Transform playerTr;

    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float traceRange = 10f;
    [SerializeField] private float judgeRange = 0.3f;

    public bool isDie = false;
    private WaitForSeconds ws;
    private MoveAgent moveAgent;

    private void Awake()
    {
        moveAgent = GetComponent<MoveAgent>();
    }

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTr = player.transform;
        }
        ws = new WaitForSeconds(judgeRange);
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(DoAction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CheckState()
    {
        while(!isDie)
        {
            if (state == EnemyState.Die)
                yield break;

            if (playerTr == null)
                yield return ws; 

            float dis = (playerTr.position - transform.position).sqrMagnitude;

            if(dis <= attackRange * attackRange)
            {
                state = EnemyState.Attack;
            }
            else if (dis <= traceRange * traceRange)
            {
                state = EnemyState.Trace;
            }    
            else
            {
                state = EnemyState.Patrol;
            }
            yield return ws;
        }

    }

    private IEnumerator DoAction()
    {
       while(!isDie)
        {
            yield return ws;
            switch (state)
            {
                case EnemyState.Patrol:
                    moveAgent.isPatrol = true;
                    break;
                case EnemyState.Trace:
                    moveAgent.traceTarget = playerTr.position;
                    break;
                case EnemyState.Attack:
                    moveAgent.Stop();
                    break;
                case EnemyState.Die:
                    moveAgent.Stop();
                    break;
                default:
                    break;
            }
        }
    }
}
