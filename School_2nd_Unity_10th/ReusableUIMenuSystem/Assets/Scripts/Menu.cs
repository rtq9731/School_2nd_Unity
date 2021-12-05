using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Menu<T>�� �����ΰ�?
//           �� Menu Ŭ������ ���׸� ����� �ް� ������ �ǰ���
public abstract class Menu<T> : Menu where T : Menu<T>
{
    private static T instance = null;
    public static T Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }

    public static void Open()
    {
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.OpenMenu(instance);
        }
    }
}

public abstract class Menu : MonoBehaviour
{
    public virtual void OnBackPressed()
    {
        if(MenuManager.Instance != null)
        {
            MenuManager.Instance.CloseMenu();
        }
    }
}
