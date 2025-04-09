

using Game.Gameplay.UI;
using Zenject;

namespace Game.Gameplay.Settings
{
    internal class GameplayUserInterfaceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindRacePanelManager();
        }

        private void BindRacePanelManager()
        {
            Container
                .Bind<IRacePanelManagerUI>()
                .To<DefaultRacePanelManagerUI>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
        }
    }
}
