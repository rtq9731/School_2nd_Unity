using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyFov))]
public class FovEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyFov fov = (EnemyFov)target;

        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);
        Handles.color = Color.white;
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.viewRange);

        Handles.color = new Color(1,1,1,0.4f);
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePos, fov.viewAngle, fov.viewRange);

        GUIStyle style = new GUIStyle();
        style.fontSize = 35;
        Handles.Label(fov.transform.position + fov.transform.forward * 5f, fov.viewRange.ToString(), style);
    }
}
