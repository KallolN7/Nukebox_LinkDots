using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Hitcode_linkDots;

namespace nukebox
{
    public class DotsLinkController : MonoBehaviour
    {

        [SerializeField]
        private DotsController dotsPrefab;
        [SerializeField]
        private SpriteRenderer linkPrefab;
        [SerializeField]
        private SpriteRenderer dotsBox;
        [SerializeField]
        private Transform dotsHolderTransform;

        List<DotsController> dotsList = new List<DotsController>();
        List<SpriteRenderer> linksList = new List<SpriteRenderer>();

        public void PrePareDots()
        {
            ResetDots();
            PopulateDots();
            SetColorToDotsAndLinks();
        }

       private void ResetDots()
        {
            if(dotsList.Count >= 1)
            {
                for (int i = 0; i < dotsList.Count; i++)
                {
                    Destroy(dotsList[i].gameObject);
                }
            }

            if(linksList.Count >= 1)
            {
                for (int i = 0; i < linksList.Count; i++)
                {
                    Destroy(linksList[i].gameObject);
                }
            }

            linksList.Clear();
            dotsList.Clear();
        }

        private void PopulateDots()
        {
           // GameObject tBg = Resources.Load("linkdots/square") as GameObject;

            float gridW = dotsBox.sprite.bounds.size.x / Config.rowCount;

            //GameObject tCircle = Resources.Load("linkdots/dot") as GameObject;
            //GameObject tLink = Resources.Load("linkdots/link") as GameObject;


            float offsetx = -1 * gridW * Config.rowCount / 2 + gridW / 2;
            float offsety = offsetx;

           // List<GameObject> tbgs = new List<GameObject>();

            for (int i = 0; i < Config.rowCount * Config.rowCount; i++)
            {
                int tx = Mathf.FloorToInt(i % Config.rowCount);
                int ty = Mathf.FloorToInt(i / Config.rowCount);
                DotsController dot = Instantiate(dotsPrefab, dotsHolderTransform);
                float tscale = gridW / dot.GetSpriteRenderer().bounds.size.x;
                dot.transform.localScale *= tscale;
                dot.transform.localPosition = new Vector2(dotsHolderTransform.localPosition.x + gridW * tx + offsetx, dotsHolderTransform.localPosition.y + gridW * ty + offsety);
                dot.GetComponent<SpriteRenderer>().sortingOrder = 1;
                dot.name = "bg" + tx + "_" + ty;
                dot.GetSpriteRenderer().color = Color.clear;
                dotsList.Add(dot);



                //dot.gameObject.AddComponent<BoxCollider>();
                //dot.gameObject.AddComponent<DotsController>();

                dot.SetData(tx, ty);

               Config.ColorData[i] = 0;//no color
                Config.DotColorData[i] = 0;//no color


                int[] rotation = new int[] { 0, 90, 180, 270 };
                for (int j = 0; j < 4; j++)
                {//add 4 link lines to each square
                    SpriteRenderer link = Instantiate(linkPrefab, dotsHolderTransform);

                    link.transform.localPosition = dot.transform.localPosition;
                    link.transform.localScale = dot.transform.localScale;
                    link.transform.localEulerAngles = new Vector3(0, 0, rotation[j]);
                    link.color = Color.clear;
                    link.sortingOrder = 2;
                    linksList.Add(link);
                }
            }
        }


        private void SetColorToDotsAndLinks()
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

                DotsController dot = Instantiate(dotsPrefab, dotsList[tindex].transform);

                dot.GetSpriteRenderer().sortingOrder = 3;
                dot.GetSpriteRenderer().color =Config.colors[i + 1];
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
                dot = Instantiate(dotsPrefab, dotsList[tindex].transform);


                dot.GetSpriteRenderer().sortingOrder = 3;
                dot.GetSpriteRenderer().color = Config.colors[i + 1];

                dot.name = "dot";

               // AnimateDots(dot, colorIndex);

                Config.DotColorData[tindex] = i + 1;
            }
        }

        private void AnimateDots(DotsController dot, int colorIndex)
        {
            dot.transform.localScale *= .9f;
            Vector3 tcScale = dot.transform.localScale;
            dot.transform.localScale = Vector3.zero;
            float tdelay = colorIndex * .1f;

            dot.transform.DOScale(tcScale, 1).SetDelay(tdelay).SetEase(Ease.OutBounce);
        }

        public void clear()
        {
            if (dotsHolderTransform != null)
            {
                foreach (Transform tobj in dotsHolderTransform)
                {
                    tobj.DOScale(Vector3.zero, .2f);
                }
            }
        }
    }
}

