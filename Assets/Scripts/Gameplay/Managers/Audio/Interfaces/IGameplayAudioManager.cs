
using Game.Settings.GameInitialization;
using System;

namespace Game.Gameplay.Audio
{
    public interface IGameplayAudioManager : IInitializable, IDisposable
    {
        public void PlayMenuOST(bool loop = true);
        public void PlayRaceOST(bool loop = true);
        public void PlayRaceStartSFX();
        public void FadeOutSFX();
        public void StopSFX();
    }
}
