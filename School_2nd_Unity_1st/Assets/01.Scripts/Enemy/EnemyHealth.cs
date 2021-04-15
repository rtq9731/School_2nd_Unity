using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{

    public float hp = 50f;
    public GameObject effect;
    public float playTumb = 0.5f;
    private float playTumbTimer = 0f;
    private GameObject temp;

    public void OnDamage(float damage, Vector3 hitPos, Vector3 hitNormal)
    {
        hp -= damage;

        if(temp == null)
        temp = Instantiate(effect, hitPos, Quaternion.LookRotation(hitNormal));

        if(playTumbTimer > playTumb)
        StartCoroutine(Hit(temp));

        if (hp <= 0)
            Die();
    }

    private void Update()
    {
        playTumbTimer += Time.deltaTime;
    }

    IEnumerator Hit(GameObject effect)
    {
        effect.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(playTumb);
        effect.GetComponent<ParticleSystem>().Stop();
        playTumbTimer = 0;
    }

    private void Die()
    {
        throw new NotImplementedException();
    }
}
