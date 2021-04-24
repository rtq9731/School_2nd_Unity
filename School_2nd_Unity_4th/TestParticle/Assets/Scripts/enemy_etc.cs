using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_etc : MonoBehaviour
{
    [SerializeField] float HP = 5;
    [SerializeField] float respawnTime = 5f;
    [SerializeField] Material mat;

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
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
        float r = Random.Range(1, 255);
        float g = Random.Range(1, 255);
        float b = Random.Range(1, 255);
        gameObject.GetComponent<Renderer>().material.color = new Color(r / 255, g / 255, b / 255, 1);
    }
}
