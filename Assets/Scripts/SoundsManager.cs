using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nukebox
{

    /// <summary>
    /// Controls all sounds based on events
    /// </summary>
    public class SoundsManager : MonoBehaviour
    {
        [SerializeField]
        private AudioSource sfxAudioSource;
        [SerializeField]
        private AudioSource musicAudioSource;
        [SerializeField]
        private SoundsHolder soundsHolder;

        private void Start()
        {
            PlayBgMusic();
        }

        /// <summary>
        /// Plays sfx sounds based on soundType enum
        /// </summary>
        /// <param name="soundType"></param>
        private void PlaySfxSound(SoundType soundType)
        {
            AudioClip clip;
            switch (soundType)
            {
                case SoundType.Sfx_GameStart:
                    clip = soundsHolder.GetSfxClip(0);
                    break;
                case SoundType.Sfx_GameOver:
                    clip = soundsHolder.GetSfxClip(2);
                    break;
                case SoundType.sfx_Addpath:
                    clip = soundsHolder.GetSfxClip(1);
                    break;
                default:
                    clip = soundsHolder.GetSfxClip((int)soundType);
                    break;
            }
            sfxAudioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Plays background music 
        /// </summary>
        private void PlayBgMusic()
        {
            musicAudioSource.clip = soundsHolder.GetBGMusicClip();
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }


        #region Events

        /// <summary>
        /// Subsribing methods to events
        /// </summary>
        private void OnEnable()
        {
            EventManager.AddListener(EventID.Event_GameStart, EventOnGameStart);
            EventManager.AddListener(EventID.Event_GameOver, EventOnGameOver);
            EventManager.AddListener(EventID.Event_AddPath, EventOnAddPath);
        }

        /// <summary>
        /// Un-subsribing methods from events
        /// </summary>
        private void OnDisable()
        {
            EventManager.RemoveListener(EventID.Event_GameStart, EventOnGameStart);
            EventManager.RemoveListener(EventID.Event_GameOver, EventOnGameOver);
            EventManager.RemoveListener(EventID.Event_AddPath, EventOnAddPath);
        }

        /// <summary>
        ///  Methos subsribed to Event_OnGameStart event
        /// </summary>
        /// <param name="obj"></param>
        private void EventOnGameStart(object obj)
        {
            PlaySfxSound(SoundType.Sfx_GameStart);
        }

        /// <summary>
        ///  Methos subsribed to Event_GameOver event
        /// </summary>
        /// <param name="obj"></param>
        private void EventOnGameOver(object obj)
        {
            PlaySfxSound(SoundType.Sfx_GameOver);
        }

        /// <summary>
        ///  Methos subsribed to Event_AddPath event
        /// </summary>
        /// <param name="obj"></param>
        private void EventOnAddPath(object obj)
        {
            PlaySfxSound(SoundType.sfx_Addpath);
        }

        #endregion
    }


    public enum SoundType
    {
        Background_Music,
        Sfx_GameStart,
        sfx_Addpath,
        Sfx_GameOver
    }
}

