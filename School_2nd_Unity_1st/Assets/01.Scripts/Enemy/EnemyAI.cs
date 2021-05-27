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

    private Animator anim;
    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashSpeed = Animator.StringToHash("speed");

    private void Awake()
    {
        moveAgent = GetComponent<MoveAgent>();
        anim = gameObject.GetComponent<Animator>();
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

    private void Update()
    {
        anim.SetFloat(hashSpeed, moveAgent.speed);
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(DoAction());
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
                    anim.SetBool(hashMove, true);
                    break;
                case EnemyState.Trace:
                    moveAgent.traceTarget = playerTr.position;
                    anim.SetBool(hashMove, true);
                    break;
                case EnemyState.Attack:
                    moveAgent.Stop();
                    anim.SetBool(hashMove, false);
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
