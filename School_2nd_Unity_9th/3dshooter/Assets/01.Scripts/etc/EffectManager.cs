using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    public GameObject bloddEffectPrefab;
    private List<GameObject> bloodEffectList = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        for (int i = 0; i < 15; i++)
        {
            MakeBloodEffect(out GameObject obj);
        }
    }

    private void MakeBloodEffect(out GameObject obj)
    {
        GameObject temp = Instantiate(bloddEffectPrefab, transform.position, Quaternion.identity, transform);
        temp.SetActive(false);
        bloodEffectList.Add(temp);
        obj = temp;
    }

    public GameObject GetBloodEffect()
    {
        GameObject effect = Instance.bloodEffectList.Find(x => !x.activeSelf);

        if(effect == null)
        {
            MakeBloodEffect(out GameObject obj);
            bloodEffectList.Add(obj);
            effect = obj;
        }

        return effect;
    }
}
