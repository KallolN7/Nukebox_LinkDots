
using UnityEngine;
using TMPro;

namespace nukebox
{
    public class LevelButtonController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI levelText;

        private int level;

        #region Public Methods

        public void OnClickButton()
        {
            //GameData.getInstance().cLevel = level;
            // all_game.transform.parent.GetComponent<MainScript>().init();
            //GameObject.Find("linkdot").GetComponent<LinkDot>().init();
        }

        public void SetData(int level)
        {
            this.level = level;
            levelText.text = level.ToString();
        }

        #endregion

        #region Private Methods
        #endregion
    }
}

