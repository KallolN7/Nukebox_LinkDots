
using System.Collections.Generic;
using UnityEngine;

namespace nukebox
{
    /// <summary>
    /// Acts as the bridge class between all classes. Transfers and stores temporary data. 
    /// </summary>
    public static class Config
    {
        #region Temporary Variables

        public static int[] totalLevel = { 15 };
        public static int difficulty = 0;
        public static int linksPerDot = 4;
        public static int rowCount;
        public static int nLink = 0;
        public static int levelPassed = 0;
        public static int currentLevel = 0;
        public static int bestScore = 0;
        public static List<List<int>> levelStates = new List<List<int>>();
        public static bool isWin = false;
        public static bool isfail = false;
        public static bool isHolding = false;
        public static int pickColor = -1;
        public static int startId = -1;
        public static int lasttx = -1;
        public static int lastty = -1;
        public static List<List<int>> paths = new List<List<int>>();
        public static JSONNode dotPoses;
        public static int[] ColorData;
        public static int[] DotColorData;
        public static Color[] colors;
        public static int[] linkedLines;
        public static int winLinkCount;

        public static Dictionary<string, DotsController> dotsDict = new Dictionary<string, DotsController>();
        public static Dictionary<string, SpriteRenderer> linDict = new Dictionary<string, SpriteRenderer>();

        #endregion

        #region Methods

        /// <summary>
        /// Sets All temporary data on game start 
        /// </summary>
        /// <param name="rowCount1"></param>
        /// <param name="currentLevel1"></param>
        /// <param name="bestScore1"></param>
        /// <param name="levelStates1"></param>
        /// <param name="paths1"></param>
        /// <param name="dotPoses1"></param>
        /// <param name="ColorData1"></param>
        /// <param name="DotColorData1"></param>
        /// <param name="colors1"></param>
        /// <param name="linkedLines1"></param>
        /// <param name="winLinkCount1"></param>
        public static void SetData(int rowCount1, int currentLevel1, int bestScore1, List<List<int>> levelStates1, 
                                                   List<List<int>> paths1, JSONNode dotPoses1,
                                                   int[] ColorData1, int[] DotColorData1, Color[] colors1,  int[] linkedLines1, int winLinkCount1, int[] totalLevel1, int difficulty1, int linksPerDot1)

        {
            rowCount = rowCount1;
            currentLevel = currentLevel1;
            bestScore = bestScore1;
            levelStates = levelStates1;
            paths = paths1;
            dotPoses = dotPoses1;
            ColorData = ColorData1;
            DotColorData = DotColorData1;
            colors = colors1;
            linkedLines = linkedLines1;
            winLinkCount = winLinkCount1;
            totalLevel = totalLevel1;
            difficulty = difficulty1;
            linksPerDot = linksPerDot1;
        }


        /// <summary>
        /// Resets all temopary datas on start of each level
        /// </summary>
        public static void ResetConfig()
        {
            dotsDict.Clear();
            linDict.Clear();
            rowCount = 0;
            nLink = 0;
            levelPassed = 0;
            currentLevel = 0;
            bestScore = 0;
            levelStates.Clear();
            isWin = false;
            isfail = false;
            isHolding = false;
            pickColor = -1;
            startId = -1;
            lasttx = -1;
            lastty = -1;
            paths.Clear();
            dotPoses = null;
            ColorData = null;
            DotColorData = null;
            colors = null;
            linkedLines = null;
            winLinkCount = 0;
         }

        #endregion
    }
}

