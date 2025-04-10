
using Game.Settings.GameInitialization;
using UnityEngine;

namespace Game.Gameplay.Entities
{
    [RequireComponent(typeof(AudioSource))]
    public class HorseAudioHandler : MonoBehaviour, IInitializable
    {
        [SerializeField] private AudioClip _footStepSFX;

        private AudioSource _audioSource;

        public void Initialize()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayFootStepSFX()
        {
            _audioSource.PlayOneShot(_footStepSFX);
        }
    }
}
