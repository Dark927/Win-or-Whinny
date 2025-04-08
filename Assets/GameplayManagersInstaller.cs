using Game.Gameplay.UI;
using Zenject;

namespace Game.Gameplay.Settings
{

    public class GameplayManagersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindHorseInfoUIManager();
        }

        private void BindHorseInfoUIManager()
        {
            Container
                .Bind<IHorseInfoManagerUI>()
                .To<HorseInfoListManagerUI>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }
    }
}
