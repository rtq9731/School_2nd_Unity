using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float shootTimerMax;

    float shootTimer = 0f;

    Enemy targetEnemy;

    float lookForTargetTimer = 0f;
    float lookForTargetTimerMax = 0.2f;

    Vector3 projectileSpawnPosition = Vector2.zero;

    private void Awake()
    {
        projectileSpawnPosition = transform.Find("ProjectileSpawnPos").position;
    }

    private void Update()
    {
        HandleTargeting();
        HandleShooting();
    }

    private void HandleTargeting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer <= 0)
        {
            lookForTargetTimer = lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void LookForTargets()
    {

        float targetMaxRadius = 20f;
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        Debug.Log(targets.Length);
        foreach (var item in targets)
        {
            Enemy enemy = item.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if (Vector2.Distance(transform.position, enemy.transform.position) <
                        Vector2.Distance(transform.position, targetEnemy.transform.position))
                    {
                        targetEnemy = enemy;
                    }
                }
            }
        }
    }

    private void HandleShooting()
    {
        shootTimer -= Time.deltaTime;
        if(shootTimer <= 0)
        {
            shootTimer = shootTimerMax;
            if(targetEnemy != null)
            {
                Arrow.Create(projectileSpawnPosition, targetEnemy);
            }
        }
    }
}
