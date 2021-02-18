using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace nukebox
{
    /// <summary>
    /// Spawns dots based on level data
    /// </summary>
    public class DotsLinkController : MonoBehaviour
    {

        [SerializeField]
        private DotsController dotsPrefab;
        [SerializeField]
        private DotsChildController dotsChildPrefab;
        [SerializeField]
        private SpriteRenderer linkPrefab;
        [SerializeField]
        private SpriteRenderer dotsBox;
        [SerializeField]
        private Transform dotsHolderTransform;

        List<DotsController> dotsList = new List<DotsController>();
        List<SpriteRenderer> linksList = new List<SpriteRenderer>();


        #region Private Methods

        /// <summary>
        /// Resets dots and links and spawns new dosts and links based on level data 
        /// </summary>
        private void PrePareDots()
        {
            ResetDots();
            PopulateInvisibleDotsAndLinks();
            PopulateVisibleDotsAndLinks();
        }

        /// <summary>
        /// Resets all dots and links
        /// </summary>
        private void ResetDots()
        {
            if (dotsList.Count >= 1)
            {
                for (int i = 0; i < dotsList.Count; i++)
                {
                    Destroy(dotsList[i].gameObject);
                }
            }

            if (linksList.Count >= 1)
            {
                for (int i = 0; i < linksList.Count; i++)
                {
                    Destroy(linksList[i].gameObject);
                }
            }

            linksList.Clear();
            dotsList.Clear();
        }

        /// <summary>
        /// Populates new invisible dots and links throughout the grid
        /// </summary>
        private void PopulateInvisibleDotsAndLinks()
        {
            float gridW = dotsBox.sprite.bounds.size.x / Config.rowCount;
            float offsetx = -1 * gridW * Config.rowCount / 2 + gridW / 2;
            float offsety = offsetx;


            for (int i = 0; i < Config.rowCount * Config.rowCount; i++)
            {
                int tx = Mathf.FloorToInt(i % Config.rowCount);
                int ty = Mathf.FloorToInt(i / Config.rowCount);
                DotsController dot = Instantiate(dotsPrefab, dotsHolderTransform);  //Spawning each invisible and interactive dots 
                float tscale = gridW / dot.GetSpriteRenderer().bounds.size.x;
                dot.transform.localScale *= tscale;
                dot.transform.localPosition = new Vector2(dotsHolderTransform.localPosition.x + gridW * tx + offsetx, dotsHolderTransform.localPosition.y + gridW * ty + offsety);
                dot.GetComponent<SpriteRenderer>().sortingOrder = 1;
                dot.name = "bg" + tx + "_" + ty;
                dot.GetSpriteRenderer().color = Color.clear;
                dotsList.Add(dot);
                Config.dotsDict.Add(dot.name, dot);
                dot.SetData(tx, ty); //Setting data to each dot

                Config.ColorData[i] = 0;
                Config.DotColorData[i] = 0;


                int[] rotation = new int[] { 0, 90, 180, 270 };
                for (int j = 0; j < Config.linksPerDot; j++)
                {
                    SpriteRenderer link = Instantiate(linkPrefab, dotsHolderTransform); //Spawning each links per dots

                    link.transform.localPosition = dot.transform.localPosition;
                    link.transform.localScale = dot.transform.localScale;
                    link.transform.localEulerAngles = new Vector3(0, 0, rotation[j]);
                    link.color = Color.clear;
                    link.sortingOrder = 2;
                    linksList.Add(link);
                    switch (j)
                    {
                        case 0://right
                            Config.linDict.Add("linkr" + tx + "_" + ty, link);
                            break;
                        case 1://up
                            Config.linDict.Add("linku" + tx + "_" + ty, link);
                            break;
                        case 2://left
                            Config.linDict.Add("linkl" + tx + "_" + ty, link);
                            break;
                        case 3://down
                            Config.linDict.Add("linkd" + tx + "_" + ty, link);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Populates all visible dots based on level data
        /// </summary>
        private void PopulateVisibleDotsAndLinks()
        {

            int colorIndex = 1;//because 0 is no color

            for (int i = 0; i < Config.dotPoses.Count; i++)
            {
                string[] pos = new string[2];
                pos[0] = Config.dotPoses[i]["v"][0]["x"];
                pos[1] = Config.dotPoses[i]["v"][0]["y"];

                if (pos[0] == null || pos[0] == "")
                    pos[0] = "0";
                if (pos[1] == null || pos[1] == "")
                    pos[1] = "0";

                int tx = int.Parse(pos[0]);
                int ty = int.Parse(pos[1]);

                int tindex = ty * Config.rowCount + tx;

                DotsChildController dot = Instantiate(dotsChildPrefab, dotsList[tindex].transform); //Spawning each visible  dots 

                dot.GetSpriteRenderer().sortingOrder = 3;
                dot.GetSpriteRenderer().color = Config.colors[i + 1];
                dot.name = "dot";

                Config.DotColorData[tindex] = i + 1;

                int tcount = Config.dotPoses[i]["v"].Count;
                pos[0] = Config.dotPoses[i]["v"][tcount - 1]["x"];
                pos[1] = Config.dotPoses[i]["v"][tcount - 1]["y"];

                if (pos[0] == null || pos[0] == "") pos[0] = "0";
                if (pos[1] == null || pos[1] == "") pos[1] = "0";

                tx = int.Parse(pos[0]);
                ty = int.Parse(pos[1]);

                tindex = ty * Config.rowCount + tx;

                dot = Instantiate(dotsChildPrefab, dotsList[tindex].transform); //Spawning each visible  dots 
                dot.GetSpriteRenderer().sortingOrder = 3;
                dot.GetSpriteRenderer().color = Config.colors[i + 1];
                dot.name = "dot";


                Config.DotColorData[tindex] = i + 1;
            }
        }
        #endregion


        #region Events

        /// <summary>
        /// Subsribing methods to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.AddListener(EventID.Event_GameStart, EventOnGameStart);
        }

        /// <summary>
        /// Un-subsribing methods from events
        /// </summary>
        private void OnDisable()
        {
            EventManager.RemoveListener(EventID.Event_GameStart, EventOnGameStart);
        }

        private void EventOnGameStart(object obj)
        {
            Debug.Log("DotsLinkController, EventOnGameStart");
            PrePareDots();
        }

        #endregion

    }
}

