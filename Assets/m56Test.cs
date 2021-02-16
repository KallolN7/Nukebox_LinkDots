using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m56Test : MonoBehaviour
{
    //1. Reverse numbers
    //2. Reverse negative numbers
    //3. Reverse number without loops
    //4. Find out the shortest rounds to match two given words.you can change only 1 letter per round.And the word per round has to be a valid word(match with dictionary)
    //5. 5 balls. 5 different properties.How to assign those properties to the balls/ball.

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Loop Enter= -589" + " | Result= " + GetReverseInt(-589));
        reverseMethod(589);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private int GetReverseInt(int i)
    {
        int reverse = 0, r;

        while (i != 0)
        {
            r = i % 10;
            reverse = reverse * 10 + r;
            i /= 10;
        }
        return reverse;
    }

    int reverse = 0, r;
    private void reverseMethod(int i)
    {
        r = i % 10;
        reverse = reverse * 10 + r;
        i /= 10;
        Debug.Log("Reverse= " + reverse);

        if (i > 0)
            reverseMethod(i);

     }


    //private int GetLeastStepsToMatchStrings2(string inputString, string givenString)
    //{
    //      int rounds = 0;

    //    if(inputString == givenString)
    //        return rounds;
    //    if (givenString.Length != inputString.Length)
    //        return rounds;

    //    for (int i = 0; i < givenString.Length; i++)
    //    {
    //        char CharOfInputString = inputString[i];
    //        char charOfGivenString = givenString[i];

    //        if(CharOfInputString!= charOfGivenString)
    //        {
    //            inputString.Replace(CharOfInputString, GetAppropriateCharFromDictionary());

    //            if(IsValidWord(inputString))
    //                rounds++;
    //            else
    //                inputString.Replace(GetAppropriateCharFromDictionary(), CharOfInputString);
    //        }
    //    }
    //    return rounds;
    //}

    int rounds;

    private int GetLeastStepsToMatchStrings(string inputString, string givenString)
    {

        if (inputString == givenString)
            return rounds;
        if (givenString.Length != inputString.Length)
            return rounds;

        for (int i = 0; i < givenString.Length; i++)
        {
            char CharOfInputString = inputString[i];
            char charOfGivenString = givenString[i];

            if (CharOfInputString != charOfGivenString)
            {
                inputString.Replace(CharOfInputString, GetAppropriateCharFromDictionary(CharOfInputString, inputString));
                rounds++;
            }
        }

        if(inputString != givenString)
        {
            GetLeastStepsToMatchStrings(inputString, givenString);
        }

        return rounds;
    }

    private bool IsValidWord(string givenString)
    {
        //logic to check if givenString is a valid word in dictionary
        return true;
    }


    private char GetAppropriateCharFromDictionary(char CharOfInputString, string inputString)
    {
        //logic to find appropriate letter from dictionary after replacing CharOfInputString of inputString so that the final word reults in a valid word
        return ' ';
    }
}

