
using UnityEngine;
using TMPro;

namespace nukebox
{
    /// <summary>
    /// Controls individual level buttons
    /// </summary>
    public class LevelButtonController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI levelText;

        private int level;


        #region Public Methods

        /// <summary>
        /// On Click Contne method
        /// </summary>
        public void OnClickButton()
        {
            PlayerPrefs.SetInt("levelPassed" + Config.difficulty, level);
            EventManager.TriggerEvent(EventID.Event_ParseData);
        }

        /// <summary>
        /// Sets Data to this level button on spawning
        /// </summary>
        /// <param name="level"></param>
        public void SetData(int level)
        {
            this.level = level;
            levelText.text = (level + 1).ToString();
        }

        #endregion

    }
}

