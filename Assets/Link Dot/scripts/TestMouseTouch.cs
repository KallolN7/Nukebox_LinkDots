using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hitcode_linkDots
{
    public class TestMouseTouch : MonoBehaviour
    {

        public GameObject linkdotgame;
        void OnMouseDown()
        {

            if (linkdotgame != null)
            {
                //init the level
                GameData.difficulty = 0;//0-4
                GameData.getInstance().cLevel = 0;//0-49;


                linkdotgame.SetActive(true);
                linkdotgame.GetComponent<LinkDot>().init();
                GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }

        void linkDotWin()
        {
            print("win the game");
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}