using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // �������̽��� �޼ҵ�� �� public
    void OnDamage(float damage, Vector3 hitPos, Vector3 hitNormal);
}
