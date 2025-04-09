

using Game.Gameplay.UI;
using Zenject;

namespace Game.Gameplay.Settings
{
    internal class GameplayUserInterfaceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindRaceStartPanel();
        }

        private void BindRaceStartPanel()
        {
            Container
                .Bind<IRaceStartPanelUI>()
                .To<DefaultRaceStartPanelUI>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }
    }
}
