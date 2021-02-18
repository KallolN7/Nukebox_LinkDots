using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System; 

namespace nukebox
{
    /// <summary>
    /// Controls all game related functionalities. Loads and Parses data from json files
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private int rowCount;
        private int[] totalLevel = { 15 };
        private int difficulty = 0;
        private List<int> levelPass;
        private int currentLevel = 0;
        private int bestScore = 0;
        private List<List<int>> levelStates = new List<List<int>>();
        private List<List<int>> paths;
        private JSONNode dotPoses;
        private int[] ColorData;
        private int[] DotColorData;
        private Color[] colors;
        private JSONNode levelData;
        private int[] linkedLines;
        private int winLinkCount;
        private TextAsset datas;
        private Dictionary<string, Dictionary<string, string>> data;


        #region Private Methods

        /// <summary>
        /// Triggering OpenLevelScreen event at Start
        /// </summary>
        private void Start()
        {
            EventManager.TriggerEvent(EventID.Event_OpenLevelScreen);
        }

        /// <summary>
        /// Inits GameManager on start of every level
        /// </summary>
        private void PrepareGameManager()
        {
            ResetData();
            ParseData();
            initScoreAnLevels();
            EventManager.TriggerEvent(EventID.Event_GameStart);
        }

        /// <summary>
        /// Init sore and levels
        /// </summary>
        private void initScoreAnLevels()
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

            levelPass = new List<int>();
            for (int i = 0; i < totalLevel.Length; i++)
            {
                int tDiffLevelPassed = PlayerPrefs.GetInt("levelPassed" + i);
                levelPass.Add(tDiffLevelPassed);

            }

            bestScore = allScore;

        }

        /// <summary>
        /// Sets Level progress and triggers GameOver event
        /// </summary>
        /// 
        private void OnGameWin()
        {
            levelStates[difficulty][currentLevel] = 1;
            PlayerPrefs.SetInt("linkdot_" + difficulty + "_" + currentLevel, 1);
        }

        /// <summary>
        /// Updates level number on game win
        /// </summary>
        private void updateLevel()
        {
            if (Config.currentLevel < Config.totalLevel[Config.difficulty] - 1)
            {
                currentLevel++;
                Config.currentLevel = currentLevel;

                if (currentLevel >= levelPass[difficulty])
                {
                    PlayerPrefs.SetInt("levelPassed" + difficulty, currentLevel);
                    levelPass[difficulty] = currentLevel + 1;
                }
            }
            else
            {
                currentLevel = 0;
                Config.currentLevel = currentLevel;
                PlayerPrefs.SetInt("levelPassed" + difficulty, currentLevel);
            }

            Debug.Log("GameManager, SetLevel, current Level = " + currentLevel);
        }

        /// <summary>
        /// Updates score on Game Over
        /// </summary>
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

        #endregion

        #region GameData


        /// <summary>
        /// loading File from StreamingAssets folder and returning jsonOnject 
        /// </summary>
        /// <param name="dataName"></param>
        /// <returns></returns>
        private string[] GetData()
        {
            string path = Application.streamingAssetsPath + "/LevelData.json";
            string[] lines = new string[0];
            string jsonString = File.ReadAllText(path);
            lines = jsonString.Split('\n');

            return lines;
        }


        /// <summary>
        /// Parsing data based on current level. Setting data to Config variables
        /// </summary>
        public void ParseData()
        {
            currentLevel = PlayerPrefs.GetInt("levelPassed" + difficulty, 0);
            string jsonString = GetData()[currentLevel];
             levelData = JSONArray.Parse(jsonString);


            if (levelData == null)
            {
                Debug.LogError("GameManager, ParseData, levelData is null");
                return;
            }
            else
            {
                Debug.Log("GameManager, ParseData" + levelData.ToString());
            }

            string rowString = levelData["r"];
            rowCount = Convert.ToInt32(rowString);

            colors = new Color[] { Color.clear, Color.red, Color.blue, Color.green, new Color(255f / 255f, 167f / 255f, 11f / 255f), Color.magenta}; 

            ColorData = new int[rowCount * rowCount];
            DotColorData = new int[rowCount * rowCount];

            dotPoses = levelData["l"];

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

            Config.SetData(rowCount, currentLevel, bestScore, levelStates, paths, dotPoses, ColorData, DotColorData, colors, linkedLines, winLinkCount);

            Debug.Log("GameManager, ParseData, current Level = " + currentLevel + " | rowCount= " + rowCount);
        }


        /// <summary>
        /// Resets data on game start
        /// </summary>
        public void ResetData()
        {
            Config.ResetConfig();

            levelStates.Clear();

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

        #region Events

        /// <summary>
        /// Subsribing methods to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.AddListener(EventID.Event_ParseData, EventOnParseData);
            EventManager.AddListener(EventID.Event_GameOver, EventOnGameOver);
        }

        /// <summary>
        /// Un-subsribing methods from events
        /// </summary>
        private void OnDisable()
        {
            EventManager.RemoveListener(EventID.Event_ParseData, EventOnParseData);
            EventManager.RemoveListener(EventID.Event_GameOver, EventOnGameOver);
        }

        private void EventOnParseData(object obj)
        {
            PrepareGameManager();
        }

        private void EventOnGameOver(object obj)
        {
            OnGameWin();
            UpdateScore();

        }

        #endregion

    }
}

