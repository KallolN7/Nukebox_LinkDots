using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
namespace Hitcode_linkDots
{
    public class TipPanel : MonoBehaviour
    {

        // Use this for initialization
        bool canTick = true;
        Transform bg;
        void Start()
        {
            bg = transform.Find("bg");
        }

        private void OnEnable()
        {
            lb_notip = transform.Find("bg").Find("lb_notip").GetComponentInChildren<Text>();
            canTick = true;

            GameObject.Find("tipTitle").GetComponentInChildren<Text>().text = Localization.Instance.GetString("askTip");
            GameObject.Find("btnYes").GetComponentInChildren<Text>().text = Localization.Instance.GetString("btnyes");
            GameObject.Find("btnNo").GetComponentInChildren<Text>().text = Localization.Instance.GetString("btnno");
        }

        // Update is called once per frame
        int n = 0;

        void Update()
        {
            if (n < 20)
            {
                n++;
                return;
            }
            else
            {
                n = 0;
            }

            if (!canTick)
                return;


            if (GameData.instance.tipRemain > 0)
            {
                lb_notip.text = Localization.Instance.GetString("tipRemain") + GameData.instance.tipRemain;
            }
            else
            {
                lb_notip.text = Localization.Instance.GetString("buyTip");
            }

            return;
            if (GameData.getInstance().tipRemain == 0)
            {

                //			DateTime tnow = System.DateTime.Now;
                TimeSpan ts = new TimeSpan(50, 0, 0, 0);
                DateTime dt2 = DateTime.Now.Subtract(ts);
                long tcTime = dt2.Ticks / 10000000;



                int tTimeLasts = (int)(tcTime - long.Parse(GameData.getInstance().tickStartTime));


                int secondRemain = 300 - tTimeLasts;
                if (secondRemain <= 0)
                {
                    secondRemain = 0;
                    //count of;
                    PlayerPrefs.SetInt("tipRemain", 1);
                    PlayerPrefs.SetString("tipStart", "0");
                    GameData.getInstance().tipRemain = 1;
                    GameData.getInstance().tickStartTime = "0";
                    GameData.getInstance().main.refreshView();
                    checkUI();
                    print("startrefresh");
                }

                //lb_notip.text = Localization.Instance.GetString("nextTip") + (secondRemain).ToString() + " seconds";


            }
        }
        //	public delegate void PanelChangedEventHandler();
        //	public event PanelChangedEventHandler showPanel;
        bool isShowed;
        bool canShow = true;
        public void showTipPanel()
        {
            GameManager.getInstance().playSfx("click");

            if (GameData.instance.isLock) return;


            showOrHideTipPanel();
            //GetComponent<Image>().raycastTarget = true;
        }

        bool isOpenStore;
        public void yesHandler()
        {
            if (!isShowed)
                return;
            GameManager.getInstance().playSfx("click");
            showOrHideTipPanel();

            if (GameData.instance.tipRemain > 0)
            {

                showTip();
            }
            else
            {
                //buy tip
                print("open store");
                isOpenStore = true;
            }
        }

        public void buyNow()
        {
            if (!isShowed)
                return;
            GameManager.getInstance().playSfx("click");
            showOrHideTipPanel();
            //buy tip
            print("open store");
            isOpenStore = true;
        }

        public void noHandler()
        {
            GameManager.getInstance().playSfx("click");
            showOrHideTipPanel();
        }





        public void OnShowCompleted()
        {
            // Add event handler code here
            //		print ("showOver");
            isShowed = true;
            canShow = true;
        }

        public void OnHideCompleted()
        {
            //		print ("hideOver");	
            isShowed = false;
            canShow = true;
            GameData.getInstance().isLock = false;
            GameObject.Find("btnRetry").GetComponent<Button>().interactable = true;

            //GetComponent<Image>().raycastTarget = false;
            if (isOpenStore)
            {
                GameData.instance.main.panelBuyCoinC.SetActive(true);
                GameData.instance.isLock = true;
                isOpenStore = false;
            }
        }

        Text lb_notip;
        Button btnYes, btnNo;
        void checkUI()
        {

            btnYes = bg.Find("btnYes").GetComponent<Button>();
            btnNo = bg.Find("btnNo").GetComponent<Button>();
            //		print (GameData.getInstance ().tipRemain + "remain");
            //if (GameData.getInstance().tipRemain == 0)
            //{

            //    lb_notip.enabled = true;
            //    btnYes.interactable = false;

            //}
            //else
            //{
            //    btnYes.interactable = true;
            //    lb_notip.enabled = false;
            //}
            //if (GameData.getInstance().isLock)
            //   Gameo.Find("btnRetryB").GetComponent<Button>().interactable = false;
        }
        float startX;
        public void showOrHideTipPanel()
        {
            if (!canShow)
                return;
            gameObject.SetActive(true);
            GameData.getInstance().tickStartTime = PlayerPrefs.GetString("tipStart", "0");
            // Add event handler code here
            if (!isShowed)
            {

                bg.DOMoveX(0, .2f).SetEase(Ease.Linear).OnComplete(OnShowCompleted);
                //						
                startX = bg.transform.position.x;

                canShow = false;
                GameData.getInstance().isLock = true;
                //disable some UI;
                checkUI();

            }
            else
            {

                canShow = false;

                transform.Find("bg").DOMoveX(startX, .2f).SetEase(Ease.Linear).OnComplete(OnHideCompleted);

            }


        }

