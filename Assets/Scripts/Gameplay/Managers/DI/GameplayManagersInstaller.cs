using Game.Gameplay.Race;
using Game.Gameplay.UI;
using Zenject;

namespace Game.Gameplay.Settings
{

    public class GameplayManagersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindRaceManager();
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
