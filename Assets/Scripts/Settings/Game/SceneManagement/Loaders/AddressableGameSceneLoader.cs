
using System;
using Cysharp.Threading.Tasks;
using Game.Settings.AssetsManagement;
using Game.Settings.Common.Events;
using Game.Utilities.Logging;
using Game.UI.Screens;
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
        private ICustomEvent<IEventListener, EventArgs> _sceneSwitchEvent;

        private GameSceneData _mainMenuScene;
        private AssetReference _loadingScreenAssetRef;

        private (GameObject instance, AsyncOperationHandle handle) _loadingScreenLoadInfo;
        private bool _isLoading;

        #endregion


        #region Properties 

        public IConcreteSceneLoader<AssetReference, AsyncOperationHandle<SceneInstance>> SceneLoader => _sceneLoader;
        public ICustomEvent<IEventListener, EventArgs> SwitchEvent => _sceneSwitchEvent;
        public bool IsLoading => _isLoading; 

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
            _sceneSwitchEvent = new SceneSwitchEvent();
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

            _loadingScreenAssetRef = config.LoadingScreenAssetRef;
        }

        #endregion

        public UniTask LoadSceneAsync(GameSceneData sceneData, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            if (loadMode == LoadSceneMode.Single)
            {
                SwitchEvent?.RaiseEvent(this, EventArgs.Empty);
            }
            return SceneLoader.LoadScene(sceneData.SceneReference, loadMode).ToUniTask();
        }


        public UniTask LoadMainMenuAsync(bool useLoadingScreen = true)
        {
            return LoadAdditiveSceneCleanAsync(_mainMenuScene, useLoadingScreen);
        }

        public UniTask LoadLevelAsync(GameSceneData levelData)
        {
            if (levelData.SceneType == GameSceneData.GameSceneType.GameLevel)
            {
                return LoadAdditiveSceneCleanAsync(levelData, true);
            }
            else
            {
                string errorMsg = $" # Fatal Error : Can not load level! Scene type : {levelData.SceneType} != {GameSceneData.GameSceneType.GameLevel} | {this}";
                CustomLogger.LogError(errorMsg);
                throw new ArgumentException(errorMsg);
            }
        }

        private UniTask LoadAdditiveSceneCleanAsync(GameSceneData targetSceneData, bool useLoadingScreen = true)
        {
            Func<AsyncOperationHandle<SceneInstance>> sceneLoadingLogic = () =>
            {
                SwitchEvent?.RaiseEvent(this, EventArgs.Empty);
                SceneLoader.UnloadAll();
                return SceneLoader.LoadScene(targetSceneData.SceneReference, LoadSceneMode.Additive);
            };
            return LoadSceneAsync(sceneLoadingLogic, useLoadingScreen);

        }

        private async UniTask LoadSceneAsync(Func<AsyncOperationHandle<SceneInstance>> sceneLoadingLogic, bool useLoadingScreen = true)
        {
            _isLoading = true;

            if (sceneLoadingLogic == null)
            {
                return;
            }

            LoadingScreenUI loadingScreen = null;

            if (useLoadingScreen)
            {
                loadingScreen = await CreateLoadingScreen(_loadingScreenAssetRef);
                loadingScreen.Initialize();
            }

            var loadHandles = sceneLoadingLogic.Invoke();

            if (loadingScreen != null)
            {
                await ExecuteLoadingScreen(loadingScreen, loadHandles);
            }

            _isLoading = false;
        }

        #region Loading Screen

        private async UniTask<LoadingScreenUI> CreateLoadingScreen(AssetReference loadingScreenRef)
        {
            AsyncOperationHandle<GameObject> loadingScreenHandle = AddressableAssetsHandler.Instance.TryLoadAssetAsync<GameObject>(loadingScreenRef);
            await loadingScreenHandle.Task;

            GameObject loadingScreenObj = GameObject.Instantiate(loadingScreenHandle.Result);
            loadingScreenObj.name = nameof(LoadingScreenUI);
            GameObject.DontDestroyOnLoad(loadingScreenObj);

            _loadingScreenLoadInfo = (loadingScreenObj, loadingScreenHandle);

            return loadingScreenObj.GetComponent<LoadingScreenUI>();
        }

        private async UniTask ExecuteLoadingScreen(LoadingScreenUI loadingScreen, AsyncOperationHandle<SceneInstance> sceneLoadHandle)
        {
            while (!sceneLoadHandle.IsDone)
            {
                loadingScreen.SetLoadingProgress(sceneLoadHandle.PercentComplete);
                await UniTask.Yield();
            }

            loadingScreen.SetFullProgress();
            await UniTask.WaitUntil(() => loadingScreen.IsFullProgress && !loadingScreen.IsUpdating);

            await loadingScreen.PrepareForDeactivation();
            RemoveLoadingScreen();
        }

        private void RemoveLoadingScreen()
        {
            if (_loadingScreenLoadInfo.instance != null)
            {
                GameObject.Destroy(_loadingScreenLoadInfo.instance);
                AddressableAssetsHandler.Instance.UnloadAsset(_loadingScreenLoadInfo.handle);
            }
        }

        #endregion

        #endregion

    }
}