        void showTip()
        {


            if (GameData.getInstance().tipRemain > 0)
            {
                GameData.getInstance().tipRemain--;
                PlayerPrefs.SetInt("tipRemain", GameData.getInstance().tipRemain);
                GameData.getInstance().main.refreshView();


                //have not give a tip
                //GameData.getInstance().tickStartTime = PlayerPrefs.GetString("tipStart", "0");
                //if (GameData.getInstance().tickStartTime == "0")
                //{
                //    canTick = false;
                //    //				long tcTime = System.DateTime.Now.Ticks;

                //    TimeSpan ts = new TimeSpan(50, 0, 0, 0);
                //    DateTime dt2 = DateTime.Now.Subtract(ts);
                //    //				print (dt2.Ticks/10000000/3600);
                //    long tcTime = dt2.Ticks / 10000000;

                //    PlayerPrefs.SetString("tipStart", tcTime.ToString());
                //    GameData.getInstance().tickStartTime = tcTime.ToString();
                //    //				print (tcTime+"tctime11");
                //    canTick = true;
                //}
            }

            if (GameData.getInstance().tipRemain == 0)
            {
                canTick = true;
            }
            else
            {
                canTick = false;
            }



            //reset all first  

            GameObject tContainer = GameObject.Find("container");
            foreach(Transform tlink  in tContainer.transform)
            {
                string tname = tlink.name;
                if(tname.Contains("link"))
                {
                    tlink.GetComponent<SpriteRenderer>().color = Color.clear;
                }

                
            }
            GameData.instance.init();




            for (int ii = 0; ii <= GameData.instance.tipUsed; ii++)
            {
                if (GameData.instance.tipUsed < GameData.instance.dotPoses.Count)
                {
                    //start tip

                    if (GameData.instance.tipRemain > 0)
                    {
                        //if no tip neccessary anymore.Disable tip button to disable waste.
                        //if no tip left,not disable the button. click tip again would ask for buy
                        if (GameData.instance.tipUsed >= GameData.instance.dotPoses.Count - 2)
                        {
                            GameObject.Find("btnTip").GetComponent<Button>().interactable = false;
                        }

                    }

                    //start tip
                    for (int i = 0; i <= GameData.instance.tipUsed; i++)
                    {
                        //set data
                        int tcolorId = i + 1;
                        GameData.instance.paths[tcolorId] = new List<int>();

                        for (int j = 1; j < GameData.instance.dotPoses[i]["v"].Count; j++)
                        {
                            Vector2 lastPos = Vector2.zero;
                            Vector2 tpos = Vector2.zero;

                            lastPos.x = GameData.instance.dotPoses[i]["v"][j - 1]["x"];
                            lastPos.y = GameData.instance.dotPoses[i]["v"][j - 1]["y"];

                            tpos.x = GameData.instance.dotPoses[i]["v"][j]["x"];//
                            tpos.y = GameData.instance.dotPoses[i]["v"][j]["y"];//

                            //set data
                            int _tid = (int)tpos.y * GameData.bsize + (int)tpos.x;
                            GameData.instance.paths[tcolorId].Add(_tid); 
                            GameData.instance.ColorData[_tid] = tcolorId;


                            string tlinkname = "";
                            if (tpos.x == lastPos.x + 1 && tpos.y == lastPos.y)//path is go right
                            {
                                tlinkname = "linkr";
                            }
                            else if (tpos.x == lastPos.x - 1 && tpos.y == lastPos.y)//path is go left
                            {
                                tlinkname = "linkl";
                            }
                            else if (tpos.x == lastPos.x && tpos.y == lastPos.y - 1)//path is go down
                            {
                                tlinkname = "linkd";
                            }
                            else if (tpos.x == lastPos.x && tpos.y == lastPos.y + 1)//path is go up
                            {
                                tlinkname = "linku";
                            }



                            Transform tlink = tContainer.transform.Find(tlinkname + lastPos.x + "_" + lastPos.y);
                            if (tlink) {
                                tlink.GetComponent<SpriteRenderer>().color = GameData.instance.colors[tcolorId];
                            }

                            GameData.instance.linkedLines[tcolorId] = 1;
                        }
                    }


                }
            }
            GameData.instance.tipUsed++;
           
        }

    }
}
