using Game.Gameplay.Audio;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Settings
{
    [CreateAssetMenu(fileName = "GameplayLevelDataInstaller", menuName = "Installers/GameplayLevelDataInstaller")]
    public class GameplayLevelDataInstaller : ScriptableObjectInstaller<GameplayLevelDataInstaller>
    {
        [SerializeField] private GameplayLevelAudioData _audioData;

        public override void InstallBindings()
        {
            Container
                .Bind<GameplayLevelAudioData>()
                .FromInstance(_audioData)
                .AsSingle();
        }
    }
}