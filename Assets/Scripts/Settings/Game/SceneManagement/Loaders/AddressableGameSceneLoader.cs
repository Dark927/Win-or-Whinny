
using System;
using Cysharp.Threading.Tasks;
using Game.Utilities.Logging;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Game.Settings.SceneManagement
{
    /// <summary>
    /// This is a main class to load scenes.
    /// </summary>
    public class AddressableGameSceneLoader : IGameSceneLoader
    {
        #region Fields

        private IConcreteSceneLoader<AssetReference, AsyncOperationHandle<SceneInstance>> _sceneLoader;

        private GameSceneData _mainMenuScene;
        private AssetReference _loadingScreenAssetReference;

        #endregion


        #region Properties 

        public IConcreteSceneLoader<AssetReference, AsyncOperationHandle<SceneInstance>> SceneLoader => _sceneLoader;

        #endregion


        #region Methods

        #region Init

        public AddressableGameSceneLoader(GameSceneLoaderConfig config)
        {
            if (config == null)
            {
                CustomLogger.LogError($" # {nameof(GameSceneLoaderConfig)} is null! | Can not initialize {nameof(AddressableGameSceneLoader)} | {this.ToString()}");
                return;
            }

            _sceneLoader = new AddressableSceneLoader();
            ConfigureLoader(config);
        }

        private void ConfigureLoader(GameSceneLoaderConfig config)
        {
            _mainMenuScene = config.MainMenuSceneData;
            if (_mainMenuScene != null && (_mainMenuScene.SceneType != GameSceneData.GameSceneType.Menu))
            {
                _mainMenuScene = null;
                CustomLogger.LogWarning($"<color=yellow> # Warning :</color> {nameof(_mainMenuScene)} {nameof(GameSceneData.GameSceneType)} is not Menu!");
            }

            _loadingScreenAssetReference = config.LoadingScreenAssetRef;
        }

        #endregion

        public UniTask LoadSceneAsync(GameSceneData sceneData, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            return SceneLoader.LoadScene(sceneData.SceneReference, loadMode).ToUniTask();
        }

        public UniTask LoadMainMenuAsync(bool useLoadingScreen = true)
        {
            Func<AsyncOperationHandle<SceneInstance>> sceneLoadingLogic = () =>
            {
                SceneLoader.UnloadAll();
                return SceneLoader.LoadScene(_mainMenuScene.SceneReference, LoadSceneMode.Additive);
            };

            return LoadSceneAsync(sceneLoadingLogic, useLoadingScreen);
        }


        private async UniTask LoadSceneAsync(Func<AsyncOperationHandle<SceneInstance>> sceneLoadingLogic, bool useLoadingScreen = true)
        {
            if (sceneLoadingLogic == null)
            {
                return;
            }

            var sceneLoadHandle = await sceneLoadingLogic.Invoke();

            LoadingScreenUI loadingScreen = null;

        }
    }

    #endregion
}
