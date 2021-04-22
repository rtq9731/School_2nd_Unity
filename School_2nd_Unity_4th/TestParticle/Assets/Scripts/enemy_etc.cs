using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_etc : MonoBehaviour
{
    [SerializeField] float HP = 5;
    [SerializeField] float respawnTime = 5f;

    private void OnParticleCollision(GameObject other)
    {
        Damage();
    }

    void Damage()
    {
        HP--;
        if (HP < 0)
            Die();
    }

    void Die()
    {
        Invoke("Respawn", respawnTime);
        gameObject.SetActive(false);
    }

    void Respawn()
    {
        gameObject.SetActive(true);
    }
}
