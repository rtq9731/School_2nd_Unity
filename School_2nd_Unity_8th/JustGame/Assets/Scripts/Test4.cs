using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test4 : MonoBehaviour
{
    SerializeField
    public delegate int GGM(int a, int b);

    public GGM g;

    public void MyGGM(int a, int b)
    {
        print(a + b);
    }
    public void MyGGM2(int a, int b)
    {
        print(a - b);
    }

    private void Start()
    {

        List<int> intList = new List<int>();

        intList.Add(1);
        intList.Add(4);
        intList.Add(5);
        intList.Add(6);

        intList.Sort((x, y) => -x.CompareTo(y));

        g = (x, y) => x - y;
        g += (x, y) => x + y;
        g(3, 5);
    }

    // delegate => �Լ��� �븮�� -> ��¥�� ������
    // �ϱ� �ù� => ������ ���� �� �ִ� ��
    // �Լ��� ���α׷����� 1�� �ù����� �Լ��� �����;� ��.
}
