using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class MenuManager : MonoBehaviour
{
    private static MenuManager instance;
    public static MenuManager Instance { get { return instance; }}

    //�޴��� �θ� Ʈ������
    [SerializeField] Transform menuParent;

    //�޴� �Ŵ������� ������ ���� Ŭ����
    public MainMenu mainMenuPrefab;
    public CreditMenu creditMenuPrefab;
    public OptionMenu optionMenuPrefab;

    //�������� ĵ���� �޴� ����
    private Stack<Menu> menuStack = new Stack<Menu>();

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            Init();
            DontDestroyOnLoad(this.gameObject);
        }
    }


    private void OnDestroy()
    {
        if(instance != null)
        {
            instance = null;
        }
    }

    private void Init()
    {
        if(menuParent == null)
        {
            GameObject menuParent = new GameObject("Menus");
            this.menuParent = menuParent.transform;
        }
        DontDestroyOnLoad(menuParent);

        //Reflection�� ����Ͽ� �Լ� Ÿ���� ���ͼ� ����
        System.Type myType = this.GetType();

        BindingFlags myFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
        FieldInfo[] fields = myType.GetFields(myFlag);

        //�����
        for (int i = 0; i < fields.Length; i++)
        {
            Debug.Log("�ʵ� �Լ��� : " + fields[i]);
        }

        // �޴� ������ ����
        //Menu[] menus = { mainMenuPrefab, optionMenuPrefab, creditMenuPrefab };

        //for (int i = 0; i < menus.Length; i++)
        foreach (var field in fields)
        {
            Menu prefab = field.GetValue(this) as Menu;
            if(prefab != null)
            {
                Menu menuInstance = Instantiate(prefab, menuParent);

                if(prefab != mainMenuPrefab)
                {
                    menuInstance.gameObject.SetActive(false);
                }
                else
                {
                    OpenMenu(menuInstance);
                }
            }
        }
    }
    
    public void OpenMenu(Menu menu)
    {
        if(menu == null)
        {
            Debug.Log(menu + "is Not Found!");
            return;
        }

        if(menuStack.Count > 0)
        {
            foreach (var item in menuStack)
            {
                menu.gameObject.SetActive(false);
            }
        }

        menu.gameObject.SetActive(true);
        menuStack.Push(menu);
    }

    public void CloseMenu()
    {
        if(menuStack.Count == 0)
        {
            Debug.LogWarning("�޴� ���ÿ� �ƹ��͵� �����ϴ�!");
            return;
        }

        Menu topMenu = menuStack.Pop();
        topMenu.gameObject.SetActive(false);

        if(menuStack.Count > 0)
        {
            menuStack.Peek().gameObject.SetActive(true);
            // �� ���� �޴��� ������ Ȱ��ȭ ( ���� ���� )
        }
    }
}
