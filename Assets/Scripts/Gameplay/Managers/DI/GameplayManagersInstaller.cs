using Game.Gameplay.Race;
using Game.Gameplay.UI;
using Zenject;

namespace Game.Gameplay.Settings
{

    public class GameplayManagersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameManager();
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
