using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nukebox
{
    /// <summary>
    /// Controls Level Screen UI. Populates Level buttons on Game start 
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private LevelButtonController levelButtonPrefab;
        [SerializeField]
        private Transform levelButtonsParentTransform;
        [SerializeField]
        private GameObject levelHolder;


        #region Private Methods

        /// <summary>
        /// Populates level buttons 
        /// </summary>
        private void PopulateLevelButtons()
        {
            if(levelButtonsParentTransform.childCount == 0)
            {
                for (int i = 0; i < Config.totalLevel[Config.difficulty]; i++)
                {
                    LevelButtonController levelButton = Instantiate(levelButtonPrefab, levelButtonsParentTransform);
                    levelButton.transform.SetParent(levelButtonsParentTransform);
                    levelButton.SetData(i);
                }
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
            EventManager.AddListener(EventID.Event_OpenLevelScreen, EventOnOpenLevelScreen);
        }

        /// <summary>
        /// Un-subsribing methods from events
        /// </summary>
        private void OnDisable()
        {
            EventManager.RemoveListener(EventID.Event_GameStart, EventOnGameStart);
            EventManager.RemoveListener(EventID.Event_OpenLevelScreen, EventOnOpenLevelScreen);
        }

        /// <summary>
        /// Closes level screen
        /// </summary>
        /// <param name="obj"></param>
        private void EventOnGameStart(object obj)
        {
            levelHolder.SetActive(false);
        }

        /// <summary>
        /// Opens level screen
        /// </summary>
        /// <param name="obj"></param>
        private void EventOnOpenLevelScreen(object obj)
        {
            Debug.Log("LevelController, OpenLevelScreen");
            levelHolder.SetActive(true);
            PopulateLevelButtons();
        }

        #endregion
    }
}

