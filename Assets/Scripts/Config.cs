using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nukebox
{
    public static class Config
    {
        public static int rowCount;
        public static int[] totalLevel = { 15 };
        public static int difficulty = 0;
        public static int nLink = 0;
        public static int levelPassed = 0;

        public static List<int> levelPass = new List<int>();
        public static int currentLevel = 0;
        public static int bestScore = 0;

        public static int isSoundOn = 0;
        public static int isSfxOn = 0;

        public static List<List<int>> levelStates = new List<List<int>>();

        public static bool isWin = false;
        public static bool isfail = false;

        public static bool isHolding = false;
        public static int pickColor = -1;

        public static int startId = -1;
        public static int lasttx = -1;
        public static int lastty = -1;
        public static List<List<int>> paths;

        public static JSONNode dotPoses;
        public static int[] ColorData;
        public static int[] DotColorData;
        public static Color[] colors;
        public static JSONNode levelData;
        public static int[] linkedLines;
        public static int winLinkCount;
    }
}

