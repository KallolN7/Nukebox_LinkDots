using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nukebox
{
    [CreateAssetMenu(menuName = "GameData", fileName = "GameData")]
    public class GameData : ScriptableObject
    {
        [SerializeField]
        public  int[] totalLevel;
        [SerializeField]
        public  int difficulty;
        [SerializeField]
        public  int linksPerDot;


        #region Get Methods

        /// <summary>
        /// Returns total Levels array
        /// </summary>
        /// <returns></returns>
        public int[] GetTotalLevels()
        {
            return totalLevel;
        }

        /// <summary>
        /// Returns difficulty index
        /// </summary>
        /// <returns></returns>
        public int GetDifficulty()
        {
            return difficulty;
        }

        /// <summary>
        /// Returns links per dot count
        /// </summary>
        /// <returns></returns>
        public int GetLinksPerDot()
        {
            return linksPerDot;
        }

        #endregion
    }
}

