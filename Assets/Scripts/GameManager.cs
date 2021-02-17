using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace nukebox
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField]
        private DotsLinkController dotsLinkController;


        private int rowCount;
        private int[] totalLevel = { 15 };
        private int difficulty = 0;
        private int nLink = 0; //check in game.When nlink = 0.All the lines linked,so win.
        private int levelPassed = 0;//how much level you passed

        private List<int> levelPass;
        private int currentLevel = 0;//currect level
        private int bestScore = 0;//bestscore for level

        private int isSoundOn = 0;//whether game music is on
        private int isSfxOn = 0;//whether the game sound effect is on

        private List<List<int>> levelStates = new List<List<int>>();

        private bool isWin = false;//check if win
        private bool isfail = false;//whether the game failed

        private bool isHolding = false;
        private int pickColor = -1;

        private int startId = -1;
        private int lasttx = -1;
        private int lastty = -1;
        private List<List<int>> paths;

        private JSONNode dotPoses;
        private int[] ColorData;
        private int[] DotColorData;
        private Color[] colors;
        private JSONNode levelData;
        private int[] linkedLines;
        private int winLinkCount;

        public static GameManager instance;

        private void Awake()
        {
            instance = this;
        }


        public void PrepareGameManager()
        {
            ParseData(currentLevel);
            initGameManager();
            InitMainScript();
        }

        #region GameManager


        /// <summary>
        /// Init this controller instance.only once.
        /// </summary>
        public void initGameManager()
        {
 
            int allScore = 0;


            levelStates = new List<List<int>>();
            for (int i = 0; i < totalLevel.Length; i++)
            {
                levelStates.Add(new List<int>());
                for (int j = 0; j < totalLevel[i]; j++)
                {

                    int tState = PlayerPrefs.GetInt("linkdot_" + i + "_" + j, 0);
                    levelStates[i].Add(tState);
                    levelStates[i][j] = tState;


                    if (tState == 1)
                    {
                        allScore++;
                    }
                }
            }

            bestScore = allScore;

            Debug.Log("bestScore is:" + allScore);

            levelPass = new List<int>();
            for (int i = 0; i < totalLevel.Length; i++)
            {
                int tDiffLevelPassed = PlayerPrefs.GetInt("levelPassed" + i);
                levelPass.Add(tDiffLevelPassed);

            }

            bestScore = allScore;

        }

        #endregion

        #region GameData


        public void ParseData(int currentLevel)
        {
            string tData = Datas.CreateInstance<Datas>().getData("linkdots")[currentLevel];//level
            levelData = JSONArray.Parse(tData);

            rowCount = int.Parse(levelData["r"]);

            colors = new Color[] { Color.clear, Color.red, Color.blue, Color.magenta, Color.cyan, Color.green, Color.yellow, Color.gray, Color.white, Color.black, new Color(252f / 255f, 157f / 255f, 154f / 255f), new Color(249f / 255f, 205f / 255f, 173f / 255f), new Color(200f / 255f, 200f / 255f, 169f / 255f) };
            ColorData = new int[rowCount * rowCount];
            DotColorData = new int[rowCount * rowCount];

            dotPoses = levelData["l"];//this is actually is pathes between 2 dots

            paths = new List<List<int>>();
            List<int> tpath0 = new List<int>();

            paths.Add(tpath0);

            for (int i = 0; i < dotPoses.Count; i++)
            {
                List<int> tpath = new List<int>();
                paths.Add(tpath);

                int tindex = dotPoses[i]["v"][0]["y"] * rowCount + dotPoses[i]["v"][0]["x"];
                DotColorData[tindex] = i + 1;

                int tcount = dotPoses[i]["v"].Count;
                int _tx = dotPoses[i]["v"][tcount - 1]["x"];
                int _ty = dotPoses[i]["v"][tcount - 1]["y"];
                tindex = _ty * rowCount + _tx;
                DotColorData[tindex] = i + 1;

            }
            winLinkCount = dotPoses.Count;
            linkedLines = new int[paths.Count + 1];
        }


        /// <summary>
        /// Always uses for initial or reset to start a new level.
        /// </summary>
        public void ResetData()
        {
            isWin = false;
            isfail = false;

            levelStates.Clear();

            //load levelState，check which level passed
            for (int i = 0; i < totalLevel.Length; i++)
            {
                List<int> tStates = new List<int>();
                for (int j = 0; j < totalLevel[i]; j++)
                {
                    int tState = PlayerPrefs.GetInt("linkdot_" + i + "_" + j);
                    tStates.Add(tState);

                }
                levelStates.Add(tStates);
            }
        }


        #endregion

        #region MainScript



        private void InitMainScript()
        {
            dotsLinkController.PrePareDots();
        }

        //handler event
        public void OnRetryClick()
        {
            dotsLinkController.PrePareDots();
        }

        

        /// <summary>
        /// when game wins.
        /// </summary>
        /// 
        public void OnGameWin()
        {
            levelStates[difficulty][currentLevel] = 1;
            PlayerPrefs.SetInt("linkdot_" + difficulty + "_" + currentLevel, 1);


            if (currentLevel >= levelPass[difficulty])
            {
                PlayerPrefs.SetInt("levelPassed" + difficulty, instance.currentLevel + 1);
                levelPass[difficulty] = currentLevel + 1;
            }

            UpdateScore();

            //PrepareVictoryScreen();
        }

        public void SetLevel(int level)
        {
            currentLevel = level;
            Config.currentLevel = level;
        }

        private void UpdateScore()
        {
            int totalScore = 0;
            for (int i = 0; i < totalLevel.Length; i++)
            {
                for (int j = 0; j < totalLevel[i]; j++)
                {
                    if (levelStates[i][j] == 1)
                    {
                        totalScore++;
                    }
                }
            }

            bestScore = totalScore;
        }

        public bool GetIsWin()
        {
            return isWin;
        }

        public bool GetIsHolding()
        {
            return isHolding;
        }

        public int GetPickColor()
        {
            return pickColor;
        }

        public void SetPickColor(int id)
        {
            pickColor = id;
        }

        public int GetCurrentLevel()
        {
            return currentLevel;
        }

        public DotsLinkController GetDotLinkController()
        {
            return dotsLinkController;
        }
        #endregion

        #region Datas

        private TextAsset datas;
        private Dictionary<string, Dictionary<string, string>> data;

        public string[] getData(string dataName)
        {
            datas = Resources.Load<TextAsset>(dataName + "/" + difficulty);
            string[] lines = new string[0];
            data = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> loc = new Dictionary<string, string>();
            lines = datas.text.Split('\n');

            return lines;
        }

        #endregion

    }
}

