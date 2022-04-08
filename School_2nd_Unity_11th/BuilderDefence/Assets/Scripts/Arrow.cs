using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public static Arrow Create(Vector2 pos, Enemy enemy)
    {
        Transform arrowPrefab = Resources.Load<Transform>("ArrowPrefab");
        Transform arrowTrm = Instantiate(arrowPrefab).transform;
        arrowTrm.position = pos;

        Arrow arrowProjectile = arrowTrm.GetComponent<Arrow>();
        arrowProjectile.SetTarget(enemy);

        return arrowProjectile;
    }

    private Enemy target;
    private Vector3 lastMoveDir;

    private float timeToDie = 2f; // Ÿ���� ����� ���̽��� �� ��

    private void Update()
    {
        Vector3 moveDir;
        if (target != null)
        {
            moveDir = (target.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        }
        else // ���� ���� ��������� ������ �������� ��� ����
        {
            moveDir = lastMoveDir;
        }

        float moveSpeed = 20f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, 0, UtilClass.GetAngleFromVector(moveDir));

        if ((timeToDie -= Time.deltaTime) <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.GetComponent<HealthSystem>().Damage(10);
            Destroy(gameObject);
        }
    }

    private void SetTarget(Enemy targetEnemy)
    {
        target = targetEnemy;
    }
}
