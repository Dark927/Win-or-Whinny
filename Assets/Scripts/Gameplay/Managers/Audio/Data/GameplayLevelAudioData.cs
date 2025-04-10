using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Audio
{
    [CreateAssetMenu(fileName = "GameplayLevelAudioData", menuName = "Game/Gameplay/Audio/Level Audio Data")]
    public class GameplayLevelAudioData : ScriptableObject
    {
        [SerializeField] private AudioClip _passiveAudioUI;
        [SerializeField] private AudioClip _raceStartAudio;
        [SerializeField] private List<AudioClip> _raceAudioCollection;

        public AudioClip PassiveAudioUI => _passiveAudioUI;
        public AudioClip RaceStartSFX => _raceStartAudio;
        public IEnumerable<AudioClip> RaceAudioCollection => _raceAudioCollection;
    }
}
