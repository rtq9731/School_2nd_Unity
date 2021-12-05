using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorMenu : MonoBehaviour
{
    [MenuItem("BuilderDefence/Scenes/Game")]
    static void EditorMenu_LoadGameScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/GameScene.unity");
    }
}
