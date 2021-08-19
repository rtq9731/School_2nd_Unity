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

    // delegate => 함수의 대리자 -> 진짜를 실행함
    // 일급 시민 => 변수에 담을 수 있는 것
    // 함수형 프로그램에서 1급 시민으로 함수를 데려와야 함.
}
