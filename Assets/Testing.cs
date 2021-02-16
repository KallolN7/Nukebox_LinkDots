using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;

public class Testing : MonoBehaviour
{

    public TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
        AsyncTest();
       //this.transform.DOMove(transform.position, 1);
    }

    private void ReverseString(string str)
    {
        char[] charArray = str.ToCharArray();
        for (int i = 0, j = str.Length - 1; i < j; i++, j--)
        {
            charArray[i] = str[j];
            charArray[j] = str[i];
        }
        string newStr = new string(charArray);
        Debug.Log("Given String= " + str + " | Output string= " + newStr);
    }

    private void PalindromeString(string str)
    {
        char[] charArray = str.ToCharArray();
        for (int i = 0, j = str.Length - 1; i < j; i++, j--)
        {
            charArray[i] = str[j];
            charArray[j] = str[i];
        }
        string newStr = new string(charArray);
        if(newStr == str)
            Debug.Log("Given String= " + str + " | Output string= " + newStr + " | isPalindrome=  true");
        else
            Debug.Log("Given String= " + str + " | Output string= " + newStr + " | isPalindrome=  false");
    }

    private void GetCountOfEachCharacterInString(string str)
    {
        Dictionary<char, int> characterCount = new Dictionary<char, int>();
        for(int i = 0; i< str.Length; i++)
        {
            if (!characterCount.ContainsKey(str[i]))
            {
                characterCount.Add(str[i], 1);
            }
            else
            {
                characterCount[str[i]]++;
            }
        }
        foreach( KeyValuePair<char, int> a in characterCount)
        {
            Debug.Log(a.Key + " = " + a.Value);
        }
    }

    private void RemoveDuplicateCharacters(string str)
    {
        Dictionary<char, int> characterCount = new Dictionary<char, int>();
        for (int i = 0; i < str.Length; i++)
        {
            if (!characterCount.ContainsKey(str[i]))
            {
                characterCount.Add(str[i],1);
            }
        }
        foreach (KeyValuePair<char, int> a in characterCount)
        {
            Debug.Log(a.Key + " = " + a.Value);
        }
    }

    private void LeftCircularArray(int[] array)
    {
        int size = array.Length;
        int temp;

        for (int i = size -1; i > 0; i--)
        {
            temp = array[size - 1];
            array[size - 1] = array[i-1];
            array[i - 1] = temp;
        }
        foreach(int a in array)
        {
            Debug.Log(a);
        }
    }

    private void RightCircularArray(int[] array)
    {
        int size = array.Length;
        int temp;

        for (int i = 0; i < size-1; i++)
        {
            temp = array[0];
            array[0] = array[i+1];
            array[i+1] = temp;
        }
        foreach (int a in array)
        {
            Debug.Log(a);
        }
    }

    private void PrintStars(int count)
    {
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                textMesh.text = " * ";
            }
            textMesh.text = "\n";
        }
    }

    private int GetDigitsSumWithRecursion(int num)
    {
        if (num != 0)
            return ((num % 10 + GetDigitsSumWithRecursion(num / 10)));
        else
            return 0;
    }

    int sum = 0;
    public  virtual void CheckForArmstrongNumber(int num)
    {
        while (num > 0)
        {
            sum += Convert.ToInt32(Mathf.Pow((num % 10), 3)) ;
            num /= 10;
        }
        if (sum == num)
            Debug.Log(sum + " is Armstrong Number");
        else
            Debug.Log(sum + " is not Armstrong Number");
    }


    private  void AsyncTest()
    {
        var task1 = Task.Run(() =>
        {
            StartCoroutine(Add());
        });
        var result = task1.GetAwaiter();
        result.OnCompleted(() =>
        {
            StartCoroutine(Add());
        });
    }

 private IEnumerator Add()
    {
        yield return new WaitForSeconds(3);
        Debug.Log((2 + 5).ToString());

    }

    //private IEnumerator AddNew()
    //{
    //    yield return new WaitCallback(Add);
    //    Debug.Log((2 + 5).ToString());

    //}
}

public class InheritedClass : Testing
{
    int sum = 0;
    public  override void CheckForArmstrongNumber(int num)
    {
        while (num > 0)
        {
            sum += Convert.ToInt32(Mathf.Pow((num % 10), 3));
            num /= 10;
        }
        if (sum == num)
            Debug.Log(sum + " is Armstrong Number");
        else
            Debug.Log(sum + " is not Armstrong Number");
    }
}