
using UnityEngine;


namespace nukebox
{
    /// <summary>
    /// Open Victory screen on Game Over Event
    /// </summary>
    public class VictoryScreenController : MonoBehaviour
    {
        [SerializeField]
        private Transform holder;


        #region OnClick Methods

        /// <summary>
        /// Called on clicking Continue Button
        /// </summary>
        public void OnContinue()
        {
            EventManager.TriggerEvent(EventID.Event_OpenLevelScreen);
            holder.gameObject.SetActive(false);
        }

        #endregion

        #region Events

        /// <summary>
        /// Subsribing methods to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.AddListener(EventID.Event_GameOver, EventOnGameOver);
        }

        /// <summary>
        /// Un-subsribing methods from events
        /// </summary>
        private void OnDisable()
        {
            EventManager.RemoveListener(EventID.Event_GameOver, EventOnGameOver);
        }


        private void EventOnGameOver(object obj)
        {
            holder.gameObject.SetActive(true);
        }


        #endregion

    }
}

