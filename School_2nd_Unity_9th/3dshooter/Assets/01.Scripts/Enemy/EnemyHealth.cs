using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : LivingEntity
{
    public float bloodEffectTime = 0.5f;
    private EnemyAI ai;

    private void Awake()
    {
        ai = GetComponent<EnemyAI>();
    }

    public override void OnDamage(float damage, Vector3 point, Vector3 normal)
    {
        if (dead) return;

        base.OnDamage(damage, point, normal);
        StartCoroutine(ShowBloodEffect(point, normal));
    }

    private IEnumerator ShowBloodEffect(Vector3 point, Vector3 normal)
    {
        GameObject effect = EffectManager.Instance.GetBloodEffect();
        Quaternion rot = Quaternion.LookRotation(normal);
        effect.transform.position = point;
        effect.transform.rotation = rot;
        effect.SetActive(true);
        yield return new WaitForSeconds(bloodEffectTime);
        effect.SetActive(false);
    }

    public override void Die()
    {
        base.Die();
        ai.SetDead();
    }
}
