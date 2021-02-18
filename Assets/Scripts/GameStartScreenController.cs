using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nukebox
{
    public class GameStartScreenController : MonoBehaviour
    {
        [SerializeField]
        private GameObject holder;
        
        /// <summary>
        /// Closes the game start screen
        /// </summary>
        public void CloseGameStartScreen()
        {
            holder.SetActive(false);
        }
    }
}

