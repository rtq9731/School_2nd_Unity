using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] int healthAmountMax;
    private int curHealthAmount;

    public Action OnDamaged = () => { };
    public Action OnDie = () => { };

    private void Awake()
    {
        curHealthAmount = healthAmountMax;
    }
    
    public void Damage(int damageAmount)
    {
        curHealthAmount -= damageAmount;
        curHealthAmount = Mathf.Clamp(curHealthAmount, 0, healthAmountMax);

        OnDamaged?.Invoke();

        if(IsDead())
        {
            OnDie?.Invoke();
        }
    }

    public bool IsDead()
    {
        return curHealthAmount == 0;
    }

    public bool IsFullHealth()
    {
        return curHealthAmount == healthAmountMax;
    }

    public int GetHealthAmount()
    {
        return curHealthAmount;
    }

    public float GetHealthAmountNormalized()
    {
        return (float)curHealthAmount / healthAmountMax;
    }

    public void SetHealthAmountMax(int hpAmountMax, bool updateHpAmount)
    {
        healthAmountMax = hpAmountMax;
        if(updateHpAmount)
        {
            curHealthAmount = hpAmountMax;
        }
    }
}
