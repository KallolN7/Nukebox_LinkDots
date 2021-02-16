using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delegates : MonoBehaviour
{
    public delegate void TestDelegate();
    public delegate int TestDelegate2(int i);
    public TestDelegate testDelegate;
    public TestDelegate2 testDelegate2;

    private void Start()
    {
        testDelegate2 += GetInt;
        testDelegate2(2);
    }

    public int GetInt(int i)
    {
        Debug.Log("GetInt " + i);
        return i;
    }
    public void TestFunction1()
    {
        Debug.Log("TestFuntion1");
    }

    public void TestFunction2()
    {
        Debug.Log("TestFuntion2");
    }


}
