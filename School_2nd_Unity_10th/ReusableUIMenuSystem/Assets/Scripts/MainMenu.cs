using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu<MainMenu>
{

    public void OnPlayPressed()
    {
        Debug.Log("���� ����");
    }

    public void OnCreditPressed()
    {
        CreditMenu.Open();
    }

    public void OnOptionPressed()
    {
        OptionMenu.Open();
    }

    public override void OnBackPressed()
    {
        Application.Quit();
    }
}
