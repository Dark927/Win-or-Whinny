using Game.Gameplay.Audio;
using Game.Gameplay.Race;
using Zenject;

namespace Game.Gameplay.Settings
{

    public class GameplayManagersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameManager();
            BindAudioManager();
            BindRaceManager();
        }

        private void BindGameManager()
        {
            Container
                .Bind<GameManager>()
                .FromComponentInHierarchy(true)
                .AsSingle()
                .NonLazy();
        }

        private void BindAudioManager()
        {
            Container
                .Bind<IGameplayAudioManager>()
                .To<GameplayAudioManager>()
                .FromComponentInHierarchy(true)
                .AsSingle()
                .NonLazy();
        }

        private void BindRaceManager()
        {
            Container
                .Bind<IHorseRaceManager>()
                .To<DefaultHorseRaceManager>()
                .FromComponentInHierarchy(true)
                .AsSingle()
                .NonLazy();
        }
    }
}
