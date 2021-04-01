using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecreateBox : MonoBehaviour
{
    [SerializeField]
    public GameObject newObj = null;

    [SerializeField]
    public float recreateTime = 0;
    private float recreateTimer = 0;

    // Update is called once per frame
    void Update()
    {
        recreateTimer += Time.deltaTime;
        if (recreateTime < recreateTimer)
            NewObj();
    }

    private void NewObj()
    {
        Instantiate(newObj, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }

}
