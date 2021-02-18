using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nukebox
{
    /// <summary>
    /// Controls each visible dots 
    /// </summary>
    public class DotsChildController : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;

        /// <summary>
        ///  Return spriteRenderer attached to this dot child
        /// </summary>
        /// <returns></returns>
        public SpriteRenderer GetSpriteRenderer()
        {
            return spriteRenderer;
        }
    }
}

