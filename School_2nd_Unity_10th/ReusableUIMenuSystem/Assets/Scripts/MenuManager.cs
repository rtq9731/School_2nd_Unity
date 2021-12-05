using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class MenuManager : MonoBehaviour
{
    private static MenuManager instance;
    public static MenuManager Instance { get { return instance; }}

    //메뉴의 부모 트랜스폼
    [SerializeField] Transform menuParent;

    //메뉴 매니저에서 관리할 개별 클래스
    public MainMenu mainMenuPrefab;
    public CreditMenu creditMenuPrefab;
    public OptionMenu optionMenuPrefab;

    //스택으로 캔버스 메뉴 관리
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

        //Reflection을 사용하여 함수 타입을 얻어와서 통합
        System.Type myType = this.GetType();

        BindingFlags myFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
        FieldInfo[] fields = myType.GetFields(myFlag);

        //디버깅
        for (int i = 0; i < fields.Length; i++)
        {
            Debug.Log("필드 함수들 : " + fields[i]);
        }

        // 메뉴 프리팹 생성
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
            Debug.LogWarning("메뉴 스택에 아무것도 없습니다!");
            return;
        }

        Menu topMenu = menuStack.Pop();
        topMenu.gameObject.SetActive(false);

        if(menuStack.Count > 0)
        {
            menuStack.Peek().gameObject.SetActive(true);
            // 그 다음 메뉴를 꺼내서 활성화 ( 제거 없이 )
        }
    }
}
