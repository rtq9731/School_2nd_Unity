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
        // ��Ÿ� �� �ǹ� üũ
        // Ÿ���� �ִٸ� ���� Ÿ���� ���?
        // �ǹ��� �������� ���� ����� �ǹ�
        // �ǹ��� ������ Ŀ��� ���� ����

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
                        Vector2.Distance(transform.position, targetTransform.position)) // ����� �ǹ� �켱 Ÿ��
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
