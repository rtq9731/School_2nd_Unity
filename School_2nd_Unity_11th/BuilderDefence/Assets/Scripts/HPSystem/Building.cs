using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO buildingType;
    private HealthSystem healthSystem;

    private void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;

        healthSystem = GetComponent<HealthSystem>();

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);

        healthSystem.OnDie += CallHealthSystemOnDie;
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.T))
        //{
        //    healthSystem.Damage(10);
        //}
    }

    private void CallHealthSystemOnDie()
    {
        Destroy(gameObject);
    }
}
