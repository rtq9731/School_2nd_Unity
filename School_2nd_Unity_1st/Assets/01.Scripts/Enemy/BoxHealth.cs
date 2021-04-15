using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHealth : MonoBehaviour, IDamageable
{

    public float hp = 50f;
    public GameObject effect;

    public void OnDamage(float damage, Vector3 hitPos, Vector3 hitNormal)
    {
        hp -= damage;

        GameObject temp = Instantiate(effect, hitPos, Quaternion.LookRotation(hitNormal));

        Destroy(temp, 1.0f);

        if (hp <= 0)
            Die();
    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}
