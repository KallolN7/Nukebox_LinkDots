using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nukebox
{
    [CreateAssetMenu(fileName = "SoundsHolder", menuName = "SoundsHolder")]
    public class SoundsHolder : ScriptableObject
    {
        [SerializeField]
        private AudioClip BgMusic;
        [SerializeField]
        private AudioClip[] sfxSounds;

        public AudioClip GetBGMusicClip()
        {
            return BgMusic;
        }

        public AudioClip GetSfxClip(int id)
        {
            return sfxSounds[id];
        }

    }
}

