using UnityEngine;
using Zenject;

namespace Game.Settings.SceneManagement
{
    [CreateAssetMenu(fileName = "NewSceneManagementInstallerSO", menuName = "Installers/Scene Management Installer SO")]
    public class SceneManagementInstallerSO : ScriptableObjectInstaller<SceneManagementInstallerSO>
    {
        [SerializeField] private GameSceneLoaderConfig _sceneLoaderConfig;

        public override void InstallBindings()
        {
            BindGameSceneLoader(_sceneLoaderConfig);
        }

        private void BindGameSceneLoader(GameSceneLoaderConfig sceneLoaderConfig)
        {
            AddressableGameSceneLoader sceneLoader = new AddressableGameSceneLoader(sceneLoaderConfig);

            Container
                .Bind<IGameSceneLoader>()
                .To<AddressableGameSceneLoader>()
                .FromInstance(sceneLoader)
                .AsSingle()
                .NonLazy();
        }
    }
}