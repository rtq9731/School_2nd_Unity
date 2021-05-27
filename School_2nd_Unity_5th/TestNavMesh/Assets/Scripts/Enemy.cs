using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform playerTr;

    NavMeshAgent enemyAgent;

    public LayerMask whatIsGround, whatIsplayer;

    // 추적
    [SerializeField] Vector3 walkPoint;
    [SerializeField] float walkPointRange;

    bool walkPointSet;

    // 공격
    [SerializeField] float timeBetweenAttack;
    [SerializeField] GameObject projectTile; // 발사체

    bool isAttacked;

    // 현재 상태
    [SerializeField] float sightRange, attackRange;

    private bool playerInSightRange, playerInAttackRange;


    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        playerTr = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        // 거리를 체크하는 로직
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsplayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsplayer);
        
        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();

        if(playerInSightRange && playerInAttackRange)
            AttackPlayer();

        if(!playerInSightRange && !playerInAttackRange)
            Patrolling();

        // 시야 / 공격 거리

        // 수색
        // 추적
        // 공격
    }


    private void Patrolling()
    {
        Debug.Log("패트롤");
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        else
        {
            enemyAgent.SetDestination(walkPoint);
        }

        Vector3 distToWalkPoint = transform.position - walkPoint;

        if(distToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

       if(Physics.Raycast(walkPoint, -transform.up, 2f , whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        Debug.Log("플레이어 감지됨!");
        enemyAgent.SetDestination(playerTr.position);
    }

    private void AttackPlayer()
    {
        Debug.Log("공격");
        enemyAgent.SetDestination(transform.position);

        transform.LookAt(playerTr);

        if(!isAttacked)
        {
            Rigidbody rb = Instantiate(projectTile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            isAttacked = true;
            Invoke("ResetAttack", timeBetweenAttack);
            Destroy(rb.gameObject, 2f);
        }
    }

    void ResetAttack()
    {
        isAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
