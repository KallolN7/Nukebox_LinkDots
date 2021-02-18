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
                case SoundType.sfx_GAmeStart:
                    clip = soundsHolder.GetSfxClip(1);
                    break;
                case SoundType.Sfx_GameOver:
                    clip = soundsHolder.GetSfxClip(2);
                    break;
                case SoundType.Sfx_FoodConsume:
                    clip = soundsHolder.GetSfxClip(0);
                    break;
                case SoundType.Sfx_TileConsume:
                    clip = soundsHolder.GetSfxClip(4);
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


        #region EventsHandler

        /// <summary>
        /// Subsribing methods to events
        /// </summary>
        private void OnEnable()
        {
            //EventManager.AddListener(EventID.Event_OnGameStart, EventOnGameStart);
            //EventManager.AddListener(EventID.Event_OnGameOver, EventOnGameOver);
            //EventManager.AddListener(EventID.Event_OnFoodConsumed, EventOnFoodConsumed);
            //EventManager.AddListener(EventID.Event_OnTileConsumed, EventOnTileConsumed);
        }

        /// <summary>
        /// Un-subsribing methods from events
        /// </summary>
        private void OnDisable()
        {
            //EventManager.RemoveListener(EventID.Event_OnGameStart, EventOnGameStart);
            //EventManager.RemoveListener(EventID.Event_OnGameOver, EventOnGameOver);
            //EventManager.RemoveListener(EventID.Event_OnFoodConsumed, EventOnFoodConsumed);
            //EventManager.RemoveListener(EventID.Event_OnTileConsumed, EventOnTileConsumed);
        }

        /// <summary>
        ///  Methos subsribed to Event_OnGameStart event
        /// </summary>
        /// <param name="obj"></param>
        private void EventOnGameStart(object obj)
        {
            PlaySfxSound(SoundType.sfx_GAmeStart);
        }

        /// <summary>
        /// Methos subsribed to Event_OnGameOver event
        /// </summary>
        /// <param name="obj"></param>
        private void EventOnGameOver(object obj)
        {
            PlaySfxSound(SoundType.Sfx_GameOver);
        }

        /// <summary>
        /// Methos subsribed to Event_FoodTimerUp event
        /// </summary>
        /// <param name="obj"></param>
        private void EventOnFoodConsumed(object obj)
        {
            PlaySfxSound(SoundType.Sfx_FoodConsume);
        }

        /// <summary>
        /// Methos subsribed to Event_OnTileConsumed event
        /// </summary>
        /// <param name="obj"></param>
        private void EventOnTileConsumed(object obj)
        {
            PlaySfxSound(SoundType.Sfx_TileConsume); 
        }

        #endregion
    }





    public enum SoundType
    {
        Background_Music,
        Sfx_FoodConsume,
        sfx_GAmeStart,
        Sfx_GameOver,
        Sfx_TileConsume,
        Sfx_BoundaryHit
    }
}

