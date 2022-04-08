using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector2 pos)
    {
        Transform enemyPrefab = Resources.Load<Transform>("EnemyPrefab");
        return Instantiate(enemyPrefab, pos, Quaternion.identity).GetComponent<Enemy>();
    }

    private Rigidbody2D rigid;
    private Transform targetTransform;
    private HealthSystem healthSystem;

    float lookForTargetTimer = 0f;
    float lookForTargetTimerMax = 0.2f;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        targetTransform = BuilderManager.Instance.GetCommandCenter().transform;

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDie += CallDie;

        lookForTargetTimer = UnityEngine.Random.Range(0f, lookForTargetTimerMax);
    }

    private void Update()
    {
        HandleTargeting();
        HandleMovement();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.transform.GetComponent<Building>();
        if(building != null)
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);

            CallDie();
        }
    }

    private void HandleTargeting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if(lookForTargetTimer <= 0)
        {
            lookForTargetTimer = lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void LookForTargets()
    {
        // 사거리 안 건물 체크
        // 타겟이 있다면 현재 타겟은 어떻게?
        // 건물이 여러개면 가장 가까운 건물
        // 건물이 없으면 커멘드 센터 돌진

        float targetMaxRadius = 10f;
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (var item in targets)
        {
            Building building = item.GetComponent<Building>();
            if(building != null)
            {
                if (targetTransform == null)
                {
                    targetTransform = building.transform;
                }
                else
                {
                    if (Vector2.Distance(transform.position, building.transform.position) < 
                        Vector2.Distance(transform.position, targetTransform.position)) // 가까운 건물 우선 타겟
                    {
                        targetTransform = building.transform;
                    }
                }
            }
        }
        
        if(targetTransform == null)
        {
            targetTransform = BuilderManager.Instance.GetCommandCenter().transform;
        }
    }

    private void HandleMovement()
    {
        if(targetTransform != null)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized * 6;
            rigid.velocity = moveDir;
        }
        else
        {
            rigid.velocity = Vector2.zero;
        }
    }

    private void CallDie()
    {
        Destroy(gameObject);
    }
}
