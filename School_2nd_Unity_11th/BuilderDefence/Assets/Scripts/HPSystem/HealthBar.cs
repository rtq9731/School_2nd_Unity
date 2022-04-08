using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;

    private Transform barTrm;

    private void Awake()
    {
        barTrm = transform.Find("Bar");
    }

    private void Start()
    {
        healthSystem.OnDamaged += CallHealthSystemOnDamage;
        UpdateHealthBarVisible();
    }

    public void CallHealthSystemOnDamage()
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void UpdateHealthBarVisible()
    {
        gameObject.SetActive(!healthSystem.IsFullHealth());
    }

    private void UpdateBar()
    {
        barTrm.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
    }
}
