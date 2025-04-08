using UnityEngine;
using Zenject;

namespace Game.Gameplay.Settings
{
    public class GameplayComponentsInstaller : MonoInstaller
    {
        #region Fields 

        [SerializeField] private HorsesSetData _levelInitialHorsesSet;


        #endregion


        #region Methods

        public override void InstallBindings()
        {
            BindHorsesProvider();
        }

        private void BindHorsesProvider()
        {
            Container
                .Bind<HorsesSetData>()
                .FromInstance(_levelInitialHorsesSet)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<IHorsesProvider>()
                .To<HorsesProviderMono>()
                .FromComponentInHierarchy(true)
                .AsSingle()
                .NonLazy();
        }

        #endregion
    }
}
