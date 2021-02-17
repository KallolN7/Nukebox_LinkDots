using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nukebox
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private LevelButtonController levelButtonPrefab;
        [SerializeField]
        private Transform levelButtonsParentTransform;

        #region Public Methods

        public void PrepareLevelController()
        {
            PopulateLevelButtons();
        }

        public void OpenLevelScreen()
        {

        }

        public void CloseLevelScreen()
        {

        }

        #endregion

        #region Private Methods

        private void PopulateLevelButtons()
        {
            if(levelButtonsParentTransform.childCount > 0)
            {
                for (int i = 0; i < 15; i++)
                {
                    LevelButtonController levelButton = Instantiate(levelButtonPrefab, levelButtonsParentTransform);
                    levelButton.transform.SetParent(levelButtonsParentTransform);
                    levelButton.SetData(i);
                }
            }
        }

        #endregion
    }
}

