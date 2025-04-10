
using DG.Tweening;
using Game.Utilities.Logging;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Audio
{
    public class GameplayAudioManager : MonoBehaviour, IGameplayAudioManager
    {
        #region Fields 


        // ToDo : there should be different logic here for assigning an AudioSource
        [Header("Sources")]
        [SerializeField] private AudioSource _mainMusicSource;
        [SerializeField] private AudioSource _secondaryMusicSource;

        [Header("Settings")]
        [SerializeField] private float _stopDurationSFX = 1.5f;

        private GameplayLevelAudioData _audioData;
        private float _secondarySourceInitialVolume;

        private Tween _activeAnimation;

        #endregion


        #region Properties

        #endregion


        #region Methods

        [Inject]
        public void Construct(GameplayLevelAudioData audioData)
        {
            _audioData = audioData;
        }

        public void Initialize()
        {
            // Ensure 2D audio.
            _mainMusicSource.spatialBlend = 0f;
            _secondaryMusicSource.spatialBlend = 0f;
            _secondarySourceInitialVolume = _secondaryMusicSource.volume;
        }

        public void Dispose()
        {
            _activeAnimation?.Kill();
        }

        public void PlayMenuOST(bool loop = true)
        {
            PlayOST(_audioData.PassiveAudioUI, loop);
        }

        public void PlayRaceOST(bool loop = true)
        {
            if (_audioData.RaceAudioCollection == null)
            {
                CustomLogger.LogComponentIsNull(gameObject.name, nameof(_audioData.RaceAudioCollection));
                return;
            }

            int raceTracksCount = _audioData.RaceAudioCollection.Count();
            AudioClip targetClip;

            if (raceTracksCount > 1)
            {
                int randomTrackIndex = Random.Range(0, raceTracksCount);
                targetClip = _audioData.RaceAudioCollection.ElementAt(randomTrackIndex);
            }
            else
            {
                targetClip = _audioData.RaceAudioCollection.FirstOrDefault();
            }

            PlayOST(targetClip, loop);
        }

        public void PlayRaceStartSFX()
        {
            PlaySFX(_audioData.RaceStartSFX, false);
        }

        public void FadeOutSFX()
        {
            if (_stopDurationSFX > 0f)
            {
                _secondaryMusicSource
                    .DOFade(0, _stopDurationSFX)
                    .OnComplete(() => _secondaryMusicSource.Stop());
            }
            else
            {
                StopSFX();
            }
        }

        public void StopSFX()
        {
            _secondaryMusicSource.Stop();
        }

        private void PlayOST(AudioClip OST, bool loop = true)
        {
            if (OST == null)
            {
                CustomLogger.LogWarning($"Can not play OST, it is null | {gameObject.name} ");
                return;
            }

            PlayAudio(_mainMusicSource, OST, loop);
        }

        private void PlaySFX(AudioClip SFX, bool loop = false)
        {
            if (SFX == null)
            {
                CustomLogger.LogWarning($"Can not play SFX, it is null | {gameObject.name} ");
                return;
            }

            _secondaryMusicSource.volume = _secondarySourceInitialVolume;
            PlayAudio(_secondaryMusicSource, SFX, loop);
        }

        private void PlayAudio(AudioSource targetSource, AudioClip targetClip, bool loop)
        {
            targetSource.clip = targetClip;
            targetSource.Play();
            targetSource.loop = loop;
        }

        #endregion
    }
}